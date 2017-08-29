using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes.Exceptions
{
    [Serializable]
    public class BannerException : Exception
    {

        public BannerException() { }
        public BannerException(string message) : base(message) { }
        public BannerException(string message, Exception inner) : base(message, inner) { }
        protected BannerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
