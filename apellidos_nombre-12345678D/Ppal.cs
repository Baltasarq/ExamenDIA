namespace Codigo {
	using System;

	class Ppal {
        public const string NotesFileName = "notas.txt";
    
		static void Main()
		{
			Console.WriteLine( "Nombre: " + Info.Nombre );
            Console.WriteLine( "Email: " + Info.Email );
			Console.WriteLine( "DNI: " + Info.Dni );
			Console.WriteLine( "===" );

			Pregunta1.Ppal.Prueba();
			Pregunta2.Ppal.Prueba();
			Pregunta3.Ppal.Prueba();
			Pregunta4.Ppal.Prueba();
			Pregunta5.Ppal.Prueba();
			Pregunta6.Ppal.Prueba();

            Console.WriteLine( "\n" );
            
            if ( Info.Dni == Info.InvalidData ) {
                Console.WriteLine( "** Recuerda guardar tus datos en: '"
                                    + Info.InfoFile
                                    +"' **\n" );
            }
		}
	}
}
