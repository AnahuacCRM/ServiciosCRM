using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
  public  class Pais : BusinessTypeBase
    {
       
        [Required(AllowEmptyStrings = false, ErrorMessage = "La clave del pais es requerido")]
        [MaxLength(256, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Clave { get; set; }

        public string Descripcion { get; set; }

    }
}
