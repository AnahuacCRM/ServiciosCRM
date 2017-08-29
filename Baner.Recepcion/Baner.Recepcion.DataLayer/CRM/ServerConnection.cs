using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataLayer.CRM
{
    public class ServerConnection : IServerConnection
    {
        #region propiedades
        public CrmServiceClient CnxCrm { get; set; }
        public IOrganizationService Service { get; set; }

        #endregion



        public ServerConnection()
        {
            if (ConfigurationManager.ConnectionStrings.Count == 0)
                throw new ConfigurationSettingsException("No se han configurado la cadena de conexion para CRM");

            if (string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString))
                throw new ConfigurationSettingsException("No se han configurado la cadena de conexion para CRM");



            var crmcs = ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;

            CnxCrm = new CrmServiceClient(crmcs);
            Service = CnxCrm.OrganizationWebProxyClient != null ? CnxCrm.OrganizationWebProxyClient : (IOrganizationService)CnxCrm.OrganizationServiceProxy;



            if (!CnxCrm.IsReady)
            {

                throw new ConfigurationSettingsException(CnxCrm.LastCrmError);
            }


        }



    }
}
