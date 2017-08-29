using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement.Exceptions
{
    [Serializable]
    public class ConfigurationSettingsException : Exception
    {
        private static string ConfigurationSettingsExceptionMessage = "Error en configuracion de XRM";

        public string Errors { get; private set; }
        public ConfigurationSettingsException(string error) : base(string.Format("{0} - {1}", ConfigurationSettingsExceptionMessage, error))
        {
            Errors = error;

        }

        public ConfigurationSettingsException(Exception inner) : base(ConfigurationSettingsExceptionMessage, inner)
        {

        }

        public ConfigurationSettingsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigurationSettingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
