using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement.Extensions
{
    public static class ExceptionExtensions
    {
        public static string Build(this Exception target)
        {
            var message = new StringBuilder();
            while (target != null)
            {
                message.AppendLine(target.Message);
                target = target.InnerException;
            }
            return message.ToString();
        }
    }
}
