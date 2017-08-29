using Baner.Recepcion.DataInterfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRM;

namespace Baner.Recepcion.DataLayer
{
    public class MonitorearConexionCrm : IMonitoreoConexion
    {
        public CRM365.Conector.Service service { get; set; }
        private IOrganizationService _xrmServerConnection;

        public bool NotificaConexionCRM()
        {
            bool goodConexion = false;
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);

            try
            {
                _xrmServerConnection = this.service.OrganizationService;
                goodConexion = true;
            }
            catch (Exception ex)
            {
                var es = ex.Message;
                goodConexion = false;
            }

            try
            {
                Account cuent = new Account();


                bool bExisteOpor = false;
                QueryExpression QueryOportEx = new QueryExpression(Account.EntityLogicalName)
                {

                    NoLock = false,
                    //ColumnSet = new ColumnSet(new string[] { "accountid" }),
                    ColumnSet = new ColumnSet { AllColumns = true },
                    TopCount = 5,
                    Criteria = {

                }
                };
                var Listcuent = _xrmServerConnection.RetrieveMultiple(QueryOportEx);
                //if (Listcuent != null)
                //// if (Listcuent.Entities.Any())
                //{
                //    goodConexion = true;

                //}
                goodConexion = true;
            }
            catch (Exception ex)
            {
                var es = ex.Message;
                goodConexion = false;
            }

            return goodConexion;
        }
    }
}
