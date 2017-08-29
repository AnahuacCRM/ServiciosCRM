using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.CRM
{
    public interface IServerConnection
    {

         CrmServiceClient CnxCrm { get; set; }
         IOrganizationService Service { get; set; }
    }
}
