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
    public class ReligionResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public ReligionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {
            //if (!string.IsNullOrWhiteSpace(source.ReligionId))
            //{
            //    var item = _catalogrepository.ListaReligion();
            //    if (item.ContainsKey(source.ReligionId))
            //        return new EntityReference(rs_religion.EntityLogicalName, new Guid(item[source.ReligionId]));
            //    else
            //        throw new LookupException(
            //            string.Format("No se pudo resolver el Lookup de Religion: {0}"
            //            , source.ReligionId));
            //}
            //else
                return null;
        }
    }
}
