using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Colonia
    {
        public Guid IdCRM { get; set; }
        public string Nombre { get; set; }
        public string CP { get; set; }
        public string DelegacionMunicipio { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}
