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

namespace Baner.Recepcion.DataLayer.Resolvers.NewProspectResolvers
{
    class EstadoCivilResolver : ValueResolver<NewProspect, OptionSetValue>
    {
        private IPickListRepository _picklistrepository;
        public EstadoCivilResolver(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }
        protected override OptionSetValue ResolveCore(NewProspect source)
        {
            if (!string.IsNullOrWhiteSpace(source.EstadoCivil))
            {
                var item = _picklistrepository.ListaEstadoCivil();
                if (item.ContainsValue(source.EstadoCivil))

                    return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.EstadoCivil).Key);
                else
                    throw new PickListException(
                        string.Format("No se pudo resolver el picklist de Estado Civil: {0}"
                        , source.EstadoCivil));
            }
            else
                return null;

        }
    }
}
