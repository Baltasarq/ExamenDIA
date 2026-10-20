// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen {
    using System;
    using System.IO;
    using System.Linq;
    using System.Globalization;


    /// <summary>Persistencia mediante archivo textual.</summary>
    public class PersistStudentInfo {
        public const string InfoFile = "usr_exam_data.txt";

        public PersistStudentInfo(StudentInfo info)
        {
            this.Info = info;
        }

        public void Save()
        {
            using (var f = new StreamWriter( InfoFile )) {
                f.WriteLine( this.Info.Dni );
                f.WriteLine( this.Info.Surname );
                f.WriteLine( this.Info.FirstName );
                f.WriteLine( this.Info.Email );
            }

            return;
        }

        public StudentInfo Info {
            get;
        }

        public void CreateMarksFile()
        {
            const int NumQuestions = 5;

            using (var f = new StreamWriter("notas.txt"))
            {
                f.WriteLine( "\n===");
                f.WriteLine( this.Info.Dni );
                f.WriteLine( this.Info.CompleteName );
                f.WriteLine( this.Info.Email );
                f.WriteLine( "===\n");

                for (int i = 0; i < NumQuestions; ++i) {
                    f.WriteLine( $"Pregunta {i + 1}: 0   " );
                }

                f.WriteLine( "\nNota:       0" );
            }

            return;
        }

        public static StudentInfo? Retrieve()
        {
            StudentInfo? toret;

            try {
                using (var f = new StreamReader( InfoFile )) {
                    string dni = f.ReadLine() ?? StudentInfo.InvalidData;
                    string surname = f.ReadLine() ?? StudentInfo.InvalidData;
                    string name = f.ReadLine() ?? StudentInfo.InvalidData;
                    string email = f.ReadLine() ?? StudentInfo.InvalidData;

                    dni = !string.IsNullOrEmpty( dni ) ? dni.Trim() : StudentInfo.InvalidData;
                    name = !string.IsNullOrEmpty( name ) ? name.Trim() : StudentInfo.InvalidData;
                    surname = !string.IsNullOrEmpty( surname ) ? surname.Trim() : StudentInfo.InvalidData;
                    email = !string.IsNullOrEmpty( email ) ? email.Trim() : StudentInfo.InvalidData;

                    toret = new StudentInfo {
                        FirstName = name,
                        Surname = surname,
                        Dni = dni,
                        Email = email
                    };
                }
            } catch(IOException) {
                toret = null;
            }

            return toret;
        }

        public static StudentInfo StudentInfoFromConsole()
        {
            var ti = CultureInfo.CurrentCulture.TextInfo;
            string dniPrefix = StrFromConsole( "Letra prefijo DNI (solo extranjeros): ", minChars: 0, maxChars: 1 );
            string dni = StrFromConsole( "DNI (solo números): ", minChars: 8, maxChars: 10, isNum: true );
            string dniLetter = StrFromConsole( "Letra DNI: ", minChars: 1, maxChars: 1 );
            string surname = StrFromConsole( "Apellidos: ", minChars: 4, maxChars: 80 );
            string name = StrFromConsole( "Nombre: ", minChars: 4, maxChars: 80 );
            string email = StrFromConsole( "e.mail: ", minChars: 4, maxChars: 80 );

            if ( email.Count( ch => ch == '@' ) != 1 ) {
                email = StrFromConsole( "e.mail: ", minChars: 4, maxChars: 80 );
            }

            return new StudentInfo {
                FirstName = ti.ToTitleCase( name ),
                Surname = ti.ToTitleCase( surname ),
                Dni = ( dniPrefix + dni + dniLetter ).ToUpper(),
                Email = email.ToLower()
            };
        }

        private static string StrFromConsole(
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
