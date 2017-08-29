using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer.CRM;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using XRM;

namespace Baner.Recepcion.DataLayer
{
    public class LeadRepository : ILeadRepository
    {
        private readonly IOrganizationService _xrmServerConnection;
        // public Rhino.Crm365Connector.Service service { get; set; }
        public CRM365.Conector.Service service { get; set; }
        public LeadRepository()
        {
            //if (ConfigurationManager.ConnectionStrings.Count == 0)
            //    throw new ConfigurationSettingsException("No se han configurado los atributos de conexion correspondientes [CRM]");

            //if (ConfigurationManager.ConnectionStrings["CRM"] == null || string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString))
            //    throw new ConfigurationSettingsException("No se ha configurado [CRM]");
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            //this.service = new Rhino.Crm365Connector.Service(org, "", user, pass);
            this.service = new CRM365.Conector.Service(org, "", user, pass);

            _xrmServerConnection = this.service.OrganizationService;
        }


        public bool ActualizaLead(PreOportunidad preOportunidad)
        {
            bool resultado = false;

            //if (!RegistroValidado(preOportunidad.LeadId,preOportunidad.IdBanner))
            {

                Lead Prospecto = new Lead();
                Prospecto.Id = preOportunidad.LeadId;
                Prospecto.ua_ID_banner = preOportunidad.IdBanner;
                
                //Actualizamos el prospecto asignandole un idbanner
                _xrmServerConnection.Update(Prospecto);

                //Asi estaba version rhino
                ////Si el registro no ha sido previamente validado, se le establece el IdBanner
                //Lead l = new Lead();
                //l.Id = preOportunidad.LeadId;
                //l.rs_validaduplicado = new OptionSetValue(1);
                //l.rs_idbanner = preOportunidad.IdBanner;

                //_xrmServerConnection.Update(l);



            }
            //else
            //    throw new CRMException("El registro ya fue validado");

            return resultado;

        }

        private bool RegistroValidado(Guid LeadId, string pBanner)
        {
            bool resultado = true;
            //var ColumnSet = new ColumnSet(new string[] { "leadid", "rs_validaduplicado" });
            var ColumnSet = new ColumnSet(new string[] { "leadid", "ua_id_banner" });


            var res = _xrmServerConnection.Retrieve(Lead.EntityLogicalName, LeadId, ColumnSet);
            if (res != null)
            {
                var Prospecto = res.ToEntity<Lead>();
                resultado = true;
                if (Prospecto.ua_ID_banner != null)
                {
                    if (Prospecto.ua_ID_banner != pBanner)
                        return false;


                }

                //asi estaba anteriormente
                //if (Prospecto.rs_validaduplicado != null)
                //{
                //    if (Prospecto.rs_validaduplicado.Value == 0)
                //        return false;
                //    else
                //        return true;
                //}
                //return false;
            }
            return resultado;
        }

        private bool ActaulziarBanderaValidoCoincidencias(Guid LeadIdp)
        {
            bool resultado = true;


            Lead ProspeActualizar = new Lead();
            ProspeActualizar.LeadId = LeadIdp;
            ProspeActualizar.ua_valida_coincidencias = true;
            _xrmServerConnection.Update(ProspeActualizar);
            resultado = true;

            return resultado;

        }
    }
}
