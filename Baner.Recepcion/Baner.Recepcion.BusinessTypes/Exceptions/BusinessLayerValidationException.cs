using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes.Exceptions
{
    [Serializable]
    public class BusinessLayerValidationException : BusinessLayerException
    {
        private static string BusinessLayerValidationExceptionMessage = "Several validation errors occurred";

        public IEnumerable<string> Errors { get; private set; }

        public BusinessLayerValidationException(IEnumerable<string> errors)
            : base(BusinessLayerValidationExceptionMessage)
        {
            Errors = errors;
        }

        public BusinessLayerValidationException(IEnumerable<string> errors, Exception inner)
            : base(BusinessLayerValidationExceptionMessage, inner)
        {
            Errors = errors;
        }

        protected BusinessLayerValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
