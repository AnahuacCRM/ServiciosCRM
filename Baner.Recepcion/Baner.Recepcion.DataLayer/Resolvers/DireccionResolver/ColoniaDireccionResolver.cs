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
    public class ColoniaDireccionResolver : ValueResolver<Direccion, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public ColoniaDireccionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(Direccion source)
        {
            //if (!string.IsNullOrEmpty(source.Colonia))
            //{
            //    var colonia = new Colonia()
            //    {
            //        CP = source.CodigoPostal,
            //        DelegacionMunicipio = source.DelegacionMunicipioId,
            //        Estado = source.Estado,
            //        Pais = source.PaisId,
            //        Nombre = source.Colonia
            //    };

            //    var item = _catalogrepository.ListaColonias(colonia);

            //    var founded = item.Find(col =>
            //          col.CP == colonia.CP && col.DelegacionMunicipio == colonia.DelegacionMunicipio
            //          && col.Estado == colonia.Estado && col.Pais == colonia.Pais
            //          && col.Nombre == colonia.Nombre
            //      );
            //    if (founded != null)
            //        return new EntityReference(rs_colonia.EntityLogicalName, founded.IdCRM);
            //    else
            //    {

            //        colonia.Estado = GetcodigoEstadoXsinonimo(colonia.Estado);
            //        var itemSinonimo = _catalogrepository.ListaColonias(colonia);

            //        var foundedSinonimo = itemSinonimo.Find(col =>
            //              col.CP == colonia.CP && col.DelegacionMunicipio == colonia.DelegacionMunicipio
            //              && col.Estado == colonia.Estado && col.Pais == colonia.Pais
            //              && col.Nombre == colonia.Nombre
            //          );
            //        if (foundedSinonimo != null)
            //            return new EntityReference(rs_colonia.EntityLogicalName, foundedSinonimo.IdCRM);
            //        else
            //        return null;
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
