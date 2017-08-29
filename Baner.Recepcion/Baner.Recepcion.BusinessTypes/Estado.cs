using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
   public class Estado: BusinessTypeBase
    {
        
        public string Clave { get; set; }

        public string Descripcion { get; set; }

        public string Pais { get; set; }

    }
}
