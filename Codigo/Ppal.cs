namespace Codigo {
	using System;

	class Ppal {
		static void Main()
		{
			Console.WriteLine( "Nombre: " + Info.Nombre );
			Console.WriteLine( "DNI: {0}{1}", Info.Dni, Info.LetraDni );
			Console.WriteLine( "===" );

			Pregunta1.Ppal.Prueba();
			Pregunta2.Ppal.Prueba();
			Pregunta3.Ppal.Prueba();
			Pregunta4.Ppal.Prueba();
			Pregunta5.Ppal.Prueba();
			Pregunta6.Ppal.Prueba();
		}
	}
}
