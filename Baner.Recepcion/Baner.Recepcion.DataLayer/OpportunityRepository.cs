using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.DataLayer.Transformators;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using XRM;

namespace Baner.Recepcion.DataLayer
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly IOrganizationService _xrmServerConnection;
        public CRM365.Conector.Service service { get; set; }
        public OpportunityRepository()
        {
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);

            _xrmServerConnection = this.service.OrganizationService;
        }

        public int RetrieveStatusById(Guid OpportunityId)
        {
            var op = (Opportunity)_xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, OpportunityId, new ColumnSet(new string[] { "statuscode", "statecode" }));
            return ((OptionSetValue)op.StatusCode).Value;

        }

        public Coincidencias ObtenerPreOportunidad(Guid LeadId)
        {

            //var resultado = default(Coincidencias);
            //var ColumnSet = new ColumnSet(new string[] { "leadid","firstname","lastname","middlename"
            //    , "rs_segundonombre", "rs_fechanacimiento", "emailaddress1","rs_sexo",
            //      "telephone1","rs_vpdid"});

            var ColumnSet1 = new ColumnSet(new string[] { "leadid","firstname","lastname","middlename",
                 "ua_fecha_nacimiento", "emailaddress1","ua_sexo",
                  "telephone1","ua_codigo_vpd"});




            var ColumnSet = new ColumnSet { AllColumns = true };


            //LeadId = GetIdProspecto("Patricia");


            var res = _xrmServerConnection.Retrieve(Lead.EntityLogicalName, LeadId, ColumnSet);


            if (res != null)
            {
                //var preoportunidad = res.ToEntity<Lead>();

                var prospepecto = res.ToEntity<Lead>();

                Coincidencias paramCoincidencias = new Coincidencias();

                if (prospepecto != null)
                {
                    if (prospepecto.ua_sexo != null)
                        paramCoincidencias.Sexo = prospepecto.ua_sexo.ToString();//obtener el sexxo en letra
                    if (prospepecto.FirstName != null)
                        paramCoincidencias.Nombre = prospepecto.FirstName;
                    if (prospepecto.MiddleName != null)
                        paramCoincidencias.Segundo_Nombre = prospepecto.MiddleName;
                    if (prospepecto.LastName != null)
                    {
                        paramCoincidencias.Apellido_Paterno = SplitApellidoPaterno(prospepecto.LastName);
                        paramCoincidencias.Apellido_Materno = SplitApellidoMaterno(prospepecto.LastName);

                    }



                    if (prospepecto.ua_codigo_vpd != null)
                        paramCoincidencias.VPD = prospepecto.ua_codigo_vpd.Trim();

                    if (prospepecto.ua_sexo != null)
                    {
                        string sexo = ((OptionSetValue)prospepecto.ua_sexo).Value == 2 ? "M" : "F";
                        paramCoincidencias.Sexo = sexo;
                    }
                    if (prospepecto.ua_fecha_nacimiento != null)
                    {
                        var fecha = new CustomDate()
                        {
                            Year = prospepecto.ua_fecha_nacimiento.Value.Year,
                            Month = prospepecto.ua_fecha_nacimiento.Value.Month,
                            Day = prospepecto.ua_fecha_nacimiento.Value.Day
                        };
                        paramCoincidencias.Fecha_Nacimiento = fecha;
                    }

                    if (prospepecto.EMailAddress1 != null)
                        paramCoincidencias.Correo_Electrónico = prospepecto.EMailAddress1.ToString();

                    if (prospepecto.Telephone1 != null)
                        paramCoincidencias.Numero_Telefonico = prospepecto.Telephone1.ToString();

                }






                //var fechaNac = default(CustomDate);
                //if (preoportunidad.ua_fecha_nacimiento != null)
                //{
                //    fechaNac = new CustomDate()
                //    {
                //        Year = preoportunidad.ua_fecha_nacimiento == null ? 0 : (int)preoportunidad.ua_fecha_nacimiento.Value.Year,
                //        Month = preoportunidad.ua_fecha_nacimiento == null ? 0 : (int)preoportunidad.ua_fecha_nacimiento.Value.Month,
                //        Day = preoportunidad.ua_fecha_nacimiento == null ? 0 : (int)preoportunidad.ua_fecha_nacimiento.Value.Day
                //    };
                //}





                return paramCoincidencias;
            }
            return default(Coincidencias);

        }

        private string SplitApellidoPaterno(string apellido)
        {
            var appellidos = apellido.Split(' ');

            if (appellidos != null && appellidos.Length > 0)
                return appellidos[0];
            else
                return "";
            //
        }

        /// <summary>
        /// Metodo para Separar los apellidos y regresar el apellido materno
        /// </summary>
        /// <param name="apellido"></param>
        /// <returns></returns>
        private string SplitApellidoMaterno(string apellido)
        {
            var appellidos = apellido.Split(' ');

            if (appellidos != null && appellidos.Length == 2)
                return appellidos[1];

            return string.Empty;
        }

        //Abre todas las oportunidades en base a la cuenta
        public List<Guid> RetrieveOportunidades(Guid CuentaId, Guid opportunityid)
        {

            Opportunity op = new Opportunity();

            // op.CreatedOn
            EntityReference idc = new EntityReference(Account.EntityLogicalName, CuentaId);
            List<Guid> Resultado = new List<Guid>();

            QueryExpression QueryStage = new QueryExpression(Opportunity.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "opportunityid" }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression("parentaccountid", ConditionOperator.Equal,idc.Id),
                                new ConditionExpression("statecode",ConditionOperator.Equal,0),
                                new ConditionExpression("opportunityid", ConditionOperator.NotEqual,opportunityid)
                            },
                            FilterOperator= LogicalOperator.And
                        }
                #endregion
            };
            var res = _xrmServerConnection.RetrieveMultiple(QueryStage);

            if (res.Entities != null && res.Entities.Any())
            {
                foreach (var o in res.Entities)
                {
                    Resultado.Add(new Guid(o["opportunityid"].ToString()));
                }
            }

            return Resultado;
        }

        public void DeactivateOportunity(string entityName, Guid OportunidadId)
        {

            LoseOpportunityRequest req = new LoseOpportunityRequest();
            Entity opportunityClose = new Entity("opportunityclose");
            opportunityClose.Attributes.Add("opportunityid", new EntityReference(entityName, OportunidadId));
            opportunityClose.Attributes.Add("subject", "Lost the Opportunity!");
            req.OpportunityClose = opportunityClose;
            OptionSetValue o = new OptionSetValue();
            o.Value = 4;
            req.Status = o;
            //LoseOpportunityResponse resp = 
            _xrmServerConnection.Execute(req);


        }

        public void CerrarOportunidadComoGanada(string entityName, Guid OportunidadId)
        {

            //LoseOpportunityRequest req = new LoseOpportunityRequest();
            //Entity opportunityClose = new Entity("opportunityclose");
            //opportunityClose.Attributes.Add("opportunityid", new EntityReference(entityName, OportunidadId));
            //opportunityClose.Attributes.Add("subject", "Win the Opportunity!");
            //req.OpportunityClose = opportunityClose;
            //OptionSetValue o = new OptionSetValue();
            //o.Value = (int)OpportunityState.Won;
            //req.Status = o;
            ////LoseOpportunityResponse resp = 
            //_xrmServerConnection.Execute(req);

            WinOpportunityRequest req = new WinOpportunityRequest();
            Entity opportunityClose = new Entity("opportunityclose");
            opportunityClose.Attributes.Add("opportunityid", new EntityReference(entityName, OportunidadId));
            opportunityClose.Attributes.Add("subject", "Won the Opportunity!");
            req.OpportunityClose = opportunityClose;
            OptionSetValue osvWon = new OptionSetValue();
            osvWon.Value = 3;
            req.Status = osvWon;
            WinOpportunityResponse resp = (WinOpportunityResponse)_xrmServerConnection.Execute(req);



        }

        public List<Guid> RetrieveProspectOportunidades(Guid pCuenta)
        {
            List<Guid> Resultado = new List<Guid>();

            QueryExpression QueryStage = new QueryExpression(Opportunity.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "opportunityid" }),
                Criteria = {
                            Conditions = {
                                //new ConditionExpression("rs_solicitante", ConditionOperator.Equal,ProspectoId),
                                 new ConditionExpression("accountid", ConditionOperator.Equal,pCuenta),
                                new ConditionExpression("statecode",ConditionOperator.Equal,0)
                            },
                            FilterOperator= LogicalOperator.And
                        }
                #endregion
            };
            var res = _xrmServerConnection.RetrieveMultiple(QueryStage);

            if (res.Entities != null && res.Entities.Any())
            {
                foreach (var o in res.Entities)
                {
                    Resultado.Add(new Guid(o["opportunityid"].ToString()));
                }
            }

            return Resultado;
        }

        public List<Guid> RetrieveProspectOportunidades(string BannerId)
        {
            List<Guid> Resultado = new List<Guid>();

            QueryExpression QueryStage = new QueryExpression(Opportunity.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "opportunityid" }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression("ua_idbanner", ConditionOperator.Equal,BannerId),
                                new ConditionExpression("statecode",ConditionOperator.Equal,0)
                            },
                            FilterOperator= LogicalOperator.And
                        }
                #endregion
            };
            var res = _xrmServerConnection.RetrieveMultiple(QueryStage);

            if (res.Entities != null && res.Entities.Any())
            {
                foreach (var o in res.Entities)
                {
                    Resultado.Add(new Guid(o["opportunityid"].ToString()));
                }
            }

            return Resultado;
        }

        public List<Guid> RetrieveOpportunityME(string idBanner)
        {
            //throw new NotImplementedException();
            List<Guid> lstRes = new List<Guid>();
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "opportunityid" }),
                Criteria = {
                    Conditions = {

                         new ConditionExpression("rs_idbanner", ConditionOperator.Equal, idBanner),
                         new ConditionExpression("statecode", ConditionOperator.Equal, 0),
                    }
                },
                LinkEntities =
                {
                    new LinkEntity() {
                        LinkFromEntityName = Opportunity.EntityLogicalName,
                        LinkToEntityName= Account.EntityLogicalName,
                        LinkFromAttributeName="rs_escuelaid",
                        LinkToAttributeName= "accountid",
                        JoinOperator= JoinOperator.Inner,
                        Columns= new ColumnSet("rs_codigoescuela"),
                        EntityAlias="Escuela",
                        LinkCriteria = {
                            Conditions = {//Filtro escuelas Medicina
                                new ConditionExpression("rs_codigoescuela", ConditionOperator.Equal, "ME"),
                            }
                        },
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                foreach (var item in ec.Entities)
                {
                    //var prospecto = ec.Entities.FirstOrDefault();
                    resultado = new Guid(item.Attributes["opportunityid"].ToString());
                    lstRes.Add(resultado);
                }
            }
            else
            {
                throw new Exception("No se encontró ninguna oportunidad que coincida con la regla de escuelas ME");
            }
            return lstRes;

        }

        public void ReopenOpportunity(string entityName, Guid OpportunityId)
        {
            /* Reopen the Opportunity record */
            EntityReference oppRef = new EntityReference()
            {
                LogicalName = Opportunity.EntityLogicalName,
                Id = OpportunityId
            };
            SetStateRequest openOpp = new SetStateRequest();
            openOpp.EntityMoniker = oppRef;
            openOpp.State = new OptionSetValue(0); // Open
            openOpp.Status = new OptionSetValue(1); // In Progress

            _xrmServerConnection.Execute(openOpp);
        }


        public bool IsClosed(Guid OpportunityId)
        {
            var op = (Opportunity)_xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, OpportunityId, new ColumnSet(new string[] { "statuscode", "statecode" }));
            return ((OptionSetValue)op.StatusCode).Value == 4;
        }

        public bool IsOpen(Guid OpportunityId)
        {
            var op = (Opportunity)_xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, OpportunityId, new ColumnSet(new string[] { "statuscode", "statecode" }));
            return ((OptionSetValue)op.StatusCode).Value == 0;
        }


        public bool MarcarTransferido(List<Guid> OportunidadesId)
        {
            bool resultado = false;
            var tasksedit = new List<System.Threading.Tasks.Task>();
            foreach (var item in OportunidadesId)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    Opportunity t = new Opportunity();
                    t.OpportunityId = item;
                    //t.rs_estatustransferencia = new OptionSetValue(1);
                    _xrmServerConnection.Update(t);
                });
                tasksedit.Add(task);
            }

            System.Threading.Tasks.Task.WaitAll(tasksedit.ToArray());
            resultado = true;

            return resultado;
        }


        //private bool ExisteOportunidad(string pVPDI, string pPrograma, string pCampus, Guid idOpor
        public bool ExisteOportunidad(string pidbanner, out Guid pbannerExiste)
        {
            Opportunity op = new Opportunity();
            pbannerExiste = default(Guid);
            //18ba18ee-de04-e711-810f-e0071b6a8131

            bool bExisteOpor = false;
            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "name", "Opportunityid" }),
                // ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal,  pidbanner)
                    //    new ConditionExpression("rs_codigoprograma", ConditionOperator.Equal,  pPrograma),
                    //     new ConditionExpression("rs_codigovpdi", ConditionOperator.NotEqual, pVPDI)
                    }
                }
            };
            var ListOport = _xrmServerConnection.RetrieveMultiple(QueryOportEx);

            if (ListOport != null)
                if (ListOport.Entities.Count > 0)
                {
                    foreach (var item in ListOport.Entities)
                    {
                        pbannerExiste = new Guid(item["Opportunityid"].ToString());
                    }

                    bExisteOpor = true;

                }



            return bExisteOpor;
        }

        public Guid CrearOportunidad(Cuenta p, Guid pCuenta, EntityReferenceTransformer _entityReferenceTransformer, bool crearCampos, string solegioprodecenciaP)
        {


            Guid idPopor = default(Guid);

            Opportunity opCreate = new Opportunity();

            opCreate.ua_suborigen = "Oportunidad";
            opCreate.ua_origen = new OptionSetValue(3);

            //Asociarlo con contacto

            if (!string.IsNullOrWhiteSpace(p.Promedio))
                opCreate.ua_promedio = p.Promedio;

            if (!string.IsNullOrWhiteSpace(p.Colegio_Procedencia))
            {
                // opCreate.ua_colegio_procedencia = new EntityReference(ua_colegios.EntityLogicalName, new Guid(p.Colegio_Procedencia));
                opCreate.ua_colegio_asesor = new EntityReference(ua_colegios_asesor.EntityLogicalName, new Guid(p.Colegio_Procedencia));
                if (!string.IsNullOrWhiteSpace(solegioprodecenciaP))
                {
                    opCreate.ua_colegio_procedencia = _entityReferenceTransformer.GetColegioProcedencia(solegioprodecenciaP);
                    if (opCreate.ua_colegio_procedencia != null)
                        opCreate.ua_colegioGUIDStr = opCreate.ua_colegio_procedencia.Id.ToString();
                }
            }
            else
                opCreate.ua_colegio_asesor = null;

            if (!string.IsNullOrWhiteSpace(p.Nacionalidad))
                opCreate.ua_desc_nacionalidad = _entityReferenceTransformer.GetNacionalidad(p.Nacionalidad);


            if (p.IdBanner != null)
                opCreate.ua_idbanner = p.IdBanner;
            if (p.Nombre != null)
                opCreate.Name = p.Nombre;
            if (p.Numero_Solicitud != null)
                opCreate.ua_numero_solicitud = p.Numero_Solicitud;

            var vpd = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", p.CampusVPD);

            if (p.CampusVPD != null)
            {
                opCreate.ua_codigo_vpd = p.CampusVPD;
                if (vpd != null)
                    opCreate.ua_vpd = vpd;
            }
            if (p.PeriodoId != null)
                opCreate.ua_periodo = GetPeriodo(p.PeriodoId);
            if (p.Nivel != null)
                opCreate.ua_desc_nivel = GetIdReferencia(ua_niveles.EntityLogicalName, "ua_nivelesid", "ua_codigo_nivel", p.Nivel);

            if (p.Escuela != null)
                opCreate.ua_desc_escuela = GetIdReferencia(ua_escuela.EntityLogicalName, "ua_escuelaid", "ua_codigo_escuela", p.Escuela);
            if (p.Codigo_Tipo_Alumno != null)
                opCreate.ua_desc_tipo_alumno = GetIdReferencia(ua_tipoalumno.EntityLogicalName, "ua_tipoalumnoid", "ua_codigo_tipo_alumno", p.Codigo_Tipo_Alumno);

            if (p.Codigo_Tipo_admision != null)
                opCreate.ua_desc_tipo_admision = GetIdReferencia(ua_tipo_admision.EntityLogicalName, "ua_tipo_admisionid", "ua_codigo_tipo_admision", p.Codigo_Tipo_admision);



            if (p.Campus != null)
                opCreate.ua_codigo_campus = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", p.Campus);
            if (opCreate.ua_codigo_campus == null)
                throw new Exception("No se pudo resolver el Lookup de Campus:  " + p.Campus);

            if (p.Programa1 != null && opCreate.ua_codigo_campus != null && vpd != null)
            {
                opCreate.ua_programav2 = GetProgramaId(p.Programa1, opCreate.ua_codigo_campus);
                opCreate.ua_programa_asesor = GetProgramaAsesor(ua_programas_por_campus_asesor.EntityLogicalName, opCreate.ua_programav2, vpd, "ua_programas_por_campus_asesorid",
                    "ua_programas_por_campus", "ua_codigo_vpd", "ua_programas_por_campus_asesor");

                opCreate.ua_Programa = _entityReferenceTransformer.GetPrograma(p.Programa1);
                if (opCreate.ua_Programa != null)
                    opCreate.ua_colegioGUIDStr = opCreate.ua_Programa.Id.ToString();
            }



            if (p.Estatus_Solicitud != null)
                opCreate.ua_estatus_solicitud = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Opportunity.EntityLogicalName, "ua_estatus_solicitud", p.Estatus_Solicitud));

            Direccion itemDir = p.Direcciones.Where(x => x.TipoDireccionId == "PR").FirstOrDefault();
            var VPDp = vpd;
            if (crearCampos)//Solamente cuando se crea la cuenta en las actualizaciones hay un flijo que jala lso datos de la cuenta
            {
                //Direccion itemDir = p.Direcciones.Where(x => x.TipoDireccionId == "PR").FirstOrDefault();

                if (itemDir != null)
                {

                    #region Direccion principal



                    if (!string.IsNullOrWhiteSpace(itemDir.PaisId))
                    {
                        var entitiPais = _entityReferenceTransformer.GetPais(itemDir.PaisId);
                        if (entitiPais != null)
                            opCreate.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitiPais, vpd,
                             "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", itemDir.PaisId, p.CampusVPD);



                    }
                    //if (opCreate.ua_pais_asesor == null)
                    //    respuestaIntegracion.Warnings.Add(" el Pais " + itemDir.PaisId + " no fue encontrado en el catalogo para asignar asesor");

                    //c.ua_Colonia_Extranjera1 = itemDir.Colonia;

                    if (!string.IsNullOrWhiteSpace(itemDir.Estado))
                    {
                        var entitiestado = _entityReferenceTransformer.GetEstadoSE(itemDir.Estado);
                        if (entitiestado != null)
                            opCreate.ua_estado_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entitiestado, VPDp,
                                                      "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", itemDir.Estado, p.CampusVPD);

                    }
                    //if (opCreate.ua_estado_asesor == null) respuestaIntegracion.Warnings.Add(" el Estado " + itemDir.Estado + " no fue encontrado en el catalogo para asignar asesor");


                    if (!string.IsNullOrWhiteSpace(itemDir.DelegacionMunicipioId))
                    {
                        var entityMunicipio = _entityReferenceTransformer.GetMunicipioId(itemDir.DelegacionMunicipioId);
                        if (entityMunicipio != null)
                            opCreate.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityMunicipio, VPDp,
                                                         "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", itemDir.DelegacionMunicipioId, p.CampusVPD);
                    }
                    //if (opCreate.ua_delegacion_municipio_asesor == null) respuestaIntegracion.Warnings.Add(" el Municipio " + itemDir.DelegacionMunicipioId + " no fue encontrado en el catalogo para asignar asesor");


                    #endregion

                }
            }
            else // si es igual a false bienen los id entityreference
            {
                if (itemDir != null)
                {

                    if (!string.IsNullOrWhiteSpace(itemDir.PaisId))
                    {
                        opCreate.ua_pais_asesor = new EntityReference(ua_pais_asesor.EntityLogicalName, new Guid(itemDir.PaisId));
                        //if (entitiPais != null)
                        //    opCreate.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitiPais, vpd,
                        //     "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", itemDir.PaisId, p.CampusVPD);
                    }

                    if (!string.IsNullOrWhiteSpace(itemDir.Estado))
                    {
                        opCreate.ua_estado_asesor = new EntityReference(ua_estados_asesor.EntityLogicalName, new Guid(itemDir.Estado));
                        //if (entitiestado != null)
                        //    opCreate.ua_estado_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entitiestado, VPDp,
                        //"ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", itemDir.Estado, p.CampusVPD);

                    }

                    if (!string.IsNullOrWhiteSpace(itemDir.DelegacionMunicipioId))
                    {
                        opCreate.ua_delegacion_municipio_asesor = new EntityReference(ua_delegacion_municipio_asesor.EntityLogicalName, new Guid(itemDir.DelegacionMunicipioId));
                        //if (entityMunicipio != null)
                        //    opCreate.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityMunicipio, VPDp,
                        //                                 "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", itemDir.DelegacionMunicipioId, p.CampusVPD);
                    }
                }
            }




            //Asocirlo con cuenta
            opCreate.ParentAccountId = new EntityReference(Account.EntityLogicalName, pCuenta);

            idPopor = _xrmServerConnection.Create(opCreate);


            //var colegioase = _xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, new Guid(idPopor.ToString()), new ColumnSet { AllColumns = true });
            //if (colegioase != null)
            //{
            //    //Lead l = new Lead();

            //    string col = colegioase.Attributes["ua_colegio_asesor"].ToString();
            //}

            return idPopor;
        }

        public Guid UpdateOportunidad(Cuenta p, Guid idOportunidad, EntityReferenceTransformer _entityReferenceTransformer, string solegioprodecenciaP)
        {
            Guid idPopor = default(Guid);
            Opportunity opUpdate = new Opportunity();


            //Asociarlo con contacto
            // opCreate.CustomerId = new EntityReference(Contact.EntityLogicalName, pContactoID);
            if (!string.IsNullOrWhiteSpace(p.Promedio))
                opUpdate.ua_promedio = p.Promedio;
            if (!string.IsNullOrWhiteSpace(p.Colegio_Procedencia))
            {
                //opUpdate.ua_colegio_procedencia = _entityReferenceTransformer.GetColegioProcedencia(p.Colegio_Procedencia);
                opUpdate.ua_colegio_asesor = new EntityReference(ua_colegios_asesor.EntityLogicalName, new Guid(p.Colegio_Procedencia));

                if (string.IsNullOrWhiteSpace(solegioprodecenciaP))
                {
                    opUpdate.ua_colegio_procedencia = _entityReferenceTransformer.GetColegioProcedencia(solegioprodecenciaP);
                    if (opUpdate.ua_colegio_procedencia != null)
                        opUpdate.ua_colegioGUIDStr = opUpdate.ua_colegio_procedencia.Id.ToString();
                }
            }
            else
                opUpdate.ua_colegio_asesor = null;

            if (!string.IsNullOrEmpty(p.Nacionalidad))
                opUpdate.ua_desc_nacionalidad = _entityReferenceTransformer.GetNacionalidad(p.Nacionalidad);

            if (p.IdBanner != null)
                opUpdate.ua_idbanner = p.IdBanner;
            if (p.Apellidos != null)
                opUpdate.Name = p.Nombre + " " + p.Apellidos;
            if (p.Numero_Solicitud != null)
                opUpdate.ua_numero_solicitud = p.Numero_Solicitud;
            if (p.CampusVPD != null)
                opUpdate.ua_codigo_vpd = p.CampusVPD;
            var vpd = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", p.CampusVPD);
            if (vpd != null)
                opUpdate.ua_vpd = vpd;

            if (p.PeriodoId != null)
                opUpdate.ua_periodo = GetPeriodo(p.PeriodoId);
            if (p.Nivel != null)
                opUpdate.ua_desc_nivel = GetIdReferencia(ua_niveles.EntityLogicalName, "ua_nivelesid", "ua_codigo_nivel", p.Nivel);

            if (p.Escuela != null)
                opUpdate.ua_desc_escuela = GetIdReferencia(ua_escuela.EntityLogicalName, "ua_escuelaid", "ua_codigo_escuela", p.Escuela);
            if (p.Codigo_Tipo_Alumno != null)
                opUpdate.ua_desc_tipo_alumno = GetIdReferencia(ua_tipoalumno.EntityLogicalName, "ua_tipoalumnoid", "ua_codigo_tipo_alumno", p.Codigo_Tipo_Alumno);
            // opCreate.ua_desc_tipo_decision = GetIdReferencia(ua_tipo_decision.EntityLogicalName, "ua_tipo_decisionid", "ua_codigo_tipo_decision", p.Tipo_decision);
            if (p.Codigo_Tipo_admision != null)
                opUpdate.ua_desc_tipo_admision = GetIdReferencia(ua_tipo_admision.EntityLogicalName, "ua_tipo_admisionid", "ua_codigo_tipo_admision", p.Codigo_Tipo_admision);



            //if (p.Campus != null)
            //    opUpdate.ua_codigo_vpd_pl = 
            if (p.Campus != null)
                opUpdate.ua_codigo_campus = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", p.Campus);



            if (p.Programa1 != null && opUpdate.ua_codigo_campus != null)
            {
                opUpdate.ua_programav2 = GetProgramaId(p.Programa1, opUpdate.ua_codigo_campus);
                opUpdate.ua_programa_asesor = GetProgramaAsesor(ua_programas_por_campus_asesor.EntityLogicalName, opUpdate.ua_programav2, opUpdate.ua_codigo_campus, "ua_programas_por_campus_asesorid",
                    "ua_programas_por_campus", "ua_codigo_vpd", "ua_programas_por_campus_asesor");

                opUpdate.ua_Programa = _entityReferenceTransformer.GetPrograma(p.Programa1);
                if (opUpdate.ua_Programa != null)
                    opUpdate.ua_colegioGUIDStr = opUpdate.ua_Programa.Id.ToString();
            }

            //GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", p.Campus);
            if (p.Estatus_Solicitud != null)
                opUpdate.ua_estatus_solicitud = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Opportunity.EntityLogicalName, "ua_estatus_solicitud", p.Estatus_Solicitud));


            opUpdate.OpportunityId = idOportunidad;
            try
            {
                _xrmServerConnection.Update(opUpdate);
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                throw new Exception("error al actualziar oportunidad");
            }
            return idOportunidad;
        }



        #region Utilerias

        private string ConjuntoObciones(string EntityLogicaname, string CampodeConjunto, string pValor)
        {
            string sValor = "";
            Dictionary<string, string> diccon = new Dictionary<string, string>();
            RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = EntityLogicaname,
                LogicalName = CampodeConjunto,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse attributeResponse =
                   (RetrieveAttributeResponse)_xrmServerConnection.Execute(attributeRequest);

            if (attributeResponse.Results.Count > 0)
            {
                string sNameOpt = "";
                int valorOpt = -1;
                int conOp = 0;

                foreach (PicklistAttributeMetadata ColecionValues in attributeResponse.Results.Values)
                {
                    foreach (var item in ColecionValues.OptionSet.Options)
                    {
                        sNameOpt = item.Label.LocalizedLabels[0].Label;
                        valorOpt = int.Parse(item.Value.ToString());
                        diccon.Add(sNameOpt, valorOpt.ToString());
                        conOp++;
                    }

                }

                if (diccon.ContainsKey(pValor))
                {
                    sValor = diccon[pValor];
                }



            }
            return sValor;
        }

        private EntityReference GetIdReferencia(string EntyLogicaname, string Codigoid, string CampoComparar, string valor)
        {
            //return ListaCatalogo("NivelesEd", ua_niveles.EntityLogicalName, "ua_codigo_nivel", "ua_nivelesid");
            ua_periodo p = new ua_periodo();
            EntityReference resultado = default(EntityReference);
            Guid IdMun = default(Guid);

            QueryExpression Query = new QueryExpression(EntyLogicaname)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { Codigoid }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression(CampoComparar, ConditionOperator.Equal,valor)

                    }

                }
            };
            var periodo = _xrmServerConnection.RetrieveMultiple(Query);

            if (periodo.Entities.Any())
            {
                var per = periodo.Entities.FirstOrDefault();

                IdMun = new Guid(per.Attributes[Codigoid].ToString());
                resultado = new EntityReference(ua_carreras_web.EntityLogicalName, IdMun);
            }

            return resultado;
        }

        public EntityReference GetPeriodo(string value)
        {
            ua_periodo p = new ua_periodo();

            EntityReference resultado = default(EntityReference);
            Guid IdMun = default(Guid);

            QueryExpression Query = new QueryExpression(ua_periodo.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_periodoid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_periodo", ConditionOperator.Equal,value)

                    }

                }
            };
            var periodo = _xrmServerConnection.RetrieveMultiple(Query);

            if (periodo.Entities.Any())
            {
                var per = periodo.Entities.FirstOrDefault();

                IdMun = new Guid(per.Attributes["ua_periodoid"].ToString());
                resultado = new EntityReference(ua_carreras_web.EntityLogicalName, IdMun);
            }

            return resultado;
        }

        private EntityReference GetProgramaId(string pcodigoPrograma, EntityReference Campus)
        {
            Guid idProgrma = default(Guid);
            Guid idProgramaPorCampus = default(Guid);
            EntityReference Progr = null;
            QueryExpression QueryCarr = new QueryExpression(ua_programaV2.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_programav2id" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_codigo_del_programa", ConditionOperator.Equal, pcodigoPrograma),

                        }
                    }
            };
            var listCarre = _xrmServerConnection.RetrieveMultiple(QueryCarr);

            if (listCarre.Entities.Any())
            {
                var periodoent = listCarre.Entities.FirstOrDefault();
                idProgrma = new Guid(periodoent.Attributes["ua_programav2id"].ToString());

                //Obtenemos el campus 


                //Obtenemos el programa
                QueryExpression Query = new QueryExpression(ua_ppc_programav2.EntityLogicalName)
                {
                    NoLock = true,
                    ColumnSet = new ColumnSet(new string[] { "ua_ppc_programav2id" }),
                    Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_ppc_campus", ConditionOperator.Equal, Campus.Id),
                            new ConditionExpression("ua_programa", ConditionOperator.Equal, idProgrma)

                        }
                    }
                };
                var ec = _xrmServerConnection.RetrieveMultiple(Query);
                if (ec.Entities.Any())
                {
                    var programa = ec.Entities.FirstOrDefault();
                    idProgramaPorCampus = new Guid(programa.Attributes["ua_ppc_programav2id"].ToString());
                    //result = (EntityReference)programa.Attributes["ua_ppc_programaid"];
                    //result = new EntityReference(ua_ppc_programa.EntityLogicalName, idp);
                    Progr = new EntityReference(ua_ppc_programav2.EntityLogicalName, idProgramaPorCampus);
                }
                if (idProgramaPorCampus == default(Guid))
                    throw new LookupException("El id campus  " + Campus.Id + " y el id programa " + pcodigoPrograma + " no existe en el catalgo");
            }
            //else
            //    throw new LookupException("el  Programa " + pcodigoPrograma + " no fue encontro en el catalogo");
            return Progr;
        }


        public EntityReference GetProgramaAsesor(string EntityLogicalName, EntityReference idPorog, EntityReference VpdP, string campoid, string F1, string F2, string col2)
        {
            string pasiasesorname = "";
            Guid idp = default(Guid);
            if (idPorog == null)
                throw new LookupException("El programa es requerida para recuperar el el pais del asesor");
            if (VpdP == null)
                throw new LookupException("La VPD es requerido para recuperar el programa");
            EntityReference result = new EntityReference();

            // ua_pais_asesor pa = new ua_pais_asesor();


            QueryExpression Query = new QueryExpression(EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { campoid, col2 }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression(F1, ConditionOperator.Equal, idPorog.Id),
                            new ConditionExpression(F2, ConditionOperator.Equal, VpdP.Id)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var programa = ec.Entities.FirstOrDefault();
                idp = new Guid(programa.Attributes[campoid].ToString());
                if (programa.Attributes.Contains(col2))
                    pasiasesorname = programa.Attributes[col2].ToString();

                //result = (EntityReference)programa.Attributes["ua_ppc_programaid"];
                //result = new EntityReference(ua_ppc_programa.EntityLogicalName, idp);

            }
            if (idp == default(Guid))
                throw new LookupException("el programa  " + idPorog + " y el id Vpd " + VpdP + " no existe en el catalgo");
            return new EntityReference(ua_pais_asesor.EntityLogicalName, idp);

        }

        public EntityReference GetDatoAsesor(string EntityLogicalName, EntityReference PaisP, EntityReference VpdP, string campoid, string F1, string F2, string col2, string descF1, string descF2)
        {
            string pasiasesorname = "";
            Guid idp = default(Guid);
            if (PaisP == null)
                throw new LookupException("El pais es requerida para recuperar el el pais del asesor");
            if (VpdP == null)
                throw new LookupException("La VPD es requerido para recuperar el programa");
            EntityReference result = new EntityReference();

            // ua_pais_asesor pa = new ua_pais_asesor();


            QueryExpression Query = new QueryExpression(EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { campoid, col2 }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression(F1, ConditionOperator.Equal, PaisP.Id),
                            new ConditionExpression(F2, ConditionOperator.Equal, VpdP.Id)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var programa = ec.Entities.FirstOrDefault();
                idp = new Guid(programa.Attributes[campoid].ToString());
                if (programa.Attributes.Contains(col2))
                    pasiasesorname = programa.Attributes[col2].ToString();

                //result = (EntityReference)programa.Attributes["ua_ppc_programaid"];
                //result = new EntityReference(ua_ppc_programa.EntityLogicalName, idp);

            }
            string msjReturn = "";

            if (idp == default(Guid))
            {
                switch (EntityLogicalName)
                {
                    case "ua_pais_asesor":
                        msjReturn = "El codigo de Pais  " + descF1 + " con la Vpd " + descF2 + " no existe en el catalgo";
                        break;
                    case "ua_estados_asesor":
                        msjReturn = "El codigo de estado  " + descF1 + " con la Vpd " + descF2 + " no existe en el catalgo";
                        break;
                    case "ua_delegacion_municipio_asesor":
                        msjReturn = "El codigo de Municipio  " + descF1 + " con la  Vpd " + descF2 + " no existe en el catalgo";
                        break;
                    case "ua_programas_por_campus_asesor":
                        msjReturn = "El codigo de programa  " + descF1 + " con la vpd" + descF2 + " no existe en el catalgo";
                        break;
                    case "ua_colegios_asesor":
                        msjReturn = "El codigo colegio   " + descF1 + " con la vpd" + descF2 + " no existe en el catalgo";
                        break;

                    default:
                        break;
                }

                throw new LookupException(msjReturn);
            }

            return new EntityReference(ua_pais_asesor.EntityLogicalName, idp);

        }




        private Guid GetIdProspecto(string campoComparar)
        {
            Guid IdMun = default(Guid);

            QueryExpression Query = new QueryExpression(Lead.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "leadid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("firstname", ConditionOperator.Equal,campoComparar)

                    }

                }
            };
            var periodo = _xrmServerConnection.RetrieveMultiple(Query);

            if (periodo.Entities.Any())
            {
                var per = periodo.Entities.FirstOrDefault();

                IdMun = new Guid(per.Attributes["leadid"].ToString());

            }
            return IdMun;
        }

        private Dictionary<int, string> RetrieveOptionsetMetadata(string entidad, string optionset)
        {
            //Dictionary to store value and text
            Dictionary<int, string> _DropdownDatasource = new Dictionary<int, string>();
            //Create request to fetch optionset
            Microsoft.Xrm.Sdk.Messages.RetrieveAttributeRequest _Request = new Microsoft.Xrm.Sdk.Messages.RetrieveAttributeRequest
            {
                EntityLogicalName = entidad,
                LogicalName = optionset,
                RetrieveAsIfPublished = true
            };

            // Execute the request

            //IOrganizationService svc = new XrmProxyService().GetCrmServiceProxy();
            Microsoft.Xrm.Sdk.Messages.RetrieveAttributeResponse _Response =
                (Microsoft.Xrm.Sdk.Messages.RetrieveAttributeResponse)_xrmServerConnection.Execute(_Request);
            Microsoft.Xrm.Sdk.Metadata.OptionMetadata[] optionList = { };
            if (_Response.AttributeMetadata.GetType() == typeof(Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata))
            {
                Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata _BooleanAttributeMetadata =
                    (Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata)_Response.AttributeMetadata;
                Microsoft.Xrm.Sdk.Metadata.OptionMetadataCollection o =
                    new Microsoft.Xrm.Sdk.Metadata.OptionMetadataCollection();
                o.Add(_BooleanAttributeMetadata.OptionSet.FalseOption);
                o.Add(_BooleanAttributeMetadata.OptionSet.TrueOption);
                optionList = o.ToArray();
            }
            else if (_Response.AttributeMetadata.GetType() == typeof(Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata))
            {
                Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata _StatusAttributeMetadata =
                    (Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata)_Response.AttributeMetadata;
                optionList = _StatusAttributeMetadata.OptionSet.Options.ToArray();
            }
            else
            {
                Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata _PicklistAttributeMetadata =
                    (Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata)_Response.AttributeMetadata;
                optionList = _PicklistAttributeMetadata.OptionSet.Options.ToArray();
            }

            foreach (Microsoft.Xrm.Sdk.Metadata.OptionMetadata _Optionset in optionList)
            {
                _DropdownDatasource.Add(int.Parse(_Optionset.Value.ToString()), _Optionset.Label.UserLocalizedLabel.Label);
            }

            return _DropdownDatasource;

        }

        /// <summary>
        /// Obtiene el Valior del texto de un Pick List a través de su valor.
        /// </summary>
        /// <param name="svc">Proveedor de acceso a los metadatos de CRM</param>
        /// <param name="entidad">Entidad</param>
        /// <param name="optionset">Picklist a obtener</param>
        /// <param name="Valor">Valor del pick list en CRM, del cual se desea obtener el texto</param>
        /// <returns>Texto del Pick List</returns>
        private string DatoPickList(string entidad, string optionset, int Valor)
        {
            string Respuesta = string.Empty;

            Dictionary<int, string> lista = RetrieveOptionsetMetadata(entidad, optionset);
            if (lista.Count > 0)
            {
                string rolAcceso = (from r in lista
                                    where r.Key == Valor
                                    select r).FirstOrDefault().Value;

                Respuesta = rolAcceso;
            }

            return Respuesta;
        }


        private string ObtenerCampus(Guid VPDId)
        {
            string resultado = string.Empty;


            BusinessUnit un = new BusinessUnit();

            //un.ua_codigo_campus
            QueryExpression query = new QueryExpression(BusinessUnit.EntityLogicalName)
            {
                NoLock = true,
                //EntityName = EntityLogicalName,
                ColumnSet = new ColumnSet("name"),
                Criteria =
                {
                    Conditions = {
                         new ConditionExpression("businessunitid", ConditionOperator.Equal, VPDId)
                    }
                },
            };

            EntityCollection ec = _xrmServerConnection.RetrieveMultiple(query);

            if (ec.Entities.Any())
                if (ec.Entities[0].Contains("name"))
                    resultado = ec.Entities[0].GetAttributeValue<string>("name");

            return resultado;
        }


        #endregion

    }
}
