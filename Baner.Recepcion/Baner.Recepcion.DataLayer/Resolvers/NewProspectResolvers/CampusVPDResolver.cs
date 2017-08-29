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
    public class CampusVPDResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public CampusVPDResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {
            return null;
            //var item = _catalogrepository.ListaCampus();
            //if (item.ContainsKey(source.CampusVPD))
            //    return new EntityReference(BusinessUnit.EntityLogicalName, new Guid(item[source.CampusVPD]));
            //else
            //    throw new LookupException("No se pudo resolver el Lookup de CampusVPD");
        }
    }
}
