using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class PreOportunidad : BusinessTypeBase
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        public Guid LeadId { get; set; }


        
        public string IdBanner { get; set; }
    }
}
