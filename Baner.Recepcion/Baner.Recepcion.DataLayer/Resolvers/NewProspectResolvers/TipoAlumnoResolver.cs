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
    public class TipoAlumnoResolver : ValueResolver<NewProspect, OptionSetValue>
    {
        private IPickListRepository _picklistrepository;
        public TipoAlumnoResolver(IPickListRepository picklistrepository)
        {
            _picklistrepository = picklistrepository;
        }
        protected override OptionSetValue ResolveCore(NewProspect source)
        {
            var item = _picklistrepository.ListaTipoAlumno();
            if (item.ContainsValue(source.CodigoTipoAlumno))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.CodigoTipoAlumno).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist Tipo Alumno: {0}"
                    , source.CodigoTipoAlumno));

        }
    }
}
