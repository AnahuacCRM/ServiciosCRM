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
    public class EstadoNacimientoResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public EstadoNacimientoResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {
            //if (!string.IsNullOrWhiteSpace(source.EstadoNacimiento))
            //{
            //    var item = _catalogrepository.ListaEstado();
            //    if (item.ContainsKey(source.EstadoNacimiento))
            //        return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[source.EstadoNacimiento]));
            //    else
            //        throw new LookupException(
            //            string.Format("No se pudo resolver el Lookup de Estado Nacimiento: {0}"
            //            , source.EstadoNacimiento));
            //}
            //else
                return null;
        }
    }
}
