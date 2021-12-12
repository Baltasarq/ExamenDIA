﻿// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


using System.Text;

namespace CodigoExamen {
	using System;
	using System.IO;
	using System.IO.Compression;

	
	class Ppal {
		static void ZipIt(string desktopPath, InfoEstudiante info)
		{
			string nf = "examen-" + info.GetStrId();
				
			ZipFile.CreateFromDirectory(
				sourceDirectoryName: ".",
				destinationArchiveFileName: Path.Combine( desktopPath, nf ),
				compressionLevel: CompressionLevel.Fastest,
				includeBaseDirectory: true,
				entryNameEncoding: Encoding.UTF8 );
		}
		
		static void Main()
		{
			var info = PersisteInfoEstudiante.Recupera();

			if ( info is null ) {
				info = PersisteInfoEstudiante.creaDesdeConsola();
				var persiste = new PersisteInfoEstudiante( info );
				
				persiste.Save();
				persiste.CreaArchivoNotas();
			}
			
			Console.WriteLine( "\n\n===" );
			Console.WriteLine( info );
			Console.WriteLine( "===" );

			Pregunta1.Ppal.Prueba();
			Pregunta2.Ppal.Prueba();
			Pregunta3.Ppal.Prueba();
			Pregunta4.Ppal.Prueba();
			Pregunta5.Ppal.Prueba();

            Console.WriteLine( "\n" );
            
            // Comprime el examen en el escritorio
            string desktopPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
            
            ZipIt( desktopPath, info );
            Console.WriteLine( $"Examen comprimido creado en: {desktopPath}" );
		}
	}
}