// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen.Pregunta3 {
    using System;

	public static class Ppal {
		public static void Prueba()
		{
			string NamespaceName = typeof( Ppal ).Namespace ?? "Pregunta3";

			Console.WriteLine( "\n\n" + NamespaceName.Substring( NamespaceName.LastIndexOf( '.' ) + 1 ) );
			Console.WriteLine( "=========" );

			Console.WriteLine( "Prueba que tu respuesta funciona." );
		}
	}
}

