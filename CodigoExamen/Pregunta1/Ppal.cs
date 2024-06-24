// ExamenDIA (c) 2021 Baltasar MIT License <baltasarq@uvigo.es>


namespace CodigoExamen.Pregunta1 {
    using System;

	public static class Ppal {
		public static void Prueba()
		{
			string NamespaceName = typeof( Ppal ).Namespace ?? "Pregunta1";

			Console.WriteLine( "\n\n" + NamespaceName.Substring( NamespaceName.LastIndexOf( '.' ) + 1 ) );
			Console.WriteLine( "=========" );
			Console.WriteLine( @"
				Esta es la plantilla para tu examen de DIA.

				Debe funcionar tanto en Rider como en Visual Studio.
				Si tienes que compartir fuentes entre preguntas
				(p.ej., porque se pide ampliar una clase ya creada),
				puedes hacerlo en la misma pregunta,
				utilizando using y partial (si es necesario), o
				incluso copiar la clase de la pregunta anterior
				en la nueva.

				Tienes creado aparte un proyecto para tests.

				Buena suerte,
				-- Baltasar García, jbgarcia@uvigo.es" );
		}
	}
}
