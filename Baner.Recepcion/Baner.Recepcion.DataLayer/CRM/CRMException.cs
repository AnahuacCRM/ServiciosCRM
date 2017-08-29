using System;
using System.Runtime.Serialization;

namespace Baner.Recepcion.DataLayer.CRM
{
    [Serializable]
    internal class CRMException : Exception
    {
        public CRMException()
        {
        }

        public CRMException(string message) : base(message)
        {
        }

        public CRMException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CRMException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}