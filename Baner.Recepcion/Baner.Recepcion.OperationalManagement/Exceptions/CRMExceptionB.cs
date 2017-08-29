using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement.Exceptions
{
    [Serializable]
    public class CRMExceptionB : Exception
    {
        private static string ConfigurationSettingsExceptionMessage = "Error al Mapear atributo Lookup";

        public string Errors { get; private set; }
        public CRMExceptionB(string error) : base(string.Format("{0} - {1}", ConfigurationSettingsExceptionMessage, error))
        {
            Errors = error;

        }

        public CRMExceptionB(Exception inner) : base(ConfigurationSettingsExceptionMessage, inner)
        {

        }

        public CRMExceptionB(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CRMExceptionB(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
