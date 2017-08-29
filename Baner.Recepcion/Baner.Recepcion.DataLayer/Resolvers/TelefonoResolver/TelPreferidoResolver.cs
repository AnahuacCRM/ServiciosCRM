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

namespace Baner.Recepcion.DataLayer.Resolvers.TelefonoResolver
{
    class TelPreferidoResolver : ValueResolver<Telefono, bool>
    {
        
        protected override bool ResolveCore(Telefono source)
        {

            if (!string.IsNullOrWhiteSpace(source.PreferidoTelefono))
                if (source.PreferidoTelefono.Equals("Y"))
                    return true;
                else if (source.PreferidoTelefono.Equals("N"))
                    return false;
                else
                    throw new PickListException(string.Format("El valor proporcionado para telefono preferido no es valido {0}", source.PreferidoTelefono));

            else
                return false;

        }
    }
}
