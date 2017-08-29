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

namespace Baner.Recepcion.DataLayer.Resolvers.TutorResolvers
{
    public class ViveResolver : ValueResolver<PadreoTutor, bool?>
    {
        //private IPickListRepository _picklistrepository;
        //public ViveResolver(IPickListRepository picklistrepository)
        //{
        //    _picklistrepository = picklistrepository;
        //}
        protected override bool? ResolveCore(PadreoTutor source)
        {
            if (!string.IsNullOrWhiteSpace(source.Vive))
            {
                if (source.Vive.ToUpper() == "Y")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
}
