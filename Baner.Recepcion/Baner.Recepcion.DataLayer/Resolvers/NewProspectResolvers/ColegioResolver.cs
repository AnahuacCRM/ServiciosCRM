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
    public class ColegioResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public ColegioResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }

        protected override EntityReference ResolveCore(NewProspect source)
        {
        //    if (!(string.IsNullOrWhiteSpace(source.ColegioProcedencia))){
        //        var item = _catalogrepository.ListaColegio();
        //        if (item.ContainsKey(source.ColegioProcedencia))
        //            return new EntityReference(Account.EntityLogicalName, new Guid(item[source.ColegioProcedencia]));
        //        else
        //            throw new LookupException(
        //                string.Format("No se pudo resolver el Lookup de Colegio: {0}"
        //                , source.ColegioProcedencia));
        //    }
        //    else
                return null;
        }
    }
}
