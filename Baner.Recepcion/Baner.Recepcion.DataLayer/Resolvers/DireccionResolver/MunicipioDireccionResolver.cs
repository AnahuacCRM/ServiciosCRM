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
    public class MunicipioDireccionResolver : ValueResolver<Direccion, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public MunicipioDireccionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Direccion source)
        {

            //if (source != null)
            //{
            //    if (source.PaisId == "99" || source.PaisId == "MEX")
            //    {
            //        var municipio = new Municipio()
            //        {
            //            CodigoMunicipio = source.DelegacionMunicipioId,
            //            Estado = source.Estado,
            //        };
            //        var item = _catalogrepository.ListaMunicipio(municipio);
            //        var founded = item.Find(mun =>
            //                    mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
            //               );
            //        if (founded != null)
            //            return new EntityReference(rs_colonia.EntityLogicalName, founded.IdCRM);
            //        else
            //        {

            //            string Estado = GetcodigoEstadoXsinonimo(source.Estado);
            //            var municipio2 = new Municipio()
            //            {
            //                CodigoMunicipio = source.DelegacionMunicipioId,
            //                Estado = Estado,
            //            };
            //            var itemSinonimo = _catalogrepository.ListaMunicipio(municipio2);
            //            var foundedSinonimo = itemSinonimo.Find(mun =>
            //                        mun.Estado == municipio2.Estado && mun.CodigoMunicipio == municipio2.CodigoMunicipio
            //                   );
            //            if (foundedSinonimo != null)
            //                return new EntityReference(rs_colonia.EntityLogicalName, foundedSinonimo.IdCRM);
            //            else
            //              return null;
            //        }
                      
            //            //throw new LookupException(
            //            //    string.Format("No se pudo resolver el Lookup de Delegacion/Municipio: {0}", source.DelegacionMunicipioId));

            //    }
            //    else //Extranjero
            //    {
            //        var municipio = new Municipio()
            //        {
            //            CodigoMunicipio = "20000",
            //            Estado = "FR",
            //        };
            //        var item = _catalogrepository.ListaMunicipio(municipio);
            //        var founded = item.Find(mun =>
            //                    mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
            //               );
            //        if (founded != null)
            //            return new EntityReference(rs_colonia.EntityLogicalName, founded.IdCRM);
            //        else
            //            throw new LookupException(
            //                string.Format("No se pudo resolver el Lookup de Delegacion/Municipio: {0}", source.DelegacionMunicipioId));
            //    }

            //}
            //else
                return null;
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
