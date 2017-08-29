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

namespace Baner.Recepcion.DataLayer.Resolvers.TutorResolvers
{
    public class ParentescoResolver : ValueResolver<PadreoTutor, OptionSetValue>
    {
        private IPickListRepository _picklistrepository;
        public ParentescoResolver(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }
        protected override OptionSetValue ResolveCore(PadreoTutor source)
        {
            if (!string.IsNullOrWhiteSpace(source.Parentesco))
            {
                var item = _picklistrepository.ListaParentesco();
                if (item.ContainsValue(source.Parentesco))
                    return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.Parentesco).Key);
                else
                    throw new PickListException("No se pudo resolver el picklist de Parentesco");
            }
            else
                return null;

        }
    }
}
