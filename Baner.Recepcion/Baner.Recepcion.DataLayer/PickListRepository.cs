using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer.Cache;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
//using Rhino.Crm365Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRM;


namespace Baner.Recepcion.DataLayer
{
    public class PickListRepository : IPickListRepository
    {
        //private readonly ILogger _logger;
        private readonly IOrganizationService _xrmServerConnection;
        private readonly MemoryCacher LocalCache;
        private readonly CRMMetadata crmmetadata;

        public CRM365.Conector.Service service { get; set; }

        public PickListRepository()
        {
            //if (ConfigurationManager.ConnectionStrings.Count == 0)
            //    throw new ConfigurationSettingsException("No se han configurado los atributos de conexion correspondientes [CRM]");

            //if (ConfigurationManager.ConnectionStrings["CRM"] == null || string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString))
            //    throw new ConfigurationSettingsException("No se ha configurado [CRM]");

            //_logger = logger;
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);

            _xrmServerConnection = this.service.OrganizationService;
            crmmetadata = new CRMMetadata();

            LocalCache = new MemoryCacher();
        }

        public Dictionary<int, string> ListaCorreoPreferido()
        {
            return crmmetadata.RetrieveOptionsetMetadata("rs_correoelectronico.EntityLogicalName", "rs_indpreferido");
        }

        public Dictionary<int, string> ListaEstadoCivil()
        {

            //return crmmetadata.RetrieveOptionsetMetadata(rs_prospecto.EntityLogicalName, "rs_codigoestadocivil");
            return this.GetConfiguration("EstadoCivil", "rs_prospecto.EntityLogicalName", "rs_estadocivil");
        }

        public Dictionary<int, string> ListaEstatusSolicitud()
        {

            //return crmmetadata.RetrieveOptionsetMetadata(rs_prospecto.EntityLogicalName, "rs_estatussolicitud");
            return this.GetConfiguration("EstatusSolicitud", "rs_prospecto.EntityLogicalName", "statuscode");
        }

        public Dictionary<int, string> ListaParentesco()
        {
            //return crmmetadata.RetrieveOptionsetMetadata(Lead.EntityLogicalName, "rs_parentesco");
            return this.GetConfiguration("Parentesco", Lead.EntityLogicalName, "rs_parentesco");
        }

        public Dictionary<int, string> ListaSexo()
        {
            return crmmetadata.RetrieveOptionsetMetadata("rs_prospecto.EntityLogicalName", "rs_sexo");
        }

        public Dictionary<int, string> ListaTelefonoPreferido()
        {
            return crmmetadata.RetrieveOptionsetMetadata("rs_telefono.EntityLogicalName", "rs_preferidotelefono");
        }

        public Dictionary<int, string> ListaTipoAdmision()
        {
            //return crmmetadata.RetrieveOptionsetMetadata(rs_prospecto.EntityLogicalName, "rs_codigotipoadmision");
            return this.GetConfiguration("TipoAdmision", "rs_prospecto.EntityLogicalName", "rs_tipoadmision");
        }

        public Dictionary<int, string> ListaTipoAlumno()
        {
            //return crmmetadata.RetrieveOptionsetMetadata(rs_prospecto.EntityLogicalName, "rs_codigotipoalumno");
            return this.GetConfiguration("TipoAlumno", "rs_prospecto.EntityLogicalName", "rs_tipoalumno");
        }

        public Dictionary<int, string> ListaEstatusAlumno()
        {
            return this.GetConfiguration("EstatusAlumno", Opportunity.EntityLogicalName, "rs_estatusalumno");
        }

        public Dictionary<int, string> ListaVive()
        {
            return crmmetadata.RetrieveOptionsetMetadata(Lead.EntityLogicalName, "rs_vive");
        }

        public Dictionary<int, string> ListTipoAdmision(string Entidad, string Atributo)
        {
            var cachename = string.Format("{0}{1}", Entidad, Atributo);
            return GetConfiguration(cachename, Entidad, Atributo);
        }


        private Dictionary<int, string> GetConfiguration(string cachename, string EntityLogicalName, string NombreEsquema)
        {
            var resultado = default(Dictionary<int, string>);
            EntityCollection ec = default(EntityCollection);

            var localtoken = cachename;
            //Recuperar de cache los codigos de accion
            var cache = LocalCache.GetValue(localtoken);
            if (cache == null)
            {//Si no estan en cache recuperarlo de la BD

                QueryExpression Query = new QueryExpression("rs_configuracion.EntityLogicalName")
                {
                    #region Consulta
                    NoLock = true,
                    ColumnSet = new ColumnSet(new string[] { "rs_codigo", "rs_descripcion", "rs_id" }),
                    Criteria = {
                            Conditions = {
                                new ConditionExpression("rs_esquemadescripcion", ConditionOperator.Equal,NombreEsquema),
                                new ConditionExpression("rs_name", ConditionOperator.Equal,EntityLogicalName)
                            },
                            FilterOperator = LogicalOperator.And
                        }
                    #endregion
                };
                ec = _xrmServerConnection.RetrieveMultiple(Query);

                if (ec != null)
                {
                    resultado = new Dictionary<int, string>();
                    foreach (var item in ec.Entities)
                    {
                        var Key = int.Parse(item["rs_id"].ToString());
                        var Value = item["rs_codigo"].ToString();
                        resultado.Add(Key, Value);
                    }

                    LocalCache.Add(localtoken, resultado, DateTimeOffset.UtcNow.AddHours(12));
                }
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                resultado = (Dictionary<int, string>)cache;
            }

            return resultado;
        }

        public Dictionary<int, string> ListaTipoDesicionAdmision()
        {
            return this.GetConfiguration("TipoDesicionAdmision", Opportunity.EntityLogicalName, "rs_tipodecision");
        }

        public Dictionary<int, string> ListOrigen(string Entidad, string Atributo)
        {
            return this.GetConfiguration("Origen", Entidad, Atributo);
        }

        public Dictionary<int, string> ListaTipoBeca(string Entidad, string Atributo)
        {
            return crmmetadata.RetrieveOptionsetMetadata(Entidad, Atributo);
        }

        public Dictionary<int, string> ListaTipoColegio()
        {

            //return crmmetadata.RetrieveOptionsetMetadata(Account.EntityLogicalName, "rs_tipocolegio");
            return this.GetConfiguration("TipoColegioP", Account.EntityLogicalName, "rs_tipocolegio");
        }

        public Dictionary<int, string> ListaTipoContacto()
        {
            return this.GetConfiguration("TipoContacto", Account.EntityLogicalName, "rs_tipocontacto");
        }
    }
}
