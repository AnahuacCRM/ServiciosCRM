﻿using Baner.Recepcion.BusinessTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessInterfaces
{
    public interface ILeadProcessor
    {
        bool ActualizaLead(PreOportunidad preOportunidad);
    }
}
