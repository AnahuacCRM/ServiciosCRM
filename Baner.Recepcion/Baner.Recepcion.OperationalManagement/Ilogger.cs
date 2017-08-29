using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.OperationalManagement
{
    public interface ILogger
    {
        void Error(Exception ex);
        void Error(string error);
        void Infomacion(string mensaje);
    }
}
