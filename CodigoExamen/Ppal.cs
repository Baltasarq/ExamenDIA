// ExamenDIA (c) 2021 Baltasar MIT License <baltasarq@uvigo.es>


namespace CodigoExamen {
	using System;
	using System.IO;
	using System.Text;
	using System.Globalization;
	using System.IO.Compression;


	class Ppal {
		static void ZipIt(string srcPath, string desktopPath, InfoEstudiante info)
		{
			string filePath = Path.Combine( desktopPath, info.GetStrId() + ".zip" );
				
			// Eliminar el archivo si existe
			if ( File.Exists( filePath ) ) {
				File.Delete( filePath );
			}
			
			ZipFile.CreateFromDirectory(
				sourceDirectoryName: srcPath,
				destinationArchiveFileName: filePath,
				compressionLevel: CompressionLevel.Fastest,
				includeBaseDirectory: true,
				entryNameEncoding: Encoding.UTF8 );
		}
		
		static void Main()
		{
			bool corrigiendo = false;
			
			Console.WriteLine( "# Examen DIA\n");
			
			// Recupera los datos del estudiante
			var info = PersisteInfoEstudiante.Recupera();

			if ( !corrigiendo ) {
				if ( info is null ) {
					info = PersisteInfoEstudiante.creaDesdeConsola();
					var persiste = new PersisteInfoEstudiante( info );
					
					persiste.Save();
					persiste.CreaArchivoNotas();
				}
				
				// Comprime el examen en el escritorio
				string srcPath = DetermineSrcDir(); 
				string desktopPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
	            
				ZipIt( srcPath, desktopPath, info );
				Console.WriteLine( $"Trabajando desde: {srcPath}" );
				Console.WriteLine( $"Examen comprimido creado en: {desktopPath}" );
			}
			
			// Ejecuta el examen
			Console.WriteLine( "\n\n===" );
			Console.WriteLine( info );
			Console.WriteLine( "===" );

			Pregunta1.Ppal.Prueba();
			Pregunta2.Ppal.Prueba();
			Pregunta3.Ppal.Prueba();
			Pregunta4.Ppal.Prueba();
			Pregunta5.Ppal.Prueba();

            Console.WriteLine( "\n" );
		}

		private static string DetermineSrcDir()
		{
			const string NombreArchivoSolucion = "ExamenDIA.sln";
			string toret = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;

			if ( !File.Exists( toret + NombreArchivoSolucion ) ) {
				throw new ApplicationException(
					"Dir de ejecución incorrecto."
							+ "\nEl proyecto no se está ejecutando "
							+ "en el directorio de la solución"
							+ "\nPor favor, revise la cfg. de ejecución "
							+ "del proyecto de manera que el "
							+ "directorio de trabajo ('Working Directory'), "
							+ "\napunte al directorio de la solución con: " + NombreArchivoSolucion
							+ "\nWorking directory: " + toret );
			}
			
			return toret;
		}
	}
}
