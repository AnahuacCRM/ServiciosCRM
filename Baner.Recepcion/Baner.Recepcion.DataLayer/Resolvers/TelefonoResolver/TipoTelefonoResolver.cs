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

namespace Baner.Recepcion.DataLayer.Resolvers.TelefonoResolver
{
    public class TipoTelefonoResolver : ValueResolver<Telefono, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public TipoTelefonoResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Telefono source)
        {

            //var item = _catalogrepository.ListaTipoTelefono();
            //if (item.ContainsKey(source.TipoTelefono))
            //    return new EntityReference(rs_tipodireccion.EntityLogicalName, new Guid(item[source.TipoTelefono]));
            //else
            //    throw new LookupException("No se pudo resolver el Lookup de TipoDireccion");
            return null;
        }
    }
}
