using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Domicilio : BusinessTypeBase
    {
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(6, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string VPDI { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string TipoDireccion { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string SecuenciaDireccion { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(1, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        [RegularExpression("^(?:I|U|D)$", ErrorMessage = "El atributo {0} solo acepta los valores I, U, D.")]
        public string TipoOperacion { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Calle1 { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Calle2 { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Colonia { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //[MaxLength(50, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        //public string Ciudad { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(3, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Estado { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(30, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string CP { get; set; }

        [MaxLength(5, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Municipio { get; set; }

        [MaxLength(5, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Pais { get; set; }

    }
}
