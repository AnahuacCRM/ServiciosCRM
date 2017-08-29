
using Baner.Recepcion.OperationalManagement.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement
{
    public class EventLogger : ILogger
    {
        private const string source = "ANA.CRM";

        public void Error(Exception ex)
        {
            string message = string.Format("{0} : Error ocurrido {1} detalle: {2}", source, DateTime.Now, ex.Build());
            EventLog.WriteEntry(source, message, EventLogEntryType.Error);
        }

        public void Error(string error)
        {
            EventLog.WriteEntry(source, error, EventLogEntryType.Error);
        }

        public void Infomacion(string mensaje)
        {
            EventLog.WriteEntry(source, mensaje, EventLogEntryType.Information);
        }
    }
}
