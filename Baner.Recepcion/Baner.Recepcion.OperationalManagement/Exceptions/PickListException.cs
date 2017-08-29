using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement.Exceptions
{
    [Serializable]
    public class PickListException : Exception
    {
        private static string ConfigurationSettingsExceptionMessage = "Error al Mapear atributo picklist";

        public string Errors { get; private set; }
        public PickListException(string error) : base(string.Format("{0} - {1}", ConfigurationSettingsExceptionMessage, error))
        {
            Errors = error;

        }

        public PickListException(Exception inner) : base(ConfigurationSettingsExceptionMessage, inner)
        {

        }

        public PickListException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PickListException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
