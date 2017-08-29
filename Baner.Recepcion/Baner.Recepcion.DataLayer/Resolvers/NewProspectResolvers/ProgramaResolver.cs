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
    public class ProgramaResolver : ValueResolver<NewProspect, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public ProgramaResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(NewProspect source)
        {
            //var programas = _catalogrepository.ListaProgramas();
            //var item = programas.Find(p => p.CodigoCampus == source.Campus && p.CodigoPrograma == source.Programa1);
            //if (item != null)
            //    return new EntityReference(rs_programa.EntityLogicalName, item.IdPrograma);
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Programa: {0}"
            //        , source.Programa1));

            return null;
        }
    }
}
