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
    public class SexoResolver : ValueResolver<NewProspect, OptionSetValue>
    {
        private IPickListRepository _picklistrepository;
        public SexoResolver(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }
        protected override OptionSetValue ResolveCore(NewProspect source)
        {
            if (!string.IsNullOrWhiteSpace(source.Sexo))
            {
                var item = _picklistrepository.ListaSexo();
                if (item.ContainsValue(source.Sexo))
                    return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.Sexo).Key);
                else
                    throw new PickListException("No se pudo resolver el picklist de Sexo");
            }
            else
                return null;

        }
    }
}
