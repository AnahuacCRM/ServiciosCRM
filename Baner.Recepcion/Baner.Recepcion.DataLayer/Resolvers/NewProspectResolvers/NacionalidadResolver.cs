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
using XRM;

namespace Baner.Recepcion.DataLayer.Resolvers
{
    public class NacionalidadResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public NacionalidadResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }

        protected override EntityReference ResolveCore(NewProspect source)
        {
            //if (!string.IsNullOrWhiteSpace(source.Nacionalidad))
            //{
            //    var item = _catalogrepository.ListaNacionalidad();
            //    if (item.ContainsKey(source.Nacionalidad))
            //        return new EntityReference(rs_nacionalidad.EntityLogicalName, new Guid(item[source.Nacionalidad]));
            //    else
            //        throw new LookupException(
            //            string.Format("No se pudo resolver el Lookup de Nacionalidad: {0}"
            //            , source.Nacionalidad));
            //}
            //else
                return null;
        }
    }
}
