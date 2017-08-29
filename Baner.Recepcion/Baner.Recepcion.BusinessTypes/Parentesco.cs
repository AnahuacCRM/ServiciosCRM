using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Parentesco: BusinessTypeBase
    {     
        /// <summary>
        /// Identificador del registro en Baner
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de la cuena es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id de cuenta es de {1} caracteres")]
        public string id_Cuenta { get; set; }

        [Required(ErrorMessage = "Debe tener registrada información de parentesco")]
        public List<PadreoTutor> PadreoTutor { get; set; }
    }

}
