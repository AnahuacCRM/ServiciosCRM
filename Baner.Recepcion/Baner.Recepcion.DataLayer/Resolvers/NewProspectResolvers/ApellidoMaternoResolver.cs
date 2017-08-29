using AutoMapper;
using Baner.Recepcion.BusinessTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.Resolvers
{
    class ApellidoMaternoResolver : ValueResolver<NewProspect, string>
    {
        protected override string ResolveCore(NewProspect source)
        {
            var appellidos = source.Apellidos.Split('*');

            if (appellidos != null && appellidos.Length == 2)
                return appellidos[1];

            return string.Empty;
           // throw new InvalidCastException("No se pudo extraer el apellido materno");
        }
    }
}
