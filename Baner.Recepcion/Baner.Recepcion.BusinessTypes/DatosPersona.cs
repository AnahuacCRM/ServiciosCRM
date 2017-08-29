using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class DatosPersona : BusinessTypeBase
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string id_Cuenta { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido.")]
        //[MaxLength(9, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        //public string IdBanner { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(60, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Nombre { get; set; }

        [MaxLength(60, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Segundo_Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(60, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Apellidos { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        public CustomDate Fecha_Nacimiento { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(1, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Sexo { get; set; }

        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Nacionalidad { get; set; }

        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Religion { get; set; }


        [MaxLength(1, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Estado_Civil { get; set; }

    }
}
