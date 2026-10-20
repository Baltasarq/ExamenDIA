// ExamenDIA (c) 2021 Baltasar MIT License <baltasarq@uvigo.es>


namespace CodigoExamen;


using System.IO.Compression;
using System.Text;


public static class Zipper {
    public static void ZipDir(string orgPath, string destFilePath)
    {
        string tempDirId = Convert.ToString( new Random().Next() );
        string tempDir = Path.Combine( Path.GetTempPath(), tempDirId );

        try {
            var files = Directory.GetFiles(
                                    orgPath,
                                    "*.cs",
                                    SearchOption.AllDirectories );

            foreach(var f in files) {
                var relativePath = Path.GetRelativePath( orgPath, f );
                var destCopyPath = Path.Combine( tempDir, relativePath );

                Directory.CreateDirectory(
                            Path.GetDirectoryName( destCopyPath )! );
                File.Copy(f, destCopyPath, overwrite: true);
            }

            ZipFile.CreateFromDirectory(
                        sourceDirectoryName: tempDir,
                        destinationArchiveFileName: destFilePath,
                        compressionLevel: CompressionLevel.Fastest,
                        includeBaseDirectory: true,
                        entryNameEncoding: Encoding.UTF8 );
        } catch(IOException) {
            ZipFile.CreateFromDirectory(
                sourceDirectoryName: orgPath,
                destinationArchiveFileName: destFilePath,
                compressionLevel: CompressionLevel.Fastest,
                includeBaseDirectory: true,
                entryNameEncoding: Encoding.UTF8 );
        }
        finally {
            Directory.Delete( tempDir, recursive: true );
        }
    }
}
