using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement.Exceptions
{
    [Serializable]
    public class LookupException : Exception
    {
        private static string ConfigurationSettingsExceptionMessage = "Error al Mapear atributo Lookup";

        public string Errors { get; private set; }
        public LookupException(string error) : base(string.Format("{0} - {1}", ConfigurationSettingsExceptionMessage, error))
        {
            Errors = error;

        }

        public LookupException(Exception inner) : base(ConfigurationSettingsExceptionMessage, inner)
        {

        }

        public LookupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LookupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
