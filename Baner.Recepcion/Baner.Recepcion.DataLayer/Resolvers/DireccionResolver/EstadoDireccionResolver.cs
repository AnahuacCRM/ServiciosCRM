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
    public class EstadoDireccionResolver : ValueResolver<Direccion, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public EstadoDireccionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Direccion source)
        {
            return null;
            //var item = _catalogrepository.ListaEstado();
            //if (source.PaisId == "99" || source.PaisId == "MEX")
            //{

            //    if (item.ContainsKey(source.Estado))
            //        return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[source.Estado]));
            //    else
            //    {
            //      string Estado = GetcodigoEstadoXsinonimo(source.Estado);
            //        if (item.ContainsKey(Estado))
            //            return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[Estado]));
            //        else
            //            return null;
            //        //throw new LookupException(
            //        //    string.Format("No se pudo resolver el Lookup de Estado de la direccion {0}", source.Estado));
            //    }


            //}
            //else
            //{
            //    if (item.ContainsKey("FR"))
            //        return new EntityReference(rs_estado.EntityLogicalName, new Guid(item["FR"]));
            //    else
            //        throw new LookupException(
            //            string.Format("No se pudo resolver el Lookup de Estado de la direccion {0}", "FR"));
            //}

        }

        private string GetcodigoEstadoXsinonimo(string value)
        {
            return "";
            //var item = _catalogrepository.ListaCodigoEstado();
            //if (item.ContainsKey(value))
            //    return item[value];
            //else
            //{
            //    if (item != null)
            //    {
            //        foreach (var i in item)
            //        {
            //            string[] sinonimos = i.Key.Split(',');
            //            var results = Array.FindAll(sinonimos, s => s.Equals(value));
            //            if (results != null && results.Length >= 1)
            //                if (results[0] == value)
            //                    return i.Value;
            //        }
            //    }
            //    return value;
            //}
        }
    }
}
