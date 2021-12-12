// ExamenDIA (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace CodigoExamen {
    using System.Linq;
    
    /// <summary>Guarda la info del estudiante: dni, nombre, email.</summary>
	public class InfoEstudiante {
        public const string InvalidData = "n/a";

        public InfoEstudiante()
        {
            this.NombrePropio =
                this.Apellidos =
                    this.Dni =
                        this.Email = InvalidData;
        }

        /// <summary>Nombre completo del alumno.</summary>
		public string Nombre => this.Apellidos + ", " + this.NombrePropio;

        public string Apellidos {
            get; init;
        }
        
        public string NombrePropio {
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
            string apellidos = this.Apellidos
                .Replace( "_", "" )
                .Replace( "-", "" );
            
            string toret = $"{apellidos}_{this.NombrePropio}-{this.Dni}";
            
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
            
            if ( obj is InfoEstudiante info ) {
                toret = this.Dni == info.Dni;
            }

            return toret;
        }

        public override string ToString()
        {
            return $"Nombre: {this.Nombre}"
                   + $"\nEmail: {this.Email}" 
                   + $"\nDNI: {this.Dni}";
        }
    }
}
