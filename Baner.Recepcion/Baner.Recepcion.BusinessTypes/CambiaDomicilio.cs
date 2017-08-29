using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class CambiaDomicilio : BusinessTypeBase
    {
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(9, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string IdBanner { get; set; }

        public List<Domicilio> lstDomicilio { get; set; }
    }
}
