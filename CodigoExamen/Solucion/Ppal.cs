// ExamenDIA (c) 2021 Baltasar MIT License <baltasarq@uvigo.es>


namespace CodigoExamen.Solucion;
    
using System;


public static class Ppal {
	public static void Prueba()
	{
		string NamespaceName = typeof( Ppal ).Namespace ?? "Solucion";

		Console.WriteLine( "\n\n" + NamespaceName.Substring( NamespaceName.LastIndexOf( '.' ) + 1 ) );
		Console.WriteLine( "=========" );
		Console.WriteLine( @"
			Esta es la plantilla para tu examen de DIA.

			Funcionará tanto en Rider como en Visual Studio.
			Creará un archivo ZIP en el escritorio con el código fuente.
			¡Para que funcione tiene que compilarse y ejecutarse el proyecto!
			Tienes creado aparte un proyecto para tests.

			Buena suerte,
			-- Baltasar García, jbgarcia@uvigo.es" );
	}
}
