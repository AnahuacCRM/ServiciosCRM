using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class GestionContactosWarning
    {
        public GestionContactosWarning()
        {
            Warnings = new List<string>();
        }
     
        public List<string> Warnings { get; set; }
       
        
    }
}
