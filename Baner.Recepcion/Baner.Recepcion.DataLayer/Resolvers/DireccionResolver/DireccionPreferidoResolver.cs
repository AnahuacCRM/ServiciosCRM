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

namespace Baner.Recepcion.DataLayer.Resolvers.DireccionResolver
{
    //public class DireccionPreferidoResolver : ValueResolver<Direccion, OptionSetValue>
    //{
    //    private IPickListRepository _picklistrepository;
    //    public DireccionPreferidoResolver(IPickListRepository picklistrepository)
    //    {
    //        _picklistrepository = picklistrepository;
    //    }
    //    protected override OptionSetValue ResolveCore(Direccion source)
    //    {
    //        var item = _picklistrepository.ListaDIreccionPreferido();
    //        if (item.ContainsValue(source.Preferido))
    //            return new OptionSetValue(item.FirstOrDefault(i => i.Value == source.Preferido).Key);
    //        else
    //            throw new PickListException("No se pudo resolver el picklist de direccion Preferido");

    //    }
    //}

}
