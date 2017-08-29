using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes.RespuestasServicio
{
    public class ResponseNewProspect
    {
        public ResponseNewProspect()
        {
            Warnings = new List<string>();
        }
        public Guid IdCRM { get; set; }
        public List<string> Warnings { get; set; }
        public Guid IdOportunidad { get; set; }
        public string Seguimientos { get; set; }
    }
}
