using AutoMapper;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.Resolvers
{
    public class EstatusSolicitudResolver : ValueResolver<NewProspect, OptionSetValue>
    {
        private IPickListRepository _picklistrepository;
        public EstatusSolicitudResolver(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }
        protected override OptionSetValue ResolveCore(NewProspect source)
        {
            var item = _picklistrepository.ListaEstatusSolicitud();
            if (item.ContainsValue(source.EstatusSolicitud))

                return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.EstatusSolicitud).Key);
            else
                throw new PickListException("No se pudo resolver el picklist de estatus solicitud");

        }
    }
}
