using Baner.Recepcion.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.Transformators
{

    public class TransformStatusSolicitud
    {
        IPickListRepository _picklistrepository;
        public TransformStatusSolicitud(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }

        public int Transform(string val)
        {
            int result = 0;
            var item = _picklistrepository.ListaEstatusSolicitud();
            if (item.ContainsValue(val))
            {
                result = item.FirstOrDefault(i => i.Value == val).Key;
            }


            return result;
        }
    }
}
