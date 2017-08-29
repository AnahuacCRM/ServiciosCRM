
using Baner.Recepcion.OperationalManagement.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement
{
    public class DiagnosticsLogger : ILogger
    {
        public void Error(string error)
        {
            Trace.TraceError(error);
        }

        public void Error(Exception ex)
        {
            Trace.TraceError(ex.Build());
        }

        public void Infomacion(string mensaje)
        {
            Trace.Write(mensaje);
        }
    }
}

