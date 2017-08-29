using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Admitido : BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string id_oportunidad { get; set; }



        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
       // [Range(1, 99, ErrorMessage = "El atributo {0} esta fuera de rango.")]
        public string StatusOpo { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string DesicionAdmision { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(12, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Programa { get; set; }

       

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(6, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Campus { get; set; }

       
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Escuela { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string TipoAdmision { get; set; }

       

    }
}
