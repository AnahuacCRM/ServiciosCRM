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
    public class TipoAdmisionResolver : ValueResolver<NewProspect, OptionSetValue>
    {
        private IPickListRepository _picklistrepository;
        public TipoAdmisionResolver(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }
        protected override OptionSetValue ResolveCore(NewProspect source)
        {
            var item = _picklistrepository.ListaTipoAdmision();
            if (item.ContainsValue(source.CodigoTipoadmision))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.CodigoTipoadmision).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist de Tipo Admision: {0}", source.CodigoTipoadmision));

        }
    }
}
