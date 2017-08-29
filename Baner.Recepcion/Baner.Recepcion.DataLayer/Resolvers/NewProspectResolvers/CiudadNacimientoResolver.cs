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

namespace Baner.Recepcion.DataLayer.Resolvers.NewProspectResolvers
{
    public class CiudadNacimientoResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public CiudadNacimientoResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {
            //recuperar la colonia para obtener la ciudad de nacimiento
            //var item = _catalogrepository.RetrieveCiudades(source.CiudadNacimiento);
            //if (item.ContainsKey(source.EstadoNacimiento + source.CiudadNacimiento))
            //    return new EntityReference(rs_colonia.EntityLogicalName, new Guid(item[source.EstadoNacimiento + source.CiudadNacimiento]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Ciudad de Nacimiento: {0}"
            //        , source.CiudadNacimiento));
            return null;
        }
    }
}
