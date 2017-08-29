using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using XRM;

namespace Baner.Recepcion.DataLayer.CRM
{
    public class BusinessProcessManager
    {
        private readonly IOrganizationService _xrmServerConnection;

        private Guid ProcesId = default(Guid);
        private Guid SatageId = default(Guid);
        //public Rhino.Crm365Connector.Service service { get; set; }
        public CRM365.Conector.Service service { get; set; }
        public BusinessProcessManager()
        {
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);

            _xrmServerConnection = this.service.OrganizationService;
        }

        private void GetProcesInfo(string processName, string stageName)
        {
            #region Recuperacion del proceso
            QueryExpression QueryProcess = new QueryExpression(Workflow.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "workflowid" }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression("name", ConditionOperator.Equal,processName),
                                //new ConditionExpression("statecode", ConditionOperator.Equal,WorkflowState.Activated)
                            },
                            //FilterOperator = LogicalOperator.And
                        }
                #endregion
            };
            var Process = _xrmServerConnection.RetrieveMultiple(QueryProcess);
            if (Process == null || Process.Entities == null || !Process.Entities.Any())
            {
                throw new CRMException(string.Format("El proceso seleccionado es invalido:{0}", processName));
            }
            else
                ProcesId = new Guid(Process.Entities.FirstOrDefault().Attributes["workflowid"].ToString());


            #endregion

            #region Recuperacion Stage
            QueryExpression QueryStage = new QueryExpression(ProcessStage.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "processstageid" }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression("stagename", ConditionOperator.Equal,stageName),
                                new ConditionExpression("processid", ConditionOperator.Equal,ProcesId)
                            },
                            FilterOperator = LogicalOperator.And
                        }
                #endregion
            };
            var Stage = _xrmServerConnection.RetrieveMultiple(QueryStage);
            if (Stage == null || Stage.Entities == null || !Stage.Entities.Any())
            {
                throw new CRMException(string.Format("El paso seleccionado es invalido:{0}", stageName));
            }
            else
                SatageId = new Guid(Stage.Entities.FirstOrDefault().Attributes["processstageid"].ToString());
            #endregion
        }

        public void UpdateStage(string processName, string stageName, string entityName, Guid EntityId)
        {
            //string stage1 = "Preoportunidad";
            //string proceso1 = "Proceso de cliente potencial a ventas de la oportunidad";

            //Se establecen los valores para el update del stage
            GetProcesInfo(processName, stageName);
            //GetProcesInfo(proceso1, stage1);

            //string stage2 = "Prospecto";
            //string proceso2 = "Proceso de cliente potencial a ventas de la oportunidad";

            ////Se establecen los valores para el update del stage
            //GetProcesInfo(proceso2, stage2);

            //string stage3 = "Solicitante";
            //string proceso3 = "Proceso de cliente potencial a ventas de la oportunidad";

            ////Se establecen los valores para el update del stage
            //GetProcesInfo(proceso3, stage3);

            //string stage5 = "Examinado";
            //string proceso5 = "Proceso de cliente potencial a ventas de la oportunidad";

            //GetProcesInfo(proceso5, stage5);

            //string stage7 = "Admitido";
            //string proceso7 = "Proceso de cliente potencial a ventas de la oportunidad";

            //GetProcesInfo(proceso7, stage7);

            //string stage6 = "Inscrito";
            //string proceso6 = "Proceso de cliente potencial a ventas de la oportunidad";

            //GetProcesInfo(proceso6, stage6);

            //string stage4 = "Nuevo Ingreso";
            //string proceso4 = "Proceso de cliente potencial a ventas de la oportunidad";

            //GetProcesInfo(proceso4, stage4);

            Entity e = new Entity(entityName)
            {
                Id = EntityId,
            };

            e.Attributes["processid"] = ProcesId;
            e.Attributes["stageid"] = SatageId;

            _xrmServerConnection.Update(e);
        }

        public void UpdateToSolicitante(string processName, string stageName, string entityName, Guid EntityId,
                        int? numSolicitud)
        {



            //Se establecen los valores para el update del stage
            GetProcesInfo(processName, stageName);

            Entity e = new Entity(entityName)
            {
                Id = EntityId,
            };

            if (numSolicitud != null && numSolicitud != 0)
            {
                e.Attributes["rs_numerosolicitud"] = numSolicitud;
                e.Attributes["rs_banderaenviooportunidad"] = new OptionSetValue(1);
            }



            e.Attributes["processid"] = ProcesId;
            e.Attributes["stageid"] = SatageId;

            _xrmServerConnection.Update(e);
        }

    }
}
