using AutoMapper;
using Baner.Recepcion.BusinessTypes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.Resolvers
{
    public class ApellidoPaternoResolver : ValueResolver<NewProspect, string>
    {
        protected override string ResolveCore(NewProspect source)
        {
            var appellidos = source.Apellidos.Split('*');

            if (appellidos != null && appellidos.Length > 0)
                return appellidos[0];

            throw new InvalidCastException("No se pudo extraer el apellido paterno");
        }
    }
}
