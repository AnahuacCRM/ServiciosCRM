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
    public class EscuelaResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public EscuelaResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }

        protected override EntityReference ResolveCore(NewProspect source)
        {
            return null;
            //var item = _catalogrepository.ListaEscuela();
            //if (item.ContainsKey(source.Escuela))
            //    return new EntityReference(Account.EntityLogicalName, new Guid(item[source.Escuela]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Escuela: {0}", source.Escuela));
        }
    }
}
