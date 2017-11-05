namespace Codigo {
    using System.IO;

	public static class Info {
		public static string Nombre {
            get {
                if ( string.IsNullOrEmpty( apellidos ) ) {
                    Retrieve();
                }
                
                return apellidos + ", " + nombre;
            }
        }
        
        public static string Email {
            get {
                if ( string.IsNullOrEmpty( email ) ) {
                    Retrieve();
                }
                
                return email;
            }
        }
        
		public static string Dni  {
            get {
                if ( string.IsNullOrEmpty( dni ) ) {
                    Retrieve();
                }
                
                return dni;
            }
        }

        private static void Retrieve()
        {
            using (var f = new StreamReader( "usr_exam_data.txt" )) {
                dni = f.ReadLine();
                nombre = f.ReadLine();
                apellidos = f.ReadLine();
                email = f.ReadLine();
            }
            
            return;
        }        
        
        private static string nombre;
        private static string apellidos;
        private static string dni;
        private static string email;
	}
}
