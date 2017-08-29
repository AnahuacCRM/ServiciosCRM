using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{

    public class Examenes : BusinessTypeBase
    {
        /// <summary>
        /// Clave del examen.
        /// </summary>
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(3, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string ClaveExamen { get; set; }

        /// <summary>
        /// Fecha de Examen de Admision.
        /// </summary>
        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        public CustomDate FechaExamen { get; set; }


    }
}
