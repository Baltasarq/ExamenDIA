// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen {
    using System;
    using System.IO;
    
    
    /// <summary>Persistencia mediante archivo textual.</summary>
    public class PersisteInfoEstudiante {
        public const string InfoFile = "usr_exam_data.txt";
        
        public PersisteInfoEstudiante(InfoEstudiante info)
        {
            this.Info = info;
        }
        
        public void Save()
        {
            using (var f = new StreamWriter( InfoFile )) {
                f.WriteLine( this.Info.Dni );
                f.WriteLine( this.Info.Apellidos );
                f.WriteLine( this.Info.NombrePropio );
                f.WriteLine( this.Info.Email );
            }

            return;
        }

        public InfoEstudiante Info {
            get;
        }
        
        public void CreaArchivoNotas()
        {
            const int NumPreguntas = 5;
			
            using (var f = new StreamWriter("notas.txt"))
            {
                f.WriteLine( "\n===");
                f.WriteLine( this.Info.Dni );
                f.WriteLine( this.Info.Nombre );
                f.WriteLine( this.Info.Email );
                f.WriteLine( "===\n");

                for (int i = 0; i < NumPreguntas; ++i) {
                    f.WriteLine( $"Pregunta {i + 1}:    " );
                }
				
                f.WriteLine( "\nNota:          " );
            }

            return;
        }

        public static InfoEstudiante? Recupera()
        {
            InfoEstudiante? toret;
            
            try {
                using (var f = new StreamReader( InfoFile )) {
                    string dni = f.ReadLine() ?? InfoEstudiante.InvalidData;
                    string apellidos = f.ReadLine() ?? InfoEstudiante.InvalidData;
                    string nombre = f.ReadLine() ?? InfoEstudiante.InvalidData;
                    string email = f.ReadLine() ?? InfoEstudiante.InvalidData;

                    dni = !string.IsNullOrEmpty( dni ) ? dni.Trim() : InfoEstudiante.InvalidData;
                    nombre = !string.IsNullOrEmpty( nombre ) ? nombre.Trim() : InfoEstudiante.InvalidData;
                    apellidos = !string.IsNullOrEmpty( apellidos ) ? apellidos.Trim() : InfoEstudiante.InvalidData;
                    email = !string.IsNullOrEmpty( email ) ? email.Trim() : InfoEstudiante.InvalidData;

                    toret = new InfoEstudiante {
                        NombrePropio = nombre,
                        Apellidos = apellidos,
                        Dni = dni,
                        Email = email
                    };
                }
            } catch(IOException) {
                toret = null;
            }

            return toret;
        }
        
        public static InfoEstudiante creaDesdeConsola()
        {
            string dni;
            string email;
            string nombre;
            string apellidos;
            
            Console.Write( "DNI: " );
            dni = Console.ReadLine()?.Trim().ToUpper() ?? InfoEstudiante.InvalidData;
            
            Console.Write( "Apellidos: " );
            apellidos = Console.ReadLine()?.Trim() ?? InfoEstudiante.InvalidData;
            
            Console.Write( "Nombre: " );
            nombre = Console.ReadLine()?.Trim() ?? InfoEstudiante.InvalidData;
            
            Console.Write( "e.mail: " );
            email = Console.ReadLine()?.Trim() ?? InfoEstudiante.InvalidData;

            return new InfoEstudiante {
                NombrePropio = nombre,
                Apellidos = apellidos,
                Dni = dni,
                Email = email
            };
        }
    }
}
