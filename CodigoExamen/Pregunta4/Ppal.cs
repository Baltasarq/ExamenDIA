// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen.Pregunta4 {
    using System;

	public static class Ppal {
		public static void Prueba()
		{
			string namespaceName = typeof( Ppal ).Namespace ?? "Pregunta4";

			Console.WriteLine( "\n\n" + namespaceName.Substring( namespaceName.LastIndexOf( '.' ) + 1 ) );
			Console.WriteLine( "=========" );

			Console.WriteLine( "Prueba que tu respuesta funciona." );
		}
	}
}

