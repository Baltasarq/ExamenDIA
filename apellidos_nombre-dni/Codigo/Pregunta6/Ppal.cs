using System;

namespace Codigo.Pregunta6 {
	public static class Ppal {
		public static void Prueba()
		{
			string NamespaceName = typeof( Ppal ).Namespace;

			Console.WriteLine( "\n\n" + NamespaceName.Substring( NamespaceName.LastIndexOf( '.' ) + 1 ) );
			Console.WriteLine( "=========" );

			Console.WriteLine( "Prueba que tu respuesta funciona." );
		}
	}
}

