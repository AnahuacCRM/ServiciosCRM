using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
  public  class Inscrito: BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id de Oportunidad es de {1} caracteres")]
        public string id_Oportunidad { get; set; }
       

        /// <summary>
        /// Fecha pago
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Fecha Pago Inscripcion es requerido")]
        public CustomDate FechaPagoInscripcion { get; set; }

        
    }
}
