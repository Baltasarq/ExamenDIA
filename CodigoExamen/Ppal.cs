// ExamenDIA (c) 2021 Baltasar MIT License <baltasarq@uvigo.es>


namespace CodigoExamen;


using System;
using System.IO;


static class Ppal {
	static void ZipIt(string srcPath, string destPath, StudentInfo info)
	{
		string destFilePath = Path.Combine( destPath, info.GetStrId() + ".zip" );
		Zipper.ZipDir( srcPath, destFilePath );
	}

	static void Main()
	{
		bool corrigiendo = false;

		Console.WriteLine( "# Examen DIA\n");

		var info = PersistStudentInfo.Retrieve();

		if ( !corrigiendo ) {
			if ( info is null ) {
				info = PersistStudentInfo.StudentInfoFromConsole();
				var persist = new PersistStudentInfo( info );

				persist.Save();
				persist.CreateMarksFile();
			}

			// Comprime el examen en el escritorio
			string srcPath = DetermineSlnDir();
			string homePath = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile );

			ZipIt( srcPath, homePath, info );
		}

		// Ejecuta el examen
		Console.WriteLine( "\n\n===" );
		Console.WriteLine( info );
		Console.WriteLine( "===" );

		Solucion.Ppal.Prueba();

		Console.WriteLine( "\n" );
	}

	private static string DetermineSlnDir()
	{
		const string SolutionName = "examendia";
		string toret = Directory.GetCurrentDirectory();
		int pos = toret.ToLower().LastIndexOf( SolutionName );

		if ( pos >= 0 ) {
			pos += SolutionName.Length;
			toret = toret.Substring( 0, pos );
		}

		return toret;
	}
}
