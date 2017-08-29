using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement.Exceptions
{
    [Serializable]
    public class InvalidExamCoode : Exception
    {
        private static string ConfigurationSettingsExceptionMessage = "Error en Codigo de Examen";

        public string Errors { get; private set; }
        public InvalidExamCoode(string error) : base(string.Format("{0} - {1}", ConfigurationSettingsExceptionMessage, error))
        {
            Errors = error;
        }

        public InvalidExamCoode(Exception inner) : base(ConfigurationSettingsExceptionMessage, inner)
        {

        }

        public InvalidExamCoode(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidExamCoode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
