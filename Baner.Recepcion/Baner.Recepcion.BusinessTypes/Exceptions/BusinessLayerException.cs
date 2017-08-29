using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes.Exceptions
{
    [Serializable]
    public class BusinessLayerException : Exception
    {
        public BusinessLayerException() { }
        public BusinessLayerException(string message) : base(message) { }
        public BusinessLayerException(string message, Exception inner) : base(message, inner) { }
        protected BusinessLayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
