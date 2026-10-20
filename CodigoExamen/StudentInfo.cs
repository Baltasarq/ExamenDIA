// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen {
    using System.Linq;

    /// <summary>Guarda la info del estudiante: dni, nombre, email.</summary>
	public class StudentInfo {
        public const string InvalidData = "n/a";

        public StudentInfo()
        {
            this.FirstName =
                this.Surname =
                    this.Dni =
                        this.Email = InvalidData;
        }

        /// <summary>Nombre completo del alumno.</summary>
		public string CompleteName => this.Surname + ", " + this.FirstName;

        public string Surname {
            get; init;
        }

        public string FirstName {
            get; init;
        }

        public string Email {
            get; init;
        }

        public string Dni {
            get; init;
        }

        public string GetStrId()
        {
            string surname = this.Surname
                .Replace( "_", "" )
                .Replace( "-", "" );

            string toret = $"{surname}_{this.FirstName}-{this.Dni}";

            // Convierte a ASCII
            toret = string.Join( "", toret.ToCharArray().Where( x => x < 127 ) );

            // Eliminar caracteres ilegales
            toret = toret.Trim().ToLower()
                .Replace( " ", "" )
                .Replace( "/", "" )
                .Replace( "\\", "" )
                .Replace( "?", "" )
                .Replace( "<", "" )
                .Replace( ">", "" )
                .Replace( "*", "" )
                .Replace( "|", "" )
                .Replace( ":", "" )
                .Replace( "^", "" )
                .Replace( ".", "" )
                .Replace( ",", "" );

            return toret;
        }

        public override int GetHashCode() => this.Dni.GetHashCode();

        public override bool Equals(object? obj)
        {
            bool toret = false;

            if ( obj is StudentInfo info ) {
                toret = ( this.Dni == info.Dni );
            }

            return toret;
        }

        public override string ToString()
        {
            return $"Nombre: {this.CompleteName}"
                   + $"\nEmail: {this.Email}"
                   + $"\nDNI: {this.Dni}";
        }
    }
}
