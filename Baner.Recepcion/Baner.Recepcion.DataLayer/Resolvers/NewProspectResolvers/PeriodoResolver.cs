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
    public class PeriodoResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public PeriodoResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {

            return null;
            //var item = _catalogrepository.ListaPeriodo();
            //if (item.ContainsKey(source.PeriodoId))
            //    return new EntityReference(rs_periodo.EntityLogicalName, new Guid(item[source.PeriodoId]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Periodo: {0}"
            //        , source.PeriodoId));
        }
    }
}
