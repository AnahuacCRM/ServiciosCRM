using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
   public class Rechazado : BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima de id_Oportunidad es de {1} caracteres")]
        public string id_Oportunidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de cuenta es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima de id_Cuenta es de {1} caracteres")]
        public string id_Cuenta { get; set; }
       

      

        /// <summary>
        /// Decision de Admisión
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "La Decision de Admisión es requerida")]
        [MaxLength(2, ErrorMessage = "La longitud máxima de Decision de Admisión es de {1} caracteres")]
        public string DecisionAdmision { get; set; }

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
