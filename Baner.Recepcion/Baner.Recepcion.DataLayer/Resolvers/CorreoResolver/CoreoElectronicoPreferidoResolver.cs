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

namespace Baner.Recepcion.DataLayer.Resolvers.CorreoResolver
{
   public  class CoreoElectronicoPreferidoResolver : ValueResolver<Correo, bool>
    {
        
       
        protected override bool ResolveCore(Correo source)
        {

            if (!string.IsNullOrWhiteSpace(source.IndPreferido))
                if (source.IndPreferido.Equals("Y"))
                    return true;
                else if (source.IndPreferido.Equals("N"))
                    return false;
                else
                    throw new PickListException(string.Format("El valor proporcionado para correo preferido no es valido {0}", source.IndPreferido));
            else
                return false;


            //var item = _picklistrepository.ListaCorreoPreferido();
            //if (item.ContainsValue(source.IndPreferido))
            //    return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.IndPreferido).Key);
            //else
            //    throw new PickListException("No se pudo resolver el picklist de Correo electronico Preferido");

        }
    }
}
