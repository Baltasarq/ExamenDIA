namespace Codigo {
    using System.IO;

	public static class Info {
        public const string InvalidData = "n/a";
        public const string InfoFile = "usr_exam_data.txt";
    
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
            try {
	            using (var f = new StreamReader( InfoFile )) {
	                dni = f.ReadLine();
	                nombre = f.ReadLine();
	                apellidos = f.ReadLine();
	                email = f.ReadLine();
	            }
            } catch(IOException) {
                dni = InvalidData;
                nombre = InvalidData;
                apellidos = InvalidData;
                email = InvalidData;
            }
            
            return;
        }        
        
        private static string nombre;
        private static string apellidos;
        private static string dni;
        private static string email;
	}
}
