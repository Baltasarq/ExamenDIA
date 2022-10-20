// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen {
    using System;
    using System.IO;
    using System.Linq;
    using System.Globalization;
    
    
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
            var ti = CultureInfo.CurrentCulture.TextInfo;
            string prefijoDNI = Pide( "Letra prefijo DNI (solo extranjeros): ", minChars: 0, maxChars: 1 );
            string dni = Pide( "DNI (solo números): ", minChars: 8, maxChars: 10, isNum: true );
            string letraDni = Pide( "Letra DNI: ", minChars: 1, maxChars: 1 );
            string apellidos = Pide( "Apellidos: ", minChars: 4, maxChars: 80 );
            string nombre = Pide( "Nombre: ", minChars: 4, maxChars: 80 );
            string email = Pide( "e.mail: ", minChars: 4, maxChars: 80 );
            
            if ( email.Count( ch => ch == '@' ) != 1 ) {
                email = Pide( "e.mail: ", minChars: 4, maxChars: 80 );
            }

            return new InfoEstudiante {
                NombrePropio = ti.ToTitleCase( nombre ),
                Apellidos = ti.ToTitleCase( apellidos ),
                Dni = ( prefijoDNI + dni + letraDni ).ToUpper(),
                Email = email.ToLower()
            };
        }

        private static string Pide(
            string msg,
            int minChars = 8,
            int maxChars = int.MaxValue,
            bool isNum = false)
        {
            int num;
            string? toret = null;

            do {
                if ( toret != null ) {
                    Console.WriteLine( $"ERROR: {minChars} < longitud < {maxChars}"
                                       + $"{( isNum ? " y deben ser dígitos" : "" )}" );
                }
                
                Console.Write( msg );
                toret = Console.ReadLine()?.Trim() ?? "";
            } while( toret.Length < minChars
                  || toret.Length > maxChars
                  || ( isNum && ( !int.TryParse( toret, out num ) ) ) );

            return toret;
        }
    }
}
