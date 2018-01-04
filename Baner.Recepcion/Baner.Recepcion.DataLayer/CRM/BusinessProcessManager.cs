using Microsoft.Crm.Sdk.Messages;
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
            GetProcesInfo(processName, stageName);
            Entity e = new Entity(entityName)
            {
                Id = EntityId,
            };
            e.Attributes["processid"] = ProcesId;
            e.Attributes["stageid"] = SatageId;

            _xrmServerConnection.Update(e);
        }

        public void UpdateStage2(Guid Opo, string origen)
        {
            #region Variables
            int Origen = int.Parse(origen); ;
            int position = 0;
            string activeStageName = string.Empty;
            string workflow = string.Empty;
            string flujo = string.Empty;
            string workflowIncorrecto = string.Empty;
            #endregion

            #region Obtener datos de Origen
            switch (Origen)
            {
                case 3:
                case 4:
                    workflow = "opportunitysalesprocess";
                    flujo = "Opportunity Sales Process";
                    workflowIncorrecto = "leadtoopportunitysalesprocess";
                    break;
                default:
                    workflow = "leadtoopportunitysalesprocess";
                    flujo = "Proceso de cliente potencial a ventas de la oportunidad";
                    workflowIncorrecto = "opportunitysalesprocess";
                    break;
            }
            #endregion

            #region Recupera Workflow.
            QueryExpression opportunityBpfQuery = new QueryExpression
            {
                EntityName = "workflow",
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "uniquename",
                            Operator = ConditionOperator.Equal,
                            Values = { workflow }
                        }
                    }
                }
            };
            Workflow retrievedBPF = (Workflow)_xrmServerConnection.RetrieveMultiple(opportunityBpfQuery).Entities[0];
            var _bpfId = retrievedBPF.Id;
            #endregion

            #region Recuperar Etapa Activa.
            RetrieveProcessInstancesRequest procOpp2Req = new RetrieveProcessInstancesRequest
            {
                EntityId = Opo,
                EntityLogicalName = Opportunity.EntityLogicalName
            };

            RetrieveProcessInstancesResponse procOpp2Resp = (RetrieveProcessInstancesResponse)_xrmServerConnection.Execute(procOpp2Req);

            int processCount = procOpp2Resp.Processes.Entities.Count;
            int positionEntity = 0;
            int posicion = 0;
            foreach (var entity in procOpp2Resp.Processes.Entities)
            {
                var optionStatusCode = (OptionSetValue)entity.Attributes["statuscode"];
                var logicalName = entity.Attributes["name"].ToString();
                var valorStatusCode = optionStatusCode.Value;
                if (valorStatusCode == 1 && logicalName == flujo)                
                    posicion = positionEntity;                
                positionEntity++;

            }
            positionEntity = posicion;
            var activeProcessInstance = procOpp2Resp.Processes.Entities[positionEntity];
            var _processOpp2Id = activeProcessInstance.Id;
            var _activeStageId = new Guid(activeProcessInstance.Attributes["processstageid"].ToString());
            RetrieveActivePathRequest pathReq = new RetrieveActivePathRequest
            {
                ProcessInstanceId = _processOpp2Id
            };
            RetrieveActivePathResponse pathResp = (RetrieveActivePathResponse)_xrmServerConnection.Execute(pathReq);
            for (int i = 0; i < pathResp.ProcessStages.Entities.Count; i++)
            {
                if (pathResp.ProcessStages.Entities[i].Attributes["processstageid"].ToString() == _activeStageId.ToString())
                {
                    activeStageName = pathResp.ProcessStages.Entities[i].Attributes["stagename"].ToString();
                    position = i;
                    break;
                }
            }
            #endregion

            #region Avance de Fase
            _activeStageId = (Guid)pathResp.ProcessStages.Entities[position + 1].Attributes["processstageid"];
            ColumnSet cols1 = new ColumnSet();
            cols1.AddColumn("activestageid");
            Entity retrievedProcessInstance = _xrmServerConnection.Retrieve(workflow, _processOpp2Id, cols1);
            retrievedProcessInstance["activestageid"] = new EntityReference(ProcessStage.EntityLogicalName, _activeStageId);
            _xrmServerConnection.Update(retrievedProcessInstance);
            #endregion
        }

        public void correccionFasesProcesos(Guid Opo, string origen)
        {
            int Origen;
            int position = 0;
            string activeStageName;
            Origen = int.Parse(origen);
            string workflow = string.Empty;
            string flujo = string.Empty;
            string workflowIncorrecto = string.Empty;
            //Switch para identificar workflow acorde a Origen.
            switch (Origen)
            {
                case 3:
                case 4:
                    workflow = "opportunitysalesprocess";
                    flujo = "Opportunity Sales Process";
                    workflowIncorrecto = "leadtoopportunitysalesprocess";
                    break;
                default:                    
                    workflow = "leadtoopportunitysalesprocess";
                    flujo = "Proceso de cliente potencial a ventas de la oportunidad";
                    workflowIncorrecto = "opportunitysalesprocess";
                    break;
            }
            #region Recupera Workflow.
            QueryExpression opportunityBpfQuery = new QueryExpression
            {
                EntityName = "workflow",
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "uniquename",
                            Operator = ConditionOperator.Equal,
                            Values = { workflow }
                        }
                    }
                }
            };
            Workflow retrievedBPF = (Workflow)_xrmServerConnection.RetrieveMultiple(opportunityBpfQuery).Entities[0];
            var _bpfId = retrievedBPF.Id;
            #endregion

            #region Recuperar Etapas.
            RetrieveProcessInstancesRequest procOpp2Req = new RetrieveProcessInstancesRequest
            {
                EntityId = Opo,
                EntityLogicalName = Opportunity.EntityLogicalName
            };
            RetrieveProcessInstancesResponse procOpp2Resp = (RetrieveProcessInstancesResponse)_xrmServerConnection.Execute(procOpp2Req);
            int processCount = procOpp2Resp.Processes.Entities.Count;
            int positionEntity = 0;
            int posicion = 0;
            foreach(var entity in procOpp2Resp.Processes.Entities)
            {
                var optionStatusCode = (OptionSetValue)entity.Attributes["statuscode"];
                var logicalName = entity.Attributes["name"].ToString();
                var valorStatusCode = optionStatusCode.Value;
                if (valorStatusCode == 1 && logicalName == flujo)
                {
                    posicion = positionEntity;
                }
                else if (valorStatusCode == 1 && logicalName != flujo)//Está activo y NO corresponde al flujo. Hay que abortalo.
                {
                    Entity entityIncorrecto = new Entity(workflowIncorrecto);
                    entityIncorrecto.Id = entity.Id; ;
                    entityIncorrecto["statecode"] = new OptionSetValue(1);
                    entityIncorrecto["statuscode"] = new OptionSetValue(3);
                    _xrmServerConnection.Update(entityIncorrecto);
                    _xrmServerConnection.Delete(workflowIncorrecto, entity.Id);
                }
                positionEntity++;

            }
            //positionEntity = posicion;
            //var activeProcessInstance = procOpp2Resp.Processes.Entities[positionEntity]; // First record is the active process instance
            //var _processOpp2Id = activeProcessInstance.Id; // Id of the active process instance, which will be used later to retrieve the active path of the process instance
            //var _activeStageId = new Guid(activeProcessInstance.Attributes["processstageid"].ToString());
            //RetrieveActivePathRequest pathReq = new RetrieveActivePathRequest
            //{
            //    ProcessInstanceId = _processOpp2Id
            //};
            //RetrieveActivePathResponse pathResp = (RetrieveActivePathResponse)_xrmServerConnection.Execute(pathReq);
            //for (int i = 0; i < pathResp.ProcessStages.Entities.Count; i++)
            //{
            //    var text = pathResp.ProcessStages.Entities[i].Attributes["processstageid"].ToString();
            //    if (pathResp.ProcessStages.Entities[i].Attributes["processstageid"].ToString() == _activeStageId.ToString())
            //    {
            //        activeStageName = pathResp.ProcessStages.Entities[i].Attributes["stagename"].ToString();
            //        position = i;
            //        break;
            //    }
            //}
            //_activeStageId = (Guid)pathResp.ProcessStages.Entities[position + 1].Attributes["processstageid"];
            // Retrieve the process instance record to update its active stage
            //ColumnSet cols1 = new ColumnSet();
            //cols1.AddColumn("activestageid");
            // Entity retrievedProcessInstance = _xrmServerConnection.Retrieve(workflow, _processOpp2Id, cols1);
            // Set the next stage as the active stage
            //retrievedProcessInstance["activestageid"] = new EntityReference(ProcessStage.EntityLogicalName, _activeStageId);
            //_xrmServerConnection.Update(retrievedProcessInstance);

            #endregion
        }

        public string ObtenerFaseReal(Guid Opo, string origen)
        {
            int Origen;
            int position = 0;
            string activeStageName = string.Empty;
            Origen = int.Parse(origen);
            string workflow = string.Empty;
            string flujo = string.Empty;
            string workflowIncorrecto = string.Empty;
            //Switch para identificar workflow acorde a Origen.
            switch (Origen)
            {
                case 3:
                case 4:
                    workflow = "opportunitysalesprocess";
                    flujo = "Opportunity Sales Process";
                    workflowIncorrecto = "leadtoopportunitysalesprocess";
                    break;
                default:
                    workflow = "leadtoopportunitysalesprocess";
                    flujo = "Proceso de cliente potencial a ventas de la oportunidad";
                    workflowIncorrecto = "opportunitysalesprocess";
                    break;
            }
            #region Recupera Workflow.
            QueryExpression opportunityBpfQuery = new QueryExpression
            {
                EntityName = "workflow",
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "uniquename",
                            Operator = ConditionOperator.Equal,
                            Values = { workflow }
                        }
                    }
                }
            };
            Workflow retrievedBPF = (Workflow)_xrmServerConnection.RetrieveMultiple(opportunityBpfQuery).Entities[0];
            var _bpfId = retrievedBPF.Id;
            #endregion

            #region Recuperar Etapas.
            RetrieveProcessInstancesRequest procOpp2Req = new RetrieveProcessInstancesRequest
            {
                EntityId = Opo,
                EntityLogicalName = Opportunity.EntityLogicalName
            };

            RetrieveProcessInstancesResponse procOpp2Resp = (RetrieveProcessInstancesResponse)_xrmServerConnection.Execute(procOpp2Req);

            int processCount = procOpp2Resp.Processes.Entities.Count;
            int positionEntity = 0;
            int posicion = 0;
            foreach (var entity in procOpp2Resp.Processes.Entities)
            {
                var optionStatusCode = (OptionSetValue)entity.Attributes["statuscode"];
                var logicalName = entity.Attributes["name"].ToString();
                var valorStatusCode = optionStatusCode.Value;
                if (valorStatusCode == 1 && logicalName == flujo)
                {
                    posicion = positionEntity;
                }               
                positionEntity++;

            }
            positionEntity = posicion;
            var activeProcessInstance = procOpp2Resp.Processes.Entities[positionEntity]; // First record is the active process instance
            var _processOpp2Id = activeProcessInstance.Id; // Id of the active process instance, which will be used later to retrieve the active path of the process instance
            var _activeStageId = new Guid(activeProcessInstance.Attributes["processstageid"].ToString());
            RetrieveActivePathRequest pathReq = new RetrieveActivePathRequest
            {
                ProcessInstanceId = _processOpp2Id
            };
            RetrieveActivePathResponse pathResp = (RetrieveActivePathResponse)_xrmServerConnection.Execute(pathReq);
            for (int i = 0; i < pathResp.ProcessStages.Entities.Count; i++)
            {
                var text = pathResp.ProcessStages.Entities[i].Attributes["processstageid"].ToString();
                if (pathResp.ProcessStages.Entities[i].Attributes["processstageid"].ToString() == _activeStageId.ToString())
                {
                    activeStageName = pathResp.ProcessStages.Entities[i].Attributes["stagename"].ToString();
                    position = i;
                    break;
                }
            }
            #endregion
            if (string.IsNullOrWhiteSpace(activeStageName))
                activeStageName = "No se pudo determinar la fase actual.";
            return activeStageName;
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
