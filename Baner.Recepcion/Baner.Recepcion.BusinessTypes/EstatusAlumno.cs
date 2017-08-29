using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class EstatusAlumno : BusinessTypeBase
    {
      
       
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Id_Oportunidad { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(30, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Estatus { get; set; }

        
    }
}
