using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class FechaExamenAdmision : BusinessTypeBase
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracter.")]
        public string id_Oportunidad { get; set; }
        /// <summary>
        /// Identificador de Banner.
        /// </summary>
        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //[MaxLength(9, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        //public string IdBanner { get; set; }

        /// <summary>
        /// Numero de la solicitud.
        /// </summary>
        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //[Range(1, 99, ErrorMessage = "El atributo {0} esta fuera de rango.")]
        //public int NumeroSolicitud { get; set; }

        /// <summary>
        /// Session del examen.
        /// </summary>
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(1, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracter.")]
        public string SessionExamen { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        public List<Examenes> lstExamenes { get; set; }

        /// <summary>
        /// VPDI
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        //[MaxLength(4, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        //public string VPDI { get; set; }

        /// <summary>
        /// Periodo 
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo es requerido")]
        //[MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        //public string Periodo { get; set; }
    }
}
