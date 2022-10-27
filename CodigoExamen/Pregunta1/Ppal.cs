// ExamenDIA (c) 2021 Baltasar MIT License <baltasarq@uvigo.es>


namespace CodigoExamen.Pregunta1 {
    using System;

	public static class Ppal {
		public static void Prueba()
		{
			string NamespaceName = typeof( Ppal ).Namespace ?? "Pregunta1";

			Console.WriteLine( "\n\n" + NamespaceName.Substring( NamespaceName.LastIndexOf( '.' ) + 1 ) );
			Console.WriteLine( "=========" );
			Console.WriteLine( Pregunta1.Explicacion );
		}
	}
}
