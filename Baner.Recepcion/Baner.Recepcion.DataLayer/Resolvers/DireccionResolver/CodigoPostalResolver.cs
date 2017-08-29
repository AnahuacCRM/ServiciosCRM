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

namespace Baner.Recepcion.DataLayer.Resolvers.DireccionResolver
{
    public class CodigoPostalResolver : ValueResolver<Direccion, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public CodigoPostalResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Direccion source)
        {
            //if (!string.IsNullOrWhiteSpace(source.CodigoPostal))
            //{
            //    var item = _catalogrepository.ListaCodigoPostal(source.CodigoPostal);
            //    if (item.ContainsKey(source.CodigoPostal))
            //        return new EntityReference(rs_codigopostal.EntityLogicalName, new Guid(item[source.CodigoPostal]));
            //    else
            //        return null;
                    
            //}
            //else
                return null;
        }
    }
}
