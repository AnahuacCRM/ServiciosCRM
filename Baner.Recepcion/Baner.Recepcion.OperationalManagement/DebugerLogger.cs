

using Baner.Recepcion.OperationalManagement.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement
{
    public class DebugerLogger : ILogger
    {
        private const string source = "RhinoLogger";
        public void Error(string error)
        {

            Debug.WriteLine(error);
        }

        public void Error(Exception ex)
        {
            string message = string.Format("{0} : Error ocurrido {1} detalle: {2}", source, DateTime.Now, ex.Build());
            Debug.WriteLine(message);
        }

        public void Infomacion(string mensaje)
        {
            Debug.WriteLine(mensaje);
        }
    }
}
