using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Correo : BusinessTypeBase
    {
        [MaxLength(4, ErrorMessage = "La longitud maxima del tipo de correo electrónico es de {1} caracteres")]
        public string TipoCorreoElectronicoId { get; set; }

        [MaxLength(128, ErrorMessage = "La longitud maxima del correo electrónico es de {1} caracteres")]
        public string Correo_Electronico { get; set; }

        [MaxLength(1, ErrorMessage = "La longitud maxima de si es preferido el correo electrónico es de {1} caracteres")]
        public string IndPreferido { get; set; }

        
    }
}
