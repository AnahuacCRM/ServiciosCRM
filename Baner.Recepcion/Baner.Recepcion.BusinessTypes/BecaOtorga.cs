using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class BecaOtorga : BusinessTypeBase
    {
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public BecaClass Beca { get; set; }

        public CustomDate FechaOtorgaBeca { get; set; }

        public CustomDate FechaVencimientoBeca { get; set; }

    }

    public class BecaClass : BusinessTypeBase
    {
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(8, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string TipoBeca { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(30, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string DescripcionBeca { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(3, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string CampusVPDI { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(6, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Periodo { get; set; }
    }

}
