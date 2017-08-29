using AutoMapper;
using Baner.Recepcion.BusinessTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.Resolvers
{
    public class FechaNacimientoResolver : ValueResolver<NewProspect, DateTime?>
    {
        protected override DateTime? ResolveCore(NewProspect source)
        {
            if (source.FechaNacimiento != null)
                return new DateTime(source.FechaNacimiento.Year, source.FechaNacimiento.Month, source.FechaNacimiento.Day);
            else
                return null;
        }
    }
}
