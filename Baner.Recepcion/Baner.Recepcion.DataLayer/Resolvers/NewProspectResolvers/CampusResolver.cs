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

    public class CampusResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public CampusResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {

            //if (!string.IsNullOrWhiteSpace(source.Campus))
            //{
            //    var item = _catalogrepository.ListaCampus();
            //    if (item.ContainsKey(source.Campus))
            //        return new EntityReference(BusinessUnit.EntityLogicalName, new Guid(item[source.Campus]));
            //    else
            //        throw new LookupException(
            //            string.Format("No se pudo resolver el Lookup de Campus: {0}", source.Campus));
            //}
            //else
                return null;
        }
    }
}
