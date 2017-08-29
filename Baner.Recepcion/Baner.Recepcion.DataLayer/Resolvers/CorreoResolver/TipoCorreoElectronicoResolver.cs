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
    public class TipoCorreoElectronicoResolver : ValueResolver<Correo, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public TipoCorreoElectronicoResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Correo source)
        {

            //if (!string.IsNullOrWhiteSpace(source.TipoCorreoElectronicoId))
            //{
            //    var item = _catalogrepository.ListaTipoCorreo();
            //    if (item.ContainsKey(source.TipoCorreoElectronicoId))
            //        return new EntityReference(rs_tipodireccion.EntityLogicalName, new Guid(item[source.TipoCorreoElectronicoId]));
            //    else
            //        throw new LookupException("No se pudo resolver el Lookup de Tipo correo electronico");
            //}
            //else
                return null;
        }
    }
}
