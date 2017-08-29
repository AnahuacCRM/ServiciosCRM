using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
   public class Oportunidad: BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo id_cuenta es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima de la id_cuenta es de {1} caracteres")]
        public string id_Cuenta { get; set; }

       
        [MaxLength(36, ErrorMessage = "La longitud máxima de la id_Oporunidad es de {1} caracteres")]
        public string id_Oportunidad { get; set; }

        public int? Numero_Solicitud { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo VPD es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del atributo VPD es de {1} caracteres")]
        public string VPD { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo Campus es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del Atributo Campus es de {1} caracteres")]
        public string Campus { get; set; }

       
        [MaxLength(2, ErrorMessage = "La longitud máxima del estatus de la solicitud es de {1} caracteres")]
        public string Estatus_Solicitud { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(6, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Periodo { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Nivel { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El programa es requerido")]
        [MaxLength(12, ErrorMessage = "La longitud máxima del programa es de {1} caracteres")]
        public string Programa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La escuela es requerido")]
        [MaxLength(2, ErrorMessage = "La escuela es de {1} caracteres")]
        public string Escuela { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo de alumno es requerido")]
        [MaxLength(1, ErrorMessage = "La longitud máxima del tipo de alumno es de {1} caracter")]
        public string Codigo_Tipo_Alumno { get; set; }
        //6s
        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo admision es requerido")]
        [MaxLength(2, ErrorMessage = "La longitud máxima del tipo admision es de {1} caracteres")]
        public string Codigo_Tipo_admision { get; set; }
    }
}
