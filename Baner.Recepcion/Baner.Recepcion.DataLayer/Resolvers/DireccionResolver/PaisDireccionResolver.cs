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
    public class PaisDireccionResolver : ValueResolver<Direccion, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public PaisDireccionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Direccion source)
        {
        //    if (!string.IsNullOrWhiteSpace(source.PaisId))
        //    {
        //        var item = _catalogrepository.ListaPais();
        //        if (item.ContainsKey(source.PaisId))
        //            return new EntityReference(rs_pais.EntityLogicalName, new Guid(item[source.PaisId]));
        //        else

        //            throw new LookupException(
        //                string.Format("No se pudo resolver el Lookup de Pais de la direccion: {0}"
        //                , source.PaisId));
        //    }
        //    else
                return null;
        }
    }
}
