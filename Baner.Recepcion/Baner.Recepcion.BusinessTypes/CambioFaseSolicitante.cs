using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
  public  class CambioFaseSolicitante
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string id_oportunidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nume de Fase es requerido")]
        public int Fase { get; set; }

    }
}
