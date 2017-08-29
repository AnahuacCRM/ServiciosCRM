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
    public class TipoDireccionResolver : ValueResolver<Direccion, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public TipoDireccionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Direccion source)
        {
            return null;
            //var item = _catalogrepository.ListaTipoDireccion();
            //if (item.ContainsKey(source.TipoDireccionId))
            //    return new EntityReference(rs_tipodireccion.EntityLogicalName, new Guid(item[source.TipoDireccionId]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de TipoDireccion: {0}", source.TipoDireccionId));
        }
    }
}
