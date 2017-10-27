namespace Codigo {
	using System;
    using System.IO;

	class Ppal {
        public const string NotesFileName = "notas.txt";
    
		static void Main()
		{
			Console.WriteLine( "Nombre: " + Info.Nombre );
            Console.WriteLine( "Email: " + Info.Email );
			Console.WriteLine( "DNI: {0}{1}", Info.Dni, Info.LetraDni );
			Console.WriteLine( "===" );

			Pregunta1.Ppal.Prueba();
			Pregunta2.Ppal.Prueba();
			Pregunta3.Ppal.Prueba();
			Pregunta4.Ppal.Prueba();
			Pregunta5.Ppal.Prueba();
			Pregunta6.Ppal.Prueba();
            
            if ( string.IsNullOrEmpty( Info.Nombre )
              || Info.Dni < 0 )
            {
                Console.WriteLine( "\n** Recuerda escribir tus datos en Info.cs **" );
            } else {
                if ( !File.Exists( NotesFileName ) ) {
                    using ( StreamWriter writer = File.CreateText( NotesFileName ) ) {
                        writer.WriteLine( "Nombre: " + Info.Nombre );
                        writer.WriteLine( "Email: " + Info.Email );
                        writer.WriteLine( "DNI: {0}{1}", Info.Dni, Info.LetraDni );
                        writer.WriteLine( "===" );
                        writer.WriteLine( "\nEjercicio 1: " );
                        writer.WriteLine( "Ejercicio 2: " );
                        writer.WriteLine( "Ejercicio 3: " );
                        writer.WriteLine( "Ejercicio 4: " );
                        writer.WriteLine( "Ejercicio 5: " );
                        writer.WriteLine( "Ejercicio 6: " );
                        writer.WriteLine();
                        writer.WriteLine( "Nota: " );
                    }
                    
                    Console.WriteLine( "\n**Archivo '{0}' creado. **", NotesFileName );
                }
            }
            
            Console.WriteLine( "\n" );
		}
	}
}
