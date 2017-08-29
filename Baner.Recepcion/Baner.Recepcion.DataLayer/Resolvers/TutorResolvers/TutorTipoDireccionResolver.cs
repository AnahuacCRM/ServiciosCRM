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

namespace Baner.Recepcion.DataLayer.Resolvers.TutorResolvers
{

    public class TutorTipoDireccionResolver : ValueResolver<PadreoTutor, EntityReference>
    {
        private ICatalogRepository _catalogrepository;
        public TutorTipoDireccionResolver(ICatalogRepository catalogrepository)
        {
            _catalogrepository = catalogrepository;
        }
        protected override EntityReference ResolveCore(PadreoTutor source)
        {
            //if (!string.IsNullOrWhiteSpace(source.TipoDireccion))
            //{
            //    var item = _catalogrepository.ListaTipoDireccion();
            //    if (item.ContainsKey(source.TipoDireccion))
            //        return new EntityReference(rs_tipodireccion.EntityLogicalName, new Guid(item[source.TipoDireccion]));
            //    else
            //        throw new LookupException("No se pudo resolver el Lookup de TipoDireccion");
            //}
            //else
                return null;
        }
    }
}
