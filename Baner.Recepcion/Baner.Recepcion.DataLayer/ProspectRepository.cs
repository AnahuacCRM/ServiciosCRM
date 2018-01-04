using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Extensions;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;
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
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;

using XRM;

namespace Baner.Recepcion.DataLayer
{
    public class ProspectRepository : IProspectRepository
    {
        //private readonly ILogger _logger;
        private readonly ICatalogRepository _catalogRepository;
        private readonly IPickListRepository _picklistRepository;
        private IOrganizationService _xrmServerConnection2;
        private readonly IOrganizationService _xrmServerConnection;

        private readonly EntityReferenceTransformer _entityReferenceTransformer;
        private readonly PickListTransformer _pickListTransformer;
        private readonly IOpportunityRepository _OpportunityRepository;

        public string vsVersionAvanceFase = System.Configuration.ConfigurationManager.AppSettings["csVersionAvanceFase"];


        public CRM365.Conector.Service service { get; set; }
        public string vsRetornoSeguimiento { get; set; }

        public ProspectRepository(ICatalogRepository catalogRepository, IPickListRepository picklistRepository, IOpportunityRepository OpportunityRepository)
        {
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);


            _xrmServerConnection = this.service.OrganizationService;

            _catalogRepository = catalogRepository;
            _picklistRepository = picklistRepository;
            _OpportunityRepository = OpportunityRepository;
            _entityReferenceTransformer = new EntityReferenceTransformer(_catalogRepository);
            _pickListTransformer = new PickListTransformer(_picklistRepository);

        }
        //Segunda conexion
        public void Conexion2(string vpdp)
        {
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];
            //Ticket 4402, cambiar contraseñas de los usuarios Consulta
            switch (vpdp.ToUpper())
            {
                case "UAQ":
                    user = "consultauaq@anahuac.mx";
                    pass = "E;0P3@@H:";
                    break;
                case "UAX":
                    user = "consultauax@anahuac.mx";
                    pass = "KLDA9RIP#";
                    break;
                case "UAS":
                    user = "consultauas@anahuac.mx";
                    pass = "AFST>PH84";
                    break;
                case "UAN":
                    user = "consultauan@anahuac.mx";
                    pass = "@IPKMI5C;";
                    break;
                case "UAM":
                    user = "consultauam@anahuac.mx";
                    pass = "L1;LY>9GR";
                    break;
                case "UAC":
                    user = "consultauac@anahuac.mx";
                    pass = "CP=0ONG=N";
                    //user = "consultacrm3@anahuac.mx";
                    //pass = "N3wP@ss03";
                    break;
                case "UAP":
                    user = "consultauap@anahuac.mx";
                    pass = ":>D8>GB;P";
                    break;
                case "UAO":
                    user = "consultauao@anahuac.mx";
                    pass = "??8T?NECY";
                    break;

            }


            this.service = new CRM365.Conector.Service(org, "", user, pass);


            _xrmServerConnection2 = this.service.OrganizationService;
        }

        #region Integracion 2 Crar cuentas, contacto, Oportunidad
        public ResponseNewProspect CreateCuenta(Cuenta prospect)
        {
            Guid idcuentaCreada = new Guid();//default(Guid);
            Guid idOportunidadRegresar = default(Guid);
            bool isCreate = false;
            Dictionary<Guid, string> RegistrosCreados = new Dictionary<Guid, string>();
            ResponseNewProspect respuestaIntegracion = new ResponseNewProspect();
            Opportunity op3 = new Opportunity();
            vsRetornoSeguimiento = "Iniciando servicio 2";

            string colegioprocedenciastring = prospect.Colegio_Procedencia;

            if (prospect.Estatus_Solicitud != null)
                op3.ua_estatus_solicitud = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Opportunity.EntityLogicalName, "ua_estatus_solicitud", prospect.Estatus_Solicitud));


            #region validaciones de campos obligatorios 

            if (string.IsNullOrWhiteSpace(prospect.Programa1))
                throw new LookupException("el programa " + prospect.Programa1 + "  no puede ir vacio");
            Guid idPrograma = GetProgramaId(prospect.Programa1);

            if (string.IsNullOrWhiteSpace(prospect.PeriodoId))
                throw new LookupException("El periodo " + prospect.PeriodoId + "El Periodo no fue encontrado en catálogo.");
            Guid idp = RetrivePeriodoId(prospect.PeriodoId);

            var idVPD = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", prospect.Campus);
            // var campus = _entityReferenceTransformer.GetCampus(prospect.Campus.ToUpper());
            if (idVPD == null)
                throw new LookupException("la Vpd " + prospect.CampusVPD + "no fue encontrada en catálogo.");
            if (string.IsNullOrWhiteSpace(prospect.CampusVPD))
                throw new LookupException("la Vpd " + prospect.CampusVPD + "no fue encontrada en catálogo.");



            var idProgramaCampus = RetrieveProgramaByCarreraWebAsesor(new EntityReference(ua_programaV2.EntityLogicalName, idPrograma), idVPD, prospect.Campus, prospect.Programa1);

            if (!prospect.Correos.Any())
                throw new LookupException(" Es necesario al menos un correo para procesar la oportunidad.");

            var vpd = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", prospect.CampusVPD);

            EntityReference colegp = _entityReferenceTransformer.GetColegioProcedencia(prospect.Colegio_Procedencia);
            if (colegp != null)
            {

                // ua_colegios_asesor c = new ua_colegios_asesor();

                EntityReference entitirecolpN = GetDatoAsesor(ua_colegios_asesor.EntityLogicalName, colegp, vpd, "ua_colegios_asesorid"
                   , "ua_colegio_procedencia", "ua_codigo_vpd", "ua_colegios_asesor", prospect.Colegio_Procedencia, prospect.CampusVPD);

                if (entitirecolpN != null)
                {
                    prospect.Colegio_Procedencia = entitirecolpN.Id.ToString();
                }




            }
            else
            {
                respuestaIntegracion.Warnings.Add("WARNING PREPA: La clave " + prospect.Colegio_Procedencia + " del colegio de procedencia no se encontró en catálogo.");
                prospect.Colegio_Procedencia = "";
            }
            #endregion

            try
            {
                //Si no existe la cuenta, crea cuenta y contactos
                if (!ExisteCuenta(prospect.IdBanner, out idcuentaCreada))
                {
                    isCreate = true;
                    idcuentaCreada = CrearCuenta(prospect, respuestaIntegracion, vpd, colegioprocedenciastring);
                    vsRetornoSeguimiento += "Termina la creacion de la Cuenta." + (char)10;


                    #region Crear Contacto para una cuenta

                    Guid idConcCreado = CrearContacto(prospect, idcuentaCreada, RegistrosCreados, respuestaIntegracion);
                    vsRetornoSeguimiento += "Termina la creacion del Contacto con id ." + idConcCreado + (char)10;
                    #endregion
                }

                respuestaIntegracion.IdCRM = idcuentaCreada;

                Opportunity op = new Opportunity();
                //op.AccountId

                var OportunityRepository = new OpportunityRepository();
                //Si no tiene ninguna oportunidad relacionada esta cuenta, creamos el prospecto y lo calificamos
                if (!HayOportunidadRelacionadaAcuenta(idcuentaCreada))
                {
                    isCreate = true;
                    //Crear Oportunidad  

                    #region Calificar prospecto y convertirlo en oportunidad


                    //try
                    {

                        prospect.Nombre += " " + prospect.Segundo_Nombre + " " + SplitApellidoPaterno(prospect.Apellidos) + " " + SplitApellidoMaterno(prospect.Apellidos);

                        //Actualizamos la oportunidad que acabamos de combertir con los datos recibidos

                        vsRetornoSeguimiento += "Mandamos crear la oportunidad con la cuenta " + idcuentaCreada;

                        string sguiop = "";
                        // try
                        {
                            idOportunidadRegresar = OportunityRepository.CrearOportunidad(prospect, idcuentaCreada, _entityReferenceTransformer, true, colegioprocedenciastring);
                        }


                        vsRetornoSeguimiento += " el id oportunidad creada es =" + idOportunidadRegresar;



                        RegistrosCreados.Add(idOportunidadRegresar, Opportunity.EntityLogicalName);
                        ////Actualizamos la etapa a Solicitante
                        if (vsVersionAvanceFase == "1")
                            ActualizarFaseCono("Solicitante", "Opportunity Sales Process", idOportunidadRegresar);
                        //Fase2
                        else if (vsVersionAvanceFase == "2")
                            ActualizarFaseCono2(idOportunidadRegresar, OrigenOportundiad(idOportunidadRegresar));



                        respuestaIntegracion.IdOportunidad = idOportunidadRegresar;


                        vsRetornoSeguimiento += "Termina la creacion de la Oportunidad." + (char)10;
                    }


                    #endregion

                }
                else// delo contrario si ya tiene al menos una cuenta relacioanda solo le creamos una nueva opo
                {

                    string sguiop = "";

                    // try
                    {
                        vsRetornoSeguimiento += "ya tiene una oportunidad ." + (char)10;
                        isCreate = true;
                        //llaves compuestas para  la oportunidad que nunca se repetiran

                        prospect.Nombre += " " + prospect.Segundo_Nombre + " " + SplitApellidoPaterno(prospect.Apellidos) + " " + SplitApellidoMaterno(prospect.Apellidos);

                        idOportunidadRegresar = OportunityRepository.CrearOportunidad(prospect, idcuentaCreada, _entityReferenceTransformer, true, colegioprocedenciastring);
                    }
                    //catch (Exception ex)
                    //{
                    //    respuestaIntegracion.Seguimientos += vsRetornoSeguimiento + "--------" + ex.Message;
                    //    _xrmServerConnection.Delete(Account.EntityLogicalName, idcuentaCreada);
                    //    //return respuestaIntegracion;


                    //}
                    RegistrosCreados.Add(idOportunidadRegresar, Opportunity.EntityLogicalName);
                    ////Actualizamos la etapa a Solicitante
                    if (vsVersionAvanceFase == "1")
                        ActualizarFaseCono("Solicitante", "Opportunity Sales Process", idOportunidadRegresar);
                    //Fase2
                    else if (vsVersionAvanceFase == "2")
                        ActualizarFaseCono2(idOportunidadRegresar, OrigenOportundiad(idOportunidadRegresar));



                    respuestaIntegracion.IdOportunidad = idOportunidadRegresar;




                }




            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> crmEx)
            {
                System.Diagnostics.Debug.WriteLine(crmEx.Message);
                if (isCreate)
                {
                    vsRetornoSeguimiento += "Intentanto RollBack para la cuenta " + idcuentaCreada.ToString() + crmEx.Message + (char)10;
                    //RollBack si se inserto algo
                    if (idcuentaCreada != Guid.Empty)
                    {


                        //Eliminar todos los registros relacionados 
                        foreach (var item in RegistrosCreados)
                        {
                            _xrmServerConnection.Delete(item.Value, item.Key);

                        }
                        //Eliminar el prospecto
                        _xrmServerConnection.Delete(Account.EntityLogicalName, idcuentaCreada);
                    }
                }
                respuestaIntegracion.Seguimientos += vsRetornoSeguimiento;
                throw;
            }
            catch (Exception ex)
            {
                vsRetornoSeguimiento += "ERROR GENÉRICO: " + ex.Message + (char)10;
                if (isCreate)
                {
                    vsRetornoSeguimiento += "Intentanto RollBack para la cuenta " + idcuentaCreada.ToString() + ex.Message + (char)10;
                    //RollBack si se inserto algo
                    if (idcuentaCreada != Guid.Empty)
                    {


                        //Eliminar todos los registros relacionados 
                        foreach (var item in RegistrosCreados)
                        {
                            _xrmServerConnection.Delete(item.Value, item.Key);

                        }
                        //Eliminar el prospecto
                        _xrmServerConnection.Delete(Account.EntityLogicalName, idcuentaCreada);
                    }
                }
                respuestaIntegracion.Seguimientos += vsRetornoSeguimiento;
                throw;
            }

            return respuestaIntegracion;
        }

        #endregion



        #region Integracion 2 Creacion de prospecto
        public ResponseNewProspect Create(NewProspect prospect)
        {
            Stopwatch swgeneral = new Stopwatch();
            Stopwatch swmapeo = new Stopwatch();
            Stopwatch swguardar = new Stopwatch();
            swgeneral.Start();
            ResponseNewProspect respuestaIntegracion = new ResponseNewProspect();



            Debug.WriteLine("Tiempo mapeo|{0}", swmapeo.ElapsedMilliseconds);
            Debug.WriteLine("Tiempo guardado|{0}", swguardar.ElapsedMilliseconds);
            Debug.WriteLine("Tiempo total|{0}", swgeneral.ElapsedMilliseconds);
            return respuestaIntegracion;
        }
        #endregion

        #region Integracion 3 Examinado
        public bool UpdateExaminado(Examinado examinado)
        {
            var ret = false;
            var obj = examinado;
            var op = GetOpenOpportunity(obj.id_Oportunidad);
            Guid idc = IdCuentaByOportunid(new Guid(obj.id_Oportunidad));
            op.ua_codigo_campus = _entityReferenceTransformer.GetCampus(obj.Campus);
            Guid idprogrmaa = GetProgramaId(obj.Programa);
            op.ua_sobresaliente = obj.PuntualizacionSobresaliente.StringToBoolTransfom();
            op.ua_promedio = obj.PromedioPreparatoria;
            var tipoAlumnoEntityRef = GetIdReferencia(ua_tipoalumno.EntityLogicalName, "ua_tipoalumnoid", "ua_codigo_tipo_alumno", obj.TipoAlumno);
            if (tipoAlumnoEntityRef != null)
                op.ua_desc_tipo_alumno = tipoAlumnoEntityRef;
            else
                throw new Exception(string.Format("El tipo alumno: {0} no existe en catálogo", obj.TipoAlumno));
            try { _xrmServerConnection.Update(op); }
            catch (Exception ex) { string exmes = ex.Message; }
            #region Cambio de Fase a Examinado            
            try
            {
                if (vsVersionAvanceFase == "1")
                {
                    string origen = OrigenOportundiad((Guid)op.OpportunityId);
                    if (origen != "3" && origen != "4")
                        ActualizarFaseCono("Examinado", "Proceso de cliente potencial a ventas de la oportunidad", (Guid)op.OpportunityId);
                    else
                        ActualizarFaseCono("Examinado", "Opportunity Sales Process", (Guid)op.OpportunityId);
                }
                //Fase2
                else if (vsVersionAvanceFase == "2")
                    ActualizarFaseCono2((Guid)op.OpportunityId, OrigenOportundiad((Guid)op.OpportunityId));
            }
            catch (Exception ex) { throw new CRMException(String.Format("Ha ocurrido un error al actualizar el stage a Examinado {0}", ex.ToString())); }
            #endregion
            ret = true;
            return ret;
        }
        #endregion

        #region Integracion 4 Admitido
        public bool UpdateAdmitido(Admitido admitido)
        {
            var ret = false;
            var obj = admitido;
            var op = new Opportunity();
            Guid idOportunidad = new Guid(admitido.id_oportunidad);
            op.OpportunityId = idOportunidad;
            Guid idPrograma = GetProgramaId(obj.Programa);
            var campusEntityRef = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", obj.Campus);
            if (campusEntityRef != null)
                op.ua_codigo_campus = campusEntityRef;
            else
                throw new Exception(string.Format("El Campus: {0} no existe.", obj.Campus));
            var tipoDecisionEntityRef = GetIdReferencia(ua_tipo_decision.EntityLogicalName, "ua_tipo_decisionid", "ua_codigo_tipo_decision", obj.DesicionAdmision);
            if (tipoDecisionEntityRef != null)
                op.ua_desc_tipo_desicion = tipoDecisionEntityRef;
            else
                throw new Exception(string.Format("El campo Tipo Decision: {0} no existe.", obj.DesicionAdmision));

            var idProgramaCampus = RetrieveProgramaByCarreraWeb(new EntityReference(ua_programaV2.EntityLogicalName, idPrograma), op.ua_codigo_campus, obj.Campus, obj.Programa);
            op.ua_programav2 = idProgramaCampus;
            op.ua_programa_asesor = RetrieveProgramaByCarreraWebAsesor(new EntityReference(ua_programaV2.EntityLogicalName, idPrograma), campusEntityRef, obj.Campus, obj.Programa);
            op.ua_Programa = _entityReferenceTransformer.GetPrograma(obj.Programa);
            op.ua_desc_escuela = _entityReferenceTransformer.GetEscuela(obj.Escuela);
            var tipoAdmisionEntityRef = GetIdReferencia(ua_tipo_admision.EntityLogicalName, "ua_tipo_admisionid", "ua_codigo_tipo_admision", obj.TipoAdmision);
            if (tipoAdmisionEntityRef != null)
                op.ua_desc_tipo_admision = tipoAdmisionEntityRef;
            else
                throw new Exception(string.Format("El campo Tipo Admision: {0} no existe.", obj.TipoAdmision));
            var oppcurrentstatus = _OpportunityRepository.RetrieveStatusById(idOportunidad);

            if (string.IsNullOrWhiteSpace(obj.StatusOpo))
            {
                if (oppcurrentstatus == 4) //Cerrada
                {
                    _OpportunityRepository.ReopenOpportunity(Opportunity.EntityLogicalName, idOportunidad);
                }
            }
            //Si la oportunidad ya fue ganada no debe permitir ningun cambio
            else if (oppcurrentstatus == 3) //Ganada
            {
                throw new CRMException("La oportunidad ya fue ganada");
            }
            _xrmServerConnection.Update(op);
            #region Cerrar la oportunidad que le llego
            if (string.IsNullOrWhiteSpace(obj.StatusOpo))
            {
                try
                {
                    Guid idCuenta = IdCuentaByOportunid(idOportunidad);
                    //_OpportunityRepository.DeactivateOportunity(Opportunity.EntityLogicalName, idOportunidad);
                    var ops = _OpportunityRepository.RetrieveOportunidades(idCuenta, idOportunidad);
                    if (ops != null && ops.Any())
                    {
                        //cerrando oportunidades
                        foreach (var opportunityToClose in ops)
                        {
                            _OpportunityRepository.DeactivateOportunity(Opportunity.EntityLogicalName, opportunityToClose);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new CRMException(String.Format("Ha ocurrido un error al inactivar las oportunidades del solicitante: {0}", ex.ToString()));
                }

            }
            #endregion
            ret = true;
            return ret;
        }


        #endregion

        #region Integracion 5 Rechazado
        public bool UpdateRechazado(Rechazado rechazado)
        {
            var ret = false;
            var op = GetOpenOpportunity(rechazado.id_Oportunidad);
            Guid idCuenta = new Guid(rechazado.id_Cuenta);
            if (!string.IsNullOrEmpty(rechazado.id_Oportunidad.Trim()))
                op.OpportunityId = new Guid(rechazado.id_Oportunidad);
            else
                throw new Exception("El campo id oportunidad no puede ir vacio");

            var tipoDecisionEntityRef = GetIdReferencia(ua_tipo_decision.EntityLogicalName, "ua_tipo_decisionid", "ua_codigo_tipo_decision", rechazado.DecisionAdmision);
            if (tipoDecisionEntityRef != null)
                op.ua_desc_tipo_desicion = tipoDecisionEntityRef;
            else
                throw new Exception(string.Format("El campo Decision Admision: {0} no existe", rechazado.DecisionAdmision));

            _xrmServerConnection.Update(op);
            _OpportunityRepository.DeactivateOportunity(Opportunity.EntityLogicalName, (Guid)op.OpportunityId);
            #region Cerrar la oportunidad que le llego
            try
            {
                var ops = _OpportunityRepository.RetrieveOportunidades(idCuenta, (Guid)op.OpportunityId);
                if (ops != null && ops.Any())
                {
                    foreach (var opportunityToClose in ops)
                    {
                        _OpportunityRepository.DeactivateOportunity(Opportunity.EntityLogicalName, opportunityToClose);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CRMException(String.Format("Ha ocurrido un error al inactivar las oportunidades del solicitante: {0}", ex.ToString()));
            }

            #endregion
            return true;


        }
        #endregion

        #region Integracion 6 Inscrito
        public bool UpdateInscrito(Inscrito inscrito)
        {
            bool respuesta = false;
            Guid idOportunida = default(Guid);
            var OpInscrito = GetOpenOpportunity(inscrito.id_Oportunidad);
            //OpInscrito.ProcessId = new Guid();
            //OpInscrito.StageId = new Guid();
            #region Mapeo de campos
            idOportunida = new Guid(inscrito.id_Oportunidad);
            OpInscrito.OpportunityId = idOportunida;
            OpInscrito.ua_pago_insc = ResolveFecha(inscrito.FechaPagoInscripcion);
            #endregion
            _xrmServerConnection.Update(OpInscrito);
            //Fase 6 JC                
            try
            {
                if (vsVersionAvanceFase == "1")
                {
                    string origen = OrigenOportundiad(idOportunida);
                    if (origen != "3" && origen != "4")
                        ActualizarFaseCono("Inscrito", "Proceso de cliente potencial a ventas de la oportunidad", idOportunida);
                    else
                        ActualizarFaseCono("Inscrito", "Opportunity Sales Process", idOportunida);
                }
                //Fase2
                else if (vsVersionAvanceFase == "2")
                    ActualizarFaseCono2(idOportunida, OrigenOportundiad(idOportunida));
            }
            catch (Exception ex)
            {
                throw new CRMException(String.Format("Ha ocurrido un error al actualizar el stage a Inscrito {0}", ex.ToString()));
            }
            respuesta = true;
            return respuesta;
        }
        #endregion

        #region Integracion 7 Seleccion Curso
        public bool UpdateNuevoIngreso(NuevoIngreso nuevoingreso)
        {
            bool res = false;
            Guid idOportunidad = default(Guid);
            var OpNuevoIngreso = GetOpenOpportunity(nuevoingreso.Id_Oportunidad);
            idOportunidad = new Guid(nuevoingreso.Id_Oportunidad);
            OpNuevoIngreso.ua_fecha_sel_curso = ResolveFecha(nuevoingreso.FechaSeleccionCursos);
            //Actualiazmos los datos de la oportunidad en CRM
            _xrmServerConnection.Update(OpNuevoIngreso);
            //Fase 7 JC.
            try
            {
                if (vsVersionAvanceFase == "1")
                {
                    string origen = OrigenOportundiad(idOportunidad);
                    if (origen != "3" && origen != "4")
                        ActualizarFaseCono("Nuevo ingreso", "Proceso de cliente potencial a ventas de la oportunidad", idOportunidad);
                    else
                        ActualizarFaseCono("Nuevo ingreso", "Opportunity Sales Process", idOportunidad);
                }
                //Fase2
                else if (vsVersionAvanceFase == "2")
                    ActualizarFaseCono2(idOportunidad, OrigenOportundiad(idOportunidad));
            }
            catch (Exception ex)
            {
                throw new CRMException(String.Format("Ha ocurrido un error al actualizar el stage a Nuevo ingreso {0}", ex.ToString()));
            }
            _OpportunityRepository.CerrarOportunidadComoGanada(Opportunity.EntityLogicalName, idOportunidad);
            res = true;
            return res;
        }
        #endregion

        #region Integracion 8 Datos Persona
        public bool UpdateDatosPersona(DatosPersona datospersona)
        {
            Guid id_Cuenta = default(Guid);
            bool res = false;
            if (!string.IsNullOrWhiteSpace(datospersona.id_Cuenta))
                id_Cuenta = new Guid(datospersona.id_Cuenta);
            var cuenta = new Account();
            cuenta.AccountId = id_Cuenta;
            //c.AccountId = RetrieveCuentaId(datospersona.IdBanner);
            //c.ua_idbanner = datospersona.IdBanner;
            cuenta.Name = string.Format("{0} {1} {2} {3}", datospersona.Nombre, datospersona.Segundo_Nombre, SplitApellidoPaterno(datospersona.Apellidos), SplitApellidoMaterno(datospersona.Apellidos));
            cuenta.ua_fecha_nacimiento = datospersona.Fecha_Nacimiento != null ? ResolveFecha(datospersona.Fecha_Nacimiento) : null;
            cuenta.ua_sexo = !string.IsNullOrEmpty(datospersona.Sexo.Trim()) ? new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Account.EntityLogicalName, "ua_sexo", datospersona.Sexo)) : null;
            cuenta.ua_desc_nacionalidad = !string.IsNullOrEmpty(datospersona.Nacionalidad.Trim()) ? _entityReferenceTransformer.GetNacionalidad(datospersona.Nacionalidad) : null;
            cuenta.ua_religion = !string.IsNullOrEmpty(datospersona.Religion.Trim()) ? _entityReferenceTransformer.GetReligion(datospersona.Religion) : null;

            //Obtenemos el id del contacto principal relacionado con 
            Guid id_ContactoPrincipal = RetriveContactoPrincipalCuenta(id_Cuenta);

            //Actualizamos la cuenta
            _xrmServerConnection.Update(cuenta);



            //No se si se actualizaran estos datos a cada contacto que se encuentre relacionado a la cuenta o al contacto principal
            if (id_ContactoPrincipal != Guid.Empty)
            {


                var contacto = new Contact();
                contacto.ContactId = id_ContactoPrincipal;

                contacto.ua_desc_estado_civil = GetIdReferencia(ua_desc_estado_civil.EntityLogicalName, "ua_desc_estado_civilid", "ua_codigo_estado_civil", datospersona.Estado_Civil);
                contacto.FirstName = datospersona.Nombre;
                contacto.MiddleName = datospersona.Segundo_Nombre;
                contacto.LastName = string.Format("{0} {1}", SplitApellidoPaterno(datospersona.Apellidos), SplitApellidoMaterno(datospersona.Apellidos));
                contacto.ua_desc_religion = cuenta.ua_religion;
                contacto.ua_desc_Nacionalidad = cuenta.ua_desc_nacionalidad;

                //Actualizamos el contacto
                _xrmServerConnection.Update(contacto);


            }

            var ops = _OpportunityRepository.RetrieveProspectOportunidades(id_Cuenta);
            //Actualizamos todas las oportunidades abiertas relacionadas con la cuenta
            if (ops.Count > 0)
                ops.ToList().ForEach(e => _xrmServerConnection.Update(new Opportunity()
                {
                    OpportunityId = e,
                    Name = cuenta.Name,
                    ua_desc_nacionalidad = cuenta.ua_desc_nacionalidad

                }));
            #region Mapeo de Datos Persona
            res = true;
            #endregion
            return res;
        }
        #endregion

        #region Integracion 9 Datos Prepa
        public bool UpdateDatosPrepa(DatosPrepa datosPrepa)
        {
            bool resultado = false;
            Guid idCuenta = default(Guid);
            bool consultaPrepa = false;

            EntityReference vpd = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", datosPrepa.VPDI);

            var cuenta = new Account();
            if (!string.IsNullOrWhiteSpace(datosPrepa.id_Cuenta))
            {
                idCuenta = new Guid(datosPrepa.id_Cuenta);
                cuenta.AccountId = new Guid(datosPrepa.id_Cuenta);
            }
            //string bannerid = RetrieveBannerCuentaID(idCuenta);
            EntityReference colegiop = !string.IsNullOrEmpty(datosPrepa.Preparatoria.Trim()) ? _entityReferenceTransformer.GetColegioProcedencia(datosPrepa.Preparatoria) : null;
            if (colegiop != null)
            {
                cuenta.ua_colegio_asesor = GetDatoAsesor(ua_colegios_asesor.EntityLogicalName, colegiop, vpd, "ua_colegios_asesorid"
                       , "ua_colegio_procedencia", "ua_codigo_vpd", "ua_colegios_asesor", datosPrepa.Preparatoria, datosPrepa.VPDI);

                cuenta.ua_colegio_procedencia = colegiop;
                cuenta.ua_colegioguidstr = colegiop.Id.ToString();
                consultaPrepa = true;
            }


            cuenta.ua_promedio = datosPrepa.PromedioPreparatoria;

            _xrmServerConnection.Update(cuenta);

            //Actualizamos los campo  de las oportunidades ralacionadas con esta cuenta
            var ops = RetrieveOpportunityByVPD(RetrieveBannerCuentaID(idCuenta), datosPrepa.VPDI);
            // var ops = _OpportunityRepository.RetrieveProspectOportunidades(idCuenta);

            //Actualizamos todas las oportunidades abiertas relacionadas con la cuenta
            foreach (var itemOp in ops)
            {
                var op = new Opportunity();
                op.OpportunityId = itemOp;
                // op.ua_codigo_vpd = datosPrepa.VPDI;
                //op.ua_codigo_vpd_pl = vpd;
                op.ua_colegio_asesor = cuenta.ua_colegio_asesor; // _entityReferenceTransformer.GetColegioProcedencia(datosPrepa.Preparatoria);
                op.ua_colegio_procedencia = colegiop;
                op.ua_colegioGUIDStr = cuenta.ua_colegioguidstr;
                op.ua_promedio = datosPrepa.PromedioPreparatoria;


                _xrmServerConnection.Update(op);
            }
            if (!consultaPrepa)
            {
                throw new Exception("WARNING PREPA: La clave " + datosPrepa.Preparatoria + " del colegio de procedencia no se encontró en catálogo.");
            }
            resultado = true;

            return resultado;
        }
        #endregion

        #region Integracion 10 Cambia Solicitud de Admision
        public bool UpdateCambiaSolicitudAdmision(CambiaSolicitudAdmision cambiaSolicitudAdmision)
        {
            bool res = false;

            //ar op = new Opportunity();



            var op = GetOpenOpportunity(cambiaSolicitudAdmision.Id_Oportunidad.ToString());
            //if (op.OpportunityId != null)
            {
                op.OpportunityId = cambiaSolicitudAdmision.Id_Oportunidad;
                // oportunidad.ua_codigo_programa = cambiaSolicitudAdmision.Programa;

                Guid idPro = GetProgramaId(cambiaSolicitudAdmision.Programa);

                op.ua_codigo_campus = _entityReferenceTransformer.GetCampus(cambiaSolicitudAdmision.Campus);
                op.ua_programav2 = RetrieveProgramaByCarreraWeb(new EntityReference(ua_programaV2.EntityLogicalName, idPro), op.ua_codigo_campus, cambiaSolicitudAdmision.Campus, cambiaSolicitudAdmision.Programa);
                var vpdiEntityRef = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", cambiaSolicitudAdmision.VPDI);

                //op.ua_programa_asesor = GetDatoAsesor(ua_programas_por_campus_asesor.EntityLogicalName, new EntityReference(ua_programaV2.EntityLogicalName, idPro), op.ua_codigo_campus, "ua_programas_por_campus_asesorid",
                //    "ua_programas_por_campus", "ua_codigo_vpd", "ua_programas_por_campus_asesor");
                //op.ua_programa_asesor = RetrieveProgramaByCarreraWebAsesor(new EntityReference(ua_programaV2.EntityLogicalName, idPro), op.ua_codigo_campus, cambiaSolicitudAdmision.Campus, cambiaSolicitudAdmision.Programa);
                op.ua_programa_asesor = RetrieveProgramaByCarreraWebAsesor(new EntityReference(ua_programaV2.EntityLogicalName, idPro),
                    vpdiEntityRef,
                    cambiaSolicitudAdmision.Campus,
                    cambiaSolicitudAdmision.Programa);
                //op.ua_programa_asesor = GetDatoAsesor(ua_programas_por_campus_asesor.EntityLogicalName,
                //       new EntityReference(ua_programaV2.EntityLogicalName, idPro),
                //       vpdiEntityRef,
                //       "ua_programas_por_campus_asesorid",
                //       "ua_programas_por_campus",
                //       "ua_codigo_vpd",
                //       "ua_programas_por_campus_asesor",
                //       cambiaSolicitudAdmision.Programa,
                //       cambiaSolicitudAdmision.Campus);


                op.ua_Programa = _entityReferenceTransformer.GetPrograma(cambiaSolicitudAdmision.Programa);

                op.ua_desc_escuela = _entityReferenceTransformer.GetEscuela(cambiaSolicitudAdmision.Escuela);

                _xrmServerConnection.Update(op);
                res = true;
            }
            return res;
        }
        #endregion

        #region Integracion 11 Solicitante Tipo
        public bool UpdateCambiosTipoAdmision(CambiosTipoAdmision cambiotipoadmision)
        {
            bool res = false;
            //Antes actualizaba los mismos datos que la oportunidad
            Lead pros = new Lead();
            // pros.ua_codigo_vpd
            //pros.ua_periodo

            //r op = new Opportunity();


            var op = GetOpenOpportunity(cambiotipoadmision.Id_Oportunidad);
            // if (op.OpportunityId != null)
            {

                if (!string.IsNullOrEmpty(cambiotipoadmision.Id_Oportunidad.Trim()))
                    op.OpportunityId = new Guid(cambiotipoadmision.Id_Oportunidad);
                else
                    throw new Exception("El id oportunidad no puede ir vacio");

                var tipoAlumnoEntityRef = GetIdReferencia(ua_tipoalumno.EntityLogicalName, "ua_tipoalumnoid", "ua_codigo_tipo_alumno", cambiotipoadmision.TipoAlumno);

                if (tipoAlumnoEntityRef != null)
                    op.ua_desc_tipo_alumno = tipoAlumnoEntityRef;
                else
                    throw new Exception(string.Format("El Tipo Alumno: {0} no existe", cambiotipoadmision.TipoAlumno));
                var tipoAdmisionEntityRef = GetIdReferencia(ua_tipo_admision.EntityLogicalName, "ua_tipo_admisionid", "ua_codigo_tipo_admision", cambiotipoadmision.TipoAdmision);

                if (tipoAdmisionEntityRef != null)
                    op.ua_desc_tipo_admision = tipoAdmisionEntityRef;
                else
                    throw new Exception(string.Format("El Tipo Admision: {0} no existe", cambiotipoadmision.TipoAdmision));

                _xrmServerConnection.Update(op);
                res = true;
            }
            return res;
        }
        #endregion

        #region Integracion 12 Update Parentesco
        public GestionContactosWarning GestionContactos(Parentesco parentesco)
        {

            GestionContactosWarning warnin = new GestionContactosWarning();
            bool resultado = false;
            Guid idContacto = default(Guid);
            Guid idcuenta = new Guid(parentesco.id_Cuenta);
            Contact objContacto;
            string DescVPD = "";
            var entityrefVpd = GetVPDOfAccount(parentesco.id_Cuenta, out DescVPD);

            foreach (var itempariente in parentesco.PadreoTutor)
            {
                objContacto = new Contact();
                //tipoParentesco = itempariente.Parentesco;
                if (itempariente.Parentesco != null)
                    objContacto.ua_tipo_parentesco_cuenta = GetIdReferencia(ua_tipo_parentesco.EntityLogicalName, "ua_tipo_parentescoid", "ua_codigo_tipo_parentesco", itempariente.Parentesco);
                idContacto = GetIdPariente(idcuenta, objContacto.ua_tipo_parentesco_cuenta);
                if (idContacto != Guid.Empty)
                    continue;
                objContacto.FirstName = itempariente.FirstName;
                objContacto.MiddleName = itempariente.MiddleName;
                objContacto.LastName = SplitApellidoPaterno(itempariente.LastName) + " " + SplitApellidoMaterno(itempariente.LastName);

                objContacto.ua_Fallecio = RetroveFallecio(itempariente.Vive);
                var dir = itempariente.Direcciones;
                if (dir != null)
                {

                    objContacto.Address1_Line1 = dir.Calle;
                    objContacto.Address1_Line2 = dir.Numero;
                    objContacto.Address1_PostalCode = dir.CodigoPostal;




                    if (dir.Estado != null)
                    {
                        var entityEstado = _entityReferenceTransformer.GetEstadoSE(dir.Estado);

                        if (entityEstado != null)
                        {
                            objContacto.ua_codigo_estado = entityEstado;
                            objContacto.ua_estados_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entityEstado, entityrefVpd,
                                "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", dir.Estado, DescVPD);
                        }

                    }
                    if (objContacto.ua_estados_asesor == null) warnin.Warnings.Add(" el Estado " + dir.Estado + " no fue encontrado en el catalogo");

                    objContacto.ua_codigo_postal = dir.CodigoPostal;

                    if (!string.IsNullOrEmpty(dir.DelegacionMunicipioId))
                    {


                        var entitiDelegacion = _entityReferenceTransformer.GetMunicipioId(dir.DelegacionMunicipioId);
                        if (entitiDelegacion != null)
                        {
                            objContacto.ua_codigo_delegacion = entitiDelegacion;
                            objContacto.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entitiDelegacion, entityrefVpd,
                           "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", dir.DelegacionMunicipioId, DescVPD);

                        }

                    }
                    if (objContacto.ua_delegacion_municipio_asesor == null) warnin.Warnings.Add(" el Municipio " + dir.DelegacionMunicipioId + " no fue encontrado en el catalogo");

                    if (dir.PaisId != null)
                    {

                        var entitypais = _entityReferenceTransformer.GetPais(dir.PaisId);
                        if (entitypais != null)
                        {
                            objContacto.ua_codigo_pais = entitypais;
                            objContacto.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitypais, entityrefVpd,
                                 "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", dir.PaisId, DescVPD);
                        }
                    }
                    if (objContacto.ua_pais_asesor == null) warnin.Warnings.Add(" el Pais " + dir.PaisId + " no fue encontrado en el catalogo");


                    if (dir.Colonia != null && dir.CodigoPostal != null && objContacto.ua_codigo_estado != null && objContacto.ua_codigo_pais != null)
                    {
                        Domicilio dom1 = new Domicilio
                        {
                            CP = dir.CodigoPostal,
                            Colonia = dir.Colonia,
                            Estado = dir.Estado,
                            Pais = dir.PaisId,
                            Municipio = dir.DelegacionMunicipioId
                        };

                        objContacto.ua_colonia_1 = _entityReferenceTransformer.GetColonia(dom1);
                        if (objContacto.ua_colonia_1 == null)
                        {
                            warnin.Warnings.Add("la colonia " + dir.Colonia + " no fue encontrado en el catalogo");
                            // objContacto.ua_Colonia_Extranjera1 = dir.Colonia;
                        }
                    }


                    if (itempariente.Parentesco != null)
                        objContacto.ua_tipo_parentesco_cuenta = GetIdReferencia(ua_tipo_parentesco.EntityLogicalName, "ua_tipo_parentescoid", "ua_codigo_tipo_parentesco", itempariente.Parentesco);
                    if (dir.TipoDireccionId != null)
                        objContacto.ua_tipo_direccion1 = GetIdReferencia(ua_tipo_direccion.EntityLogicalName, "ua_tipo_direccionid", "ua_codigo_tipo_direccion", dir.TipoDireccionId);


                }

                if (idContacto == Guid.Empty) //Nuevo
                {
                    #region nuevo pariente 
                    objContacto.ParentCustomerId = new EntityReference(Account.EntityLogicalName, idcuenta);

                    var idContactoCreado = _xrmServerConnection.Create(objContacto);

                    resultado = true;

                    #endregion

                }
                else                //Editamos
                {
                    //objContacto.ContactId = idContacto;

                    //#region Editamos el pariente 

                    //_xrmServerConnection.Update(objContacto);
                    //resultado = true;

                    //#endregion

                }
            }

            return warnin;
        }
        #endregion

        #region Integracion 13 Examen Admision (Pendiente de ajustar)
        public bool FechaExamenAdmision(FechaExamenAdmision fechaExamenAdmision)
        {
            bool res = false;

            //pportunity OP = new Opportunity();


            var OP = GetOpenOpportunity(fechaExamenAdmision.id_Oportunidad);
            // if (OP.OpportunityId != null)
            {
                OP.OpportunityId = new Guid(fechaExamenAdmision.id_Oportunidad);
                OP.ua_sesion_examen = fechaExamenAdmision.SessionExamen;


                if (fechaExamenAdmision.lstExamenes != null)
                {
                    if (fechaExamenAdmision.lstExamenes.Any())
                    {
                        foreach (var examen in fechaExamenAdmision.lstExamenes)
                        {
                            if (string.IsNullOrWhiteSpace(examen.ClaveExamen))
                            {
                                throw new CRMException("Se envio el registro incompleto para la fecha de admision");
                            }




                            if (examen.ClaveExamen == "PAA")
                            {
                                OP.ua_fecha_paa = (examen.FechaExamen != null) ? OP.ua_fecha_paa = examen.FechaExamen.GetDate() : null;
                                // OP.ua_fecha_paa = examen.FechaExamen.GetDate();
                            }
                            else if (examen.ClaveExamen == "EOV")
                            {

                                OP.ua_fecha_eov = (examen.FechaExamen != null) ? OP.ua_fecha_eov = examen.FechaExamen.GetDate() : null;

                            }

                        }
                    }
                    else
                    {
                        OP.ua_fecha_paa = null;
                        OP.ua_fecha_eov = null;
                    }
                }
                else
                {
                    OP.ua_fecha_paa = null;
                    OP.ua_fecha_eov = null;
                }
                // OP.ua_periodo = new EntityReference(ua_periodo.EntityLogicalName, RetrivePeriodoId(fechaExamenAdmision.Periodo));

                _xrmServerConnection.Update(OP);
                res = true;
            }
            return res;
        }
        #endregion

        #region Integracion 14 Resultado Examen

        /// <summary>
        /// Actualizar el resultado de examen
        /// Controller: srvCambioResultadoExamenController
        /// </summary>
        /// <param name="presultadoExamen"></param>
        public bool UpdateResultadoExamen(ResultadoExamen presultadoExamen)
        {
            var resultado = false;
            Guid idCuenta = default(Guid);
            if (string.IsNullOrWhiteSpace(presultadoExamen.id_Cuenta))
                throw new InvalidExamCoode("El id_Cuenta del examen no puede ir vacio");
            else
                idCuenta = new Guid(presultadoExamen.id_Cuenta);

            var op = new Opportunity();
            op.ua_codigo_vpd = presultadoExamen.VPDI;
            //op.ua_codigo_vpd = _entityReferenceTransformer.GetCampus(presultadoExamen.VPDI);
            op.ua_paa = false;
            var ListOportunidadesII = new List<Guid>();
            ListOportunidadesII = RetrieveOpportunityByVPD(RetrieveBannerCuentaID(idCuenta), presultadoExamen.VPDI);

            foreach (var itemResultEx in presultadoExamen.ResultadosdeExamen)
            {

                switch (itemResultEx.CodigoExamen)
                {
                    case "PAAN":
                        //op.ua_paa =itemResultEx.BanderaScore.StringToBoolTransfom();
                        op.ua_fecha_paan = itemResultEx.FechaResultado.GetDate();
                        break;
                    case "PAAV":
                        // op.ua_paav = itemResultEx.BanderaScore.StringToBoolTransfom();
                        op.ua_fecha_paav = itemResultEx.FechaResultado.GetDate();
                        break;
                    case "PARA":
                        // op.ua_para = item.BanderaScore.StringToBoolTransfom();
                        op.ua_fecha_para = itemResultEx.FechaResultado.GetDate();
                        break;
                    case "MMPI":
                        //op.rs_mmpia = item.BanderaScore.StringToBoolTransfom();
                        op.ua_fecha_mmpi = itemResultEx.FechaResultado.GetDate();
                        break;
                    case "ESCM":
                        // op.rs_escm = item.BanderaScore.StringToBoolTransfom();
                        op.ua_fecha_escm = itemResultEx.FechaResultado.GetDate();
                        break;
                    case "ESCP":
                        // op.rs_escei = item.BanderaScore.StringToBoolTransfom();
                        op.ua_fecha_escei = itemResultEx.FechaResultado.GetDate();
                        break;
                    default:
                        throw new InvalidExamCoode(string.Format("El código de Examen proporcionado no es valido {0}", itemResultEx.CodigoExamen));
                }
            }

            //Cuando llegan todas las obligatorias
            if (op.ua_fecha_paan != null && op.ua_fecha_para != null && op.ua_fecha_paav != null)
                op.ua_paa = true;
            //Cuando llega una que no es de las obligatorias
            else if (op.ua_fecha_paan == null && op.ua_fecha_para == null && op.ua_fecha_paav == null)
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_paan", "ua_fecha_para", "ua_fecha_paav");
            else if (op.ua_fecha_paan != null && op.ua_fecha_para != null) //Cuando llegan dos de las obligatorias
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_paav", "", "");
            else if (op.ua_fecha_paan != null && op.ua_fecha_paav != null) //Cuando llegan dos de las obligatorias
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_para", "", "");
            else if (op.ua_fecha_para != null && op.ua_fecha_paav != null) //Cuando llegan dos de las obligatorias
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_paan", "", "");
            //cuando llena una de las obligatorias
            else if (op.ua_fecha_paan != null)
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_para", "ua_fecha_paav", "");
            else if (op.ua_fecha_para != null)
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_paan", "ua_fecha_paav", "");
            else if (op.ua_fecha_paav != null)
                op.ua_paa = ExistenFechasExamen(ListOportunidadesII[0], "ua_fecha_paan", "ua_fecha_para", "");




            // bool paa = false;
            foreach (var OportunidadId in ListOportunidadesII)
            {

                op.OpportunityId = OportunidadId;
                _xrmServerConnection.Update(op);

                //var consultarPAA = _xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, OportunidadId, new ColumnSet(new string[] { "ua_paa" }));
                //if (consultarPAA != null)
                //{
                //    if (consultarPAA.Attributes.Contains("ua_paa"))
                //    {
                //        paa = (bool)consultarPAA.Attributes["ua_paa"];
                //        if (paa == false)
                //        {
                //            op.ua_paa = true;
                //            _xrmServerConnection.Update(op);
                //        }

                //    }
                //}
                resultado = true;
            }

            return resultado;
        }

        #endregion

        #region Integracion 15 Propedeutico
        public bool UpdatePropedeutico(Propedeutico propedeutico)
        {
            Guid idOportunidad = default(Guid);
            if (string.IsNullOrEmpty(propedeutico.id_Oportunidad.Trim()))
                throw new Exception("El id oportunidad no puede ir vacio");
            else
                idOportunidad = new Guid(propedeutico.id_Oportunidad);
            var op = GetOpenOpportunity(propedeutico.id_Oportunidad);
            // var op = new Opportunity();
            op.OpportunityId = idOportunidad;
            // apuntan a donde mismo o diferente catalogo?
            //op.ua_periodo = _entityReferenceTransformer.GetPeriodo(propedeutico.PeriodoPL);
            op.ua_periodo_pl = _entityReferenceTransformer.GetPeriodo(propedeutico.PeriodoPL);
            //Es entero o string el que recibe
            op.ua_solicitud_pl = propedeutico.SolicitudAdmisionPL;
            Guid program = GetProgramaId(propedeutico.ProgramaPL);
            if (program != Guid.Empty)
                op.ua_codigo_programa_pl = new EntityReference(ua_programaV2.EntityLogicalName, program);

            //Ver a que catalogo apunta

            if (string.IsNullOrEmpty(propedeutico.DecisionAdmision.Trim()))
                op.ua_desc_tipo_decision_pl = null;
            else
            {

                var tipoDecisionEntityRef = GetIdReferencia(ua_tipo_decision.EntityLogicalName, "ua_tipo_decisionid", "ua_codigo_tipo_decision", propedeutico.DecisionAdmision);
                if (tipoDecisionEntityRef != null)
                    op.ua_desc_tipo_decision_pl = tipoDecisionEntityRef;
                else
                    throw new Exception(string.Format("Decision Admision: {0} no existe.", propedeutico.DecisionAdmision));
            }

            //op.ua_codigo_vpd = propedeutico.VPDI;
            op.ua_codigo_vpd_pl = _entityReferenceTransformer.GetCampus(propedeutico.VPDI);

            //op.ua_codigo_campus = _entityReferenceTransformer.GetCampus(propedeutico.CampusAdmisionPL);

            if (string.IsNullOrEmpty(propedeutico.CampusAdmisionPL.Trim()))
                op.ua_codigo_campus_pl = null;
            else
                op.ua_codigo_campus_pl = _entityReferenceTransformer.GetCampus(propedeutico.CampusAdmisionPL);

            op.ua_genero_pl = true;
            //Debe de ser un programa pl
            //Guid idprogrmaa = GetProgramaId(propedeutico.ProgramaPL);



            _xrmServerConnection.Update(op);

            return true;
        }
        #endregion

        #region Integracion 16 Proceso Incompleto 
        public bool UpdateExaminadoPI(ExaminadoPI examinadoPI)
        {
            var idOportunidad = default(Guid);
            if (string.IsNullOrEmpty(examinadoPI.id_Oportunidad.Trim()))
                throw new LookupException("El id_Oportunidad no puede ir vacio.");
            else
                idOportunidad = new Guid(examinadoPI.id_Oportunidad);
            //rop = new Opportunity();

            var ret = false;

            var op = GetOpenOpportunity(examinadoPI.id_Oportunidad);
            // if (op.OpportunityId != null)
            {
                op.OpportunityId = idOportunidad;
                //Ver en el crm a que catalogo apunta
                op.ua_desc_tipo_desicion = GetIdReferencia(ua_tipo_decision.EntityLogicalName, "ua_tipo_decisionid", "ua_codigo_tipo_decision", examinadoPI.DecisionAdmision);
                if (op.ua_desc_tipo_desicion == null)
                    throw new Exception(" El tipo de admision " + examinadoPI.DecisionAdmision + "  no se encontro en el catalogo");
                //var oport1 = _xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, idOportunidad, new ColumnSet { AllColumns = true });
                _xrmServerConnection.Update(op);
                //var col = new ColumnSet
                // var ti = _xrmServerConnection.Retrieve(ua_tipo_decision.EntityLogicalName, new Guid("3532a14e-230d-e711-810b-e0071b6aa0a1"), new ColumnSet { AllColumns = true });
                ret = true;
            }
            return ret;


        }
        #endregion

        #region Integracion 17 Preuniversitario
        public Guid CreatePreUniversitario(PreUniversitario preUniversitario)
        {

            Guid res = Guid.Empty;
            Lead Prospecto = new Lead();

            string CodigoPrograma = "";
            Prospecto.FirstName = preUniversitario.Nombre;
            Prospecto.MiddleName = preUniversitario.Segundo_Nombre;
            Prospecto.LastName = preUniversitario.Apellido_Paterno + " " + preUniversitario.Apellido_Materno;
            Prospecto.Subject = preUniversitario.Nombre + " " + preUniversitario.Segundo_Nombre + " " + Prospecto.LastName;

            Prospecto.Telephone1 = preUniversitario.Telefono_Lada + preUniversitario.Telefono_Numero;

            Prospecto.EMailAddress1 = preUniversitario.Correo_Electronico;


            if (preUniversitario.Nivel != null)
                Prospecto.ua_codigo_nivel = _entityReferenceTransformer.GetNivel(preUniversitario.Nivel.ToUpper());
            //Falta campo a tabla de campus
            if (preUniversitario.Campus != null)
                Prospecto.ua_codigo_campus = _entityReferenceTransformer.GetCampus(preUniversitario.Campus.ToUpper());


            Prospecto.ua_vpd = _entityReferenceTransformer.GetCampus(preUniversitario.VPD.ToUpper());
            Prospecto.ua_codigo_vpd2 = Prospecto.ua_vpd;
            //se usa para que el campo caiga en oportunidad
            Prospecto.ua_codigo_vpd = preUniversitario.VPD;

            //Antes era rs_programa1
            if (!string.IsNullOrWhiteSpace(preUniversitario.Codigo))
            {

                //Prospecto.ua_programa = RetrieveProgramaByCarreraWeb(RetrivePrograma(preUniversitario.Codigo, out CodigoPrograma), Prospecto.ua_codigo_campus);
                //Get idcarreraweb
                var programabycarrera = RetrieveProgramaByCarreraWeb(RetrivePrograma(preUniversitario.Codigo), Prospecto.ua_codigo_campus, out CodigoPrograma, preUniversitario.Campus, preUniversitario.Codigo);
                //Get progama por campus idm
                Prospecto.ua_programav2 = GetProgramaByVPd(CodigoPrograma, Prospecto.ua_codigo_campus);
                ua_programas_por_campus_asesor co = new ua_programas_por_campus_asesor();

                //Get programa por campus asesor
                Prospecto.ua_programas_por_campus_asesor = GetDatoAsesor(ua_programas_por_campus_asesor.EntityLogicalName, Prospecto.ua_programav2, Prospecto.ua_vpd, "ua_programas_por_campus_asesorid",
                    "ua_programas_por_campus", "ua_codigo_vpd", "ua_programas_por_campus_asesor", CodigoPrograma, preUniversitario.VPD);

                Prospecto.ua_Programa = _entityReferenceTransformer.GetPrograma(CodigoPrograma);



            }

            if (!string.IsNullOrWhiteSpace(preUniversitario.Pais))
            {

                var entitypais = _entityReferenceTransformer.GetPais(preUniversitario.Pais);
                if (entitypais != null)
                    Prospecto.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitypais, Prospecto.ua_vpd,
                         "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", preUniversitario.Pais, preUniversitario.VPD);

                Prospecto.ua_codigo_pais = entitypais;
            }
            if (!string.IsNullOrWhiteSpace(preUniversitario.Estado))
            {
                var entityEstado = _entityReferenceTransformer.GetEstadoSE(preUniversitario.Estado.ToUpper());

                if (entityEstado != null)
                    Prospecto.ua_estado_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entityEstado, Prospecto.ua_vpd,
                        "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", preUniversitario.Estado, preUniversitario.VPD);

                Prospecto.ua_codigo_estado = entityEstado;

            }
            if (!string.IsNullOrWhiteSpace(preUniversitario.Municipio))
            {
                var entityDelegacionM = _entityReferenceTransformer.GetMunicipioId(preUniversitario.Municipio.ToUpper());
                if (entityDelegacionM != null)
                    Prospecto.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityDelegacionM, Prospecto.ua_vpd,
                        "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", preUniversitario.Municipio, preUniversitario.VPD);

                Prospecto.ua_codigo_delegacion = entityDelegacionM;
            }
            if (!string.IsNullOrEmpty(preUniversitario.Periodo))
                Prospecto.ua_periodo = _entityReferenceTransformer.GetPeriodo(preUniversitario.Periodo);





            //Banner =3
            //WEb o CRM = 1
            //Formulario APREU=2
            if (preUniversitario.Origen != null)
                Prospecto.ua_origen = new OptionSetValue(int.Parse(preUniversitario.Origen));
            Prospecto.ua_suborigen = preUniversitario.SubOrigen;


            Prospecto.ua_asignar_asesor = new OptionSetValue(2);

            //Prospecto.rs_informacioncorrecta = true;
            //Prospecto.ua_AsignarAsesor=


            res = _xrmServerConnection2.Create(Prospecto);



            return res;

        }
        #endregion


        #region Metodos utilitarios.
        public void email_send()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("ing.gucane@gmail.com");
            mail.To.Add("gcn@bbpingod.com");
            mail.Subject = "Test Mail - 1";
            mail.Body = "mail with attachment";

            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            //mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("ing.gucane@gmail.com", "3x1t0swG");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }

        private bool ExisteProspecto2(string prospe)
        {


            Opportunity op = new Opportunity();

            bool res = false;
            string spros = "";
            ua_logidbanner bl = new ua_logidbanner();

            QueryExpression Query = new QueryExpression(ua_logidbanner.EntityLogicalName)
            {
                NoLock = true,
                //ColumnSet = new ColumnSet(new string[] { "ua_prospectoid", "ua_cuentaid","ua_oportunidadid","ua_logidbannerid" }),
                ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_prospectoid", ConditionOperator.Equal, prospe),

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Contactoprim = ec.Entities.FirstOrDefault();

                //resultado = new Guid(Contactoprim.Attributes["primarycontactid"].ToString());
                if (Contactoprim.Attributes.Contains("ua_prospectoid"))
                {
                    spros = Contactoprim.Attributes["ua_prospectoid"].ToString();
                    res = true;
                }

            }
            return res;
        }

        private void RegistrarLogBanner()
        {
            ua_logidbanner log = new ua_logidbanner();
            log.ua_Prospectoid = "Proespecto";
            log.ua_oportunidadid = "Oportunidad";
            log.ua_cuentaid = "Cuenta";
            log.ua_bannerlogid = "idbanner";
            var r = _xrmServerConnection.Create(log);

        }

        public bool ExisteProspecto(string prospe, out LogBanner bannerlog)
        {
            bannerlog = null;
            bool res = false;
            string spros = "";
            ua_logidbanner bl = new ua_logidbanner();

            QueryExpression Query = new QueryExpression(ua_logidbanner.EntityLogicalName)
            {
                NoLock = true,
                //ColumnSet = new ColumnSet(new string[] { "ua_prospectoid", "ua_cuentaid","ua_oportunidadid","ua_logidbannerid" }),
                ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_prospectoid", ConditionOperator.Equal, prospe),

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                res = true;
                var Contactoprim = ec.Entities.FirstOrDefault();
                bannerlog = new LogBanner();
                //resultado = new Guid(Contactoprim.Attributes["primarycontactid"].ToString());
                if (Contactoprim.Attributes.Contains("ua_prospectoid"))
                    bannerlog.Propecto = Contactoprim.Attributes["ua_prospectoid"].ToString();

                if (Contactoprim.Attributes.Contains("ua_oportunidadid"))
                    bannerlog.Oportunidad = Contactoprim.Attributes["ua_oportunidadid"].ToString();

                if (Contactoprim.Attributes.Contains("ua_cuentaid"))
                    bannerlog.Cuenta = Contactoprim.Attributes["ua_cuentaid"].ToString();

                if (Contactoprim.Attributes.Contains("ua_logidbannerid"))
                    bannerlog.Banner = Contactoprim.Attributes["ua_logidbannerid"].ToString();




            }
            return res;
        }

        private string GetCuentaOpo()
        {
            //ua_desc_nacionalidad b = new ua_desc_nacionalidad();

            string retn = "";
            //EntityReference refnacioi = new EntityReference(ua_desc_nacionalidad.EntityLogicalName, new Guid(idnacionaldiad));
            Guid idNacGuid = new Guid("2d694f0f-ce5c-e711-8104-c4346bdcf2f1");
            Account c = new Account();

            var descn = _xrmServerConnection.Retrieve(Account.EntityLogicalName, idNacGuid, new ColumnSet(new string[] { "ua_idbanner" }));
            if (descn != null)
            {
                retn = descn.Attributes["ua_idbanner"].ToString();
            }


            Guid idopo = new Guid("2b694f0f-ce5c-e711-8104-c4346bdcf2f1");
            Opportunity o = new Opportunity();

            var oport = _xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, idopo, new ColumnSet(new string[] { "ua_idbanner" }));
            if (oport != null)
            {
                var opidb = oport.Attributes["ua_idbanner"].ToString();
            }

            return retn;

        }

        private string GetidProspectoByName(string firsname, string midname, string namec)
        {

            string nomre = "";
            //Lead p = new Lead();
            Opportunity op = new Opportunity();

            QueryExpression Query = new QueryExpression(Account.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "name", "ua_idbanner" }),
                Criteria = {
                    Conditions = {

                        new ConditionExpression("name", ConditionOperator.Equal, namec),

                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                nomre = prospecto.Attributes["name"].ToString();


            }

            return nomre;

        }

        #endregion

        #region Integracion 18 Solicita Beca
        public Guid CreateSolicitaBeca(SolicitaBeca solicitaBeca)
        {
            Guid IdBeca = default(Guid);
            Guid idCuenta = default(Guid);
            if (!string.IsNullOrEmpty(solicitaBeca.id_Cuenta.Trim()))
                idCuenta = new Guid(solicitaBeca.id_Cuenta);
            else
                throw new Exception("El campo Id Cuenta no puede ir vacio");
            string idbanerN = RetrieveBannerCuentaID(idCuenta);

            foreach (var item in solicitaBeca.SolicitudBecas)
            {
                var obj = new ua_solicituddebeca();
                obj.ua_sol_beca_idbanner = idbanerN + "-" + item.Periodo;
                obj.ua_AccountSolicituddeBecaId = new EntityReference(Account.EntityLogicalName, idCuenta);
                obj.ua_Idbanner = idbanerN;// RetrieveBannerCuentaID(idCuenta);
                obj.ua_Tipodebeca = item.TipoBeca;
                obj.ua_beca_sol_desc = item.DescripcionBeca;
                obj.ua_beca_sol_vpd = _entityReferenceTransformer.GetCampus(item.CampusVPDI);
                obj.ua_beca_sol_periodo = item.Periodo;
                obj.ua_beca_sol_fecha = ResolveFecha(item.FechaSolicitudBeca);
                IdBeca = ExisteSolBeca(obj.ua_Idbanner, obj.ua_beca_sol_vpd, item.TipoBeca, item.Periodo, idCuenta);
                if (IdBeca == Guid.Empty)
                {
                    IdBeca = _xrmServerConnection.Create(obj);
                }
                else
                {
                    var upd = new ua_solicituddebeca()
                    {

                        ua_AccountSolicituddeBecaId = obj.ua_AccountSolicituddeBecaId,
                        ua_Idbanner = obj.ua_Idbanner,
                        ua_solicituddebecaId = IdBeca,
                        ua_beca_sol_desc = item.DescripcionBeca,
                        ua_beca_sol_fecha = ResolveFecha(item.FechaSolicitudBeca)
                    };
                    _xrmServerConnection.Update(upd);
                }
            }
            return IdBeca;
        }
        #endregion

        #region Integracion 19 Otorga Beca
        public bool OtorgamientoaBeca(OtorgamientoBeca pOtorgaBeca)
        {
            bool res = false;
            if (string.IsNullOrWhiteSpace(pOtorgaBeca.Id_Cuenta))
                throw new Exception("La cuenta " + pOtorgaBeca.Id_Cuenta + " no puede ir vacia");

            Guid lIdCuenta = new Guid(pOtorgaBeca.Id_Cuenta);

            ua_becaotorgada BecOtorg = new ua_becaotorgada();
            //Parametros para valdiar que exista
            //Identificador Cuenta CRM
            //descripcion crédito
            //VPDI
            //Periodo


            //BecOtorg.ua_beca_ot_vpd
            //BecOtorg.ua_beca_ot_periodo
            //BecOtorg.ua_BecasOtorgadasId
            //BecOtorg.ua_beca_ot_tipo

            //Cuenta asociada a la beca otorgada
            BecOtorg.ua_BecasOtorgadasId = new EntityReference(ua_becaotorgada.EntityLogicalName, lIdCuenta);
            //Banner otorgado a la beca otorgada
            BecOtorg.ua_IDbanner = RetrieveBannerCuentaID(lIdCuenta);

            if (pOtorgaBeca.lstBeca.Any())
            {
                foreach (var itemBecaOt in pOtorgaBeca.lstBeca)
                {
                    BecOtorg.ua_beca_otorgada = BecOtorg.ua_IDbanner + "-" + itemBecaOt.Beca.Periodo;
                    BecOtorg.ua_beca_ot_tipo = itemBecaOt.Beca.TipoBeca;
                    BecOtorg.ua_beca_ot_desc = itemBecaOt.Beca.DescripcionBeca;
                    BecOtorg.ua_beca_ot_vpd = _entityReferenceTransformer.GetCampus(itemBecaOt.Beca.CampusVPDI);
                    BecOtorg.ua_beca_ot_periodo = _entityReferenceTransformer.GetPeriodo(itemBecaOt.Beca.Periodo);
                    BecOtorg.ua_beca_ot_fecha = ResolveFecha(itemBecaOt.FechaOtorgaBeca);
                    BecOtorg.ua_beca_ot_fecha_venc = ResolveFecha(itemBecaOt.FechaVencimientoBeca);
                    Guid idbeca = ExisteBecaOtorgada(BecOtorg.ua_IDbanner, BecOtorg.ua_beca_ot_vpd, itemBecaOt.Beca.TipoBeca, BecOtorg.ua_beca_ot_periodo, lIdCuenta);

                    if (idbeca != Guid.Empty)
                    {
                        BecOtorg.ua_becaotorgadaId = idbeca;
                        _xrmServerConnection.Update(BecOtorg);
                    }
                    else
                        _xrmServerConnection.Create(BecOtorg);
                }
            }

            return res;
        }

        #endregion

        #region Integracion 20 Otorga Credito
        public Guid CreateOtorgaCredito(OtorgaCredito otorgaCredito)
        {
            Guid idCreditOtorgado = default(Guid);
            Guid IdCuentacredito = default(Guid);
            if (string.IsNullOrWhiteSpace(otorgaCredito.id_Cuenta))
                throw new Exception("El id de la cuenta " + otorgaCredito.id_Cuenta + " no puede ir vacio.");
            else
                IdCuentacredito = new Guid(otorgaCredito.id_Cuenta);

            //Instansiamos el objero credito 
            var creditoO = new ua_credito_educativo_otorgado();

            //cuenta asociada con el credito otorgado
            creditoO.ua_CrditoEducativoId = new EntityReference(Account.EntityLogicalName, IdCuentacredito);
            //Idbanenr de la cuetna asociada
            creditoO.ua_Idbanner = RetrieveBannerCuentaID(IdCuentacredito);

            foreach (var itemCreditos in otorgaCredito.InfoCreditos)
            {
                creditoO.ua_credito_educativo = creditoO.ua_Idbanner + "-" + itemCreditos.Periodo;
                creditoO.ua_credito_desc = itemCreditos.DescripcionCredito;
                creditoO.ua_credito_vpd = _entityReferenceTransformer.GetCampus(itemCreditos.CampusVPDI);
                creditoO.ua_credito_periodo = _entityReferenceTransformer.GetPeriodo(itemCreditos.Periodo);
                creditoO.ua_credito_fecha = ResolveFecha(itemCreditos.FechaOtorgaCredito);
                //Validamos si ya hay un credito disponible asociado a ese banner y esas claves
                Guid CreditoOtor = RetriveBecaCreditoId(creditoO.ua_Idbanner, itemCreditos.DescripcionCredito, creditoO.ua_credito_vpd, creditoO.ua_credito_periodo);
                if (CreditoOtor == Guid.Empty)
                    idCreditOtorgado = _xrmServerConnection.Create(creditoO);
                else
                {
                    creditoO.ua_credito_educativo_otorgadoId = CreditoOtor;
                    _xrmServerConnection.Update(creditoO);
                    idCreditOtorgado = CreditoOtor;
                }
            }
            return idCreditOtorgado;
        }

        #endregion 

        #region Integracion 21 Cambio SGASTDN
        public bool UpdateCambioSGASTDN(CambioSGASTDN cambioSGASTDN)
        {
            bool resultado = false;
            Guid idOportundad = default(Guid);

            var op = GetOpenOpportunity(cambioSGASTDN.id_Oportunidad.ToString());
            // if (op.OpportunityId != null)
            {
                if (!string.IsNullOrEmpty(cambioSGASTDN.id_Oportunidad.ToString().Trim()))
                {
                    idOportundad = new Guid(cambioSGASTDN.id_Oportunidad.ToString());
                    // var op = new Opportunity();
                    op.OpportunityId = idOportundad;
                    Guid idPrograma = GetProgramaId(cambioSGASTDN.Programa);

                    var campusEntityRef = _entityReferenceTransformer.GetCampus(cambioSGASTDN.Campus);
                    var vpdiEntityRef = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", cambioSGASTDN.VPDI);

                    var idProgramaCampus = RetrieveProgramaByCarreraWeb(new EntityReference(ua_programaV2.EntityLogicalName, idPrograma), campusEntityRef, cambioSGASTDN.Campus, cambioSGASTDN.Programa);
                    op.ua_codigo_campus = campusEntityRef;
                    op.ua_programav2 = idProgramaCampus;

                    //Ticket 4570: Se ajusta para que tome la VPD y no el campus en la búsqueda del programa por campus asesor.
                    //op.ua_programa_asesor = GetDatoAsesor(ua_programas_por_campus_asesor.EntityLogicalName, new EntityReference(ua_programaV2.EntityLogicalName, idProgramaCampus.Id)
                    //    , campusEntityRef, "ua_programas_por_campus_asesorid", "ua_programas_por_campus", "ua_codigo_vpd", "ua_programas_por_campus_asesor", cambioSGASTDN.Programa, cambioSGASTDN.Campus);
                    op.ua_programa_asesor = GetDatoAsesor(ua_programas_por_campus_asesor.EntityLogicalName,
                        new EntityReference(ua_programaV2.EntityLogicalName, idProgramaCampus.Id),
                        vpdiEntityRef,
                        "ua_programas_por_campus_asesorid",
                        "ua_programas_por_campus",
                        "ua_codigo_vpd",
                        "ua_programas_por_campus_asesor",
                        cambioSGASTDN.Programa,
                        cambioSGASTDN.Campus);
                    //Fin ticket 4570.
                    op.ua_Programa = _entityReferenceTransformer.GetPrograma(cambioSGASTDN.Programa);

                    op.ua_desc_escuela = _entityReferenceTransformer.GetEscuela(cambioSGASTDN.Escuela);

                    _xrmServerConnection.Update(op);
                    resultado = true;
                }
            }
            return resultado;
        }
        #endregion

        #region Integracion 22 Transferencia
        public bool UpdateTransferencia(Transferencia transferencia)
        {
            Guid IdOportunidad = default(Guid);
            if (!string.IsNullOrEmpty(transferencia.id_Oportunidad.Trim()))
                IdOportunidad = new Guid(transferencia.id_Oportunidad);
            else
                throw new Exception("El id oportunidad no puede ir vacio");
            //var op = new Opportunity();

            var ret = false;

            var op = GetOpenOpportunity(transferencia.id_Oportunidad);
            // if (op.OpportunityId != null)
            {

                op.OpportunityId = IdOportunidad;
                //op.ua_periodo = _entityReferenceTransformer.GetPeriodo(transferencia.Periodo);
                op.ua_campus_origen = _entityReferenceTransformer.GetCampus(transferencia.Campus_Origen);
                op.ua_campus_destino = _entityReferenceTransformer.GetCampus(transferencia.Campus_Destino);

                _xrmServerConnection.Update(op);
                _OpportunityRepository.DeactivateOportunity(Opportunity.EntityLogicalName, IdOportunidad);
                ret = true;
            }

            return ret;
        }
        #endregion

        #region Integracion 23 Cambia Domicilio
        public bool UpdateCambiaDomicilio(CambiaDomicilio domicilio)
        {
            bool res = false;


            return res;
        }
        #endregion

        #region Integracion 24 Cambia Telefono
        public bool CambiaTelefono(CambiaTelefono cambiatelefono)
        {
            bool res = false;




            return res;
        }

        #endregion

        #region Integracion 25 Cambia Email
        public bool UpdateCambiaEmail(CambiaEmail cambiaEmail)
        {
            bool res = false;




            return res;
        }
        #endregion

        #region Integracion 29 Catalogo Colegios
        public bool UpdateCatalogoColegios(CatalogoColegios catalogoColegios)
        {
            bool res = false;
            //10 banner
            // Account cuenta = new Account();
            ua_colegios colegio = new ua_colegios();


            colegio.ua_codigo_colegio = catalogoColegios.Clave_Colegio;
            colegio.ua_desc_colegios = catalogoColegios.Nombre_Colegio;
            colegio.ua_colegio_calle = catalogoColegios.Calle;
            colegio.ua_colegio_numero = catalogoColegios.Numero;
            colegio.ua_colegio_colonia = catalogoColegios.Colonia;
            if (!string.IsNullOrWhiteSpace(catalogoColegios.Municipio))
            {
                Municipio municipio = new Municipio();
                municipio.CodigoMunicipio = catalogoColegios.Municipio;
                municipio.Estado = catalogoColegios.Estado;
                // cuenta.rs_delegacionmunicipioid = _entityReferenceTransformer.GetMunicipioSE(municipio);
                colegio.ua_colegio_delegacion = _entityReferenceTransformer.GetMunicipioId(municipio.CodigoMunicipio);
            }
            if (!string.IsNullOrWhiteSpace(catalogoColegios.Estado))
                colegio.ua_colegio_estado = _entityReferenceTransformer.GetEstadoSE(catalogoColegios.Estado);
            if (!string.IsNullOrWhiteSpace(catalogoColegios.Pais))
            {
                try
                {

                    colegio.ua_colegio_pais = _entityReferenceTransformer.GetPais(catalogoColegios.Pais);
                }
                catch (Exception)
                {
                }
            }

            if (!string.IsNullOrWhiteSpace(catalogoColegios.Codigo_Postal))
            {
                try
                {
                    //cuenta.rs_codigopostalid = _entityReferenceTransformer.GetCodigoPostal(catalogoColegios.CodigoPostal);
                    colegio.ua_colegio_codigo_postal = catalogoColegios.Codigo_Postal;
                }
                catch (Exception ex)
                {
                    if (!ex.ToString().Contains("No se pudo resolver el Lookup de código Postal:"))
                        throw new LookupException(ex.ToString());
                }

            }

            colegio.ua_colegio_tipo_colegio = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(ua_colegios.EntityLogicalName, "ua_colegio_tipo_colegio", catalogoColegios.Tipo_Colegio));

            // cuenta.rs_tipocolegio = _pickListTransformer.GetTipoColegio(catalogoColegios.TipoColegio);

            //colegio.ua_colegio_tipo_colegio = _pickListTransformer.GetTipoColegio(catalogoColegios.TipoColegio);
            // colegio.ua_colegio_tipo_colegio = new OptionSetValue(GetTipoColegioEnum(catalogoColegios.Tipo_Colegio));


            #region Registro en CRM

            Guid Coleid = RetriveAccountId(catalogoColegios.Clave_Colegio);
            if (Coleid == Guid.Empty)
                Coleid = _xrmServerConnection.Create(colegio);
            else
            {
                colegio.ua_colegiosId = Coleid;
                _xrmServerConnection.Update(colegio);
            }

            #endregion




            res = true;

            return res;
        }
        #endregion

        #region Integracion 30 Catalogo Periodos
        public bool UpdateCatalogoPeriodos(CatalogoPeriodos catalogoPeriodos)
        {
            bool res = false;

            // rs_periodo periodo = new rs_periodo();

            ua_periodo periodo = new ua_periodo();

            periodo.ua_periodo1 = catalogoPeriodos.Periodo;
            // periodo.ua_Tipo_Periodo = new OptionSetValue(GetTipoPeriodo(catalogoPeriodos.Tipo_Periodo));
            periodo.ua_Tipo_Periodo = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(ua_periodo.EntityLogicalName, "ua_tipo_periodo", catalogoPeriodos.Tipo_Periodo));
            periodo.ua_anio_academico = catalogoPeriodos.Ano_academico;
            periodo.ua_Desc_periodo = catalogoPeriodos.Descripcion;
            periodo.ua_fecha_incio_periodo = catalogoPeriodos.Fecha_de_Inicio_Periodo.GetDate();
            periodo.ua_fecha_fin_periodo = catalogoPeriodos.Fecha_de_Fin_Periodo.GetDate();
            periodo.ua_fecha_inicio_alojamiento = catalogoPeriodos.Fecha_Inicio_Alojamiento.GetDate();
            periodo.ua_fecha_fin_alojamiento = catalogoPeriodos.Fecha_Fin_Alojamiento.GetDate();
            Guid PeriodoId = RetrivePeriodo30(catalogoPeriodos.Periodo);


            #region Registro en CRM
            if (PeriodoId == Guid.Empty)
                _xrmServerConnection.Create(periodo);
            else
            {
                periodo.ua_periodoId = PeriodoId;
                _xrmServerConnection.Update(periodo);
            }
            res = true;
            #endregion

            return res;
        }
        #endregion

        #region Integracion 31 Alta de Oportunidad

        public ResponseNewProspect AltaOportunidad(Oportunidad pOportunidad)
        {
            //var c=  NotificaConexionCRM();


            Guid id_Cuenta = default(Guid);
            Guid id_ContactoPrincipal = default(Guid);
            Guid id_OportunidadCreada = default(Guid);
            Guid id_ProspectoCreado = default(Guid);
            ResponseNewProspect respuestaIntegracion = new ResponseNewProspect();
            //Creamos el objeto cuenta para actualizar la oportunidad
            Cuenta cuenta = new Cuenta
            {
                Numero_Solicitud = pOportunidad.Numero_Solicitud,
                CampusVPD = pOportunidad.VPD,
                Campus = pOportunidad.Campus,
                Estatus_Solicitud = pOportunidad.Estatus_Solicitud,
                PeriodoId = pOportunidad.Periodo,
                Nivel = pOportunidad.Nivel,
                Programa1 = pOportunidad.Programa,
                Escuela = pOportunidad.Escuela,
                Codigo_Tipo_Alumno = pOportunidad.Codigo_Tipo_Alumno,
                Codigo_Tipo_admision = pOportunidad.Codigo_Tipo_admision,




            };

            string spromedio = "", colegioprocedenciastring = "";

            if (!string.IsNullOrWhiteSpace(pOportunidad.id_Cuenta))
            {
                id_Cuenta = new Guid(pOportunidad.id_Cuenta);
                respuestaIntegracion.IdCRM = id_Cuenta;

            }


            //Obtenemos el id del contacto principal relacionado con 
            id_ContactoPrincipal = RetriveContactoPrincipalCuenta(id_Cuenta);


            string idbanner = RetrieveBannerCuentaID(id_Cuenta);

            cuenta.IdBanner = idbanner;


            var OportunityRepository = new OpportunityRepository();

            Guid idOportunidadRegresar = default(Guid);


            Guid idPrograma = GetProgramaId(pOportunidad.Programa);

            var idVPD = GetIdReferencia(BusinessUnit.EntityLogicalName, "businessunitid", "name", pOportunidad.VPD);


            //var idProgramaCampus = RetrieveProgramaByCarreraWebAsesor(new EntityReference(ua_programaV2.EntityLogicalName, idPrograma), idVPD, pOportunidad.Campus, pOportunidad.Programa);

            Guid idp = RetrivePeriodoId(pOportunidad.Periodo);

            //Obtener Pais, Estado, Delegacion Municipio de contacto
            //Contact con = new Contact();
            dynamic xrNombreContacto = null;
            if (id_ContactoPrincipal != Guid.Empty)
                xrNombreContacto = _xrmServerConnection.Retrieve(Contact.EntityLogicalName, id_ContactoPrincipal, new ColumnSet(new string[] { "ua_pais_asesor", "ua_estados_asesor", "ua_delegacion_municipio_asesor" }));

            //Obtiene la informacion de la cuenta
            //var xrNombreCuenta = _xrmServerConnection.Retrieve(Account.EntityLogicalName, id_Cuenta, new ColumnSet(new string[] { "name", "ua_colegio_asesor", "ua_promedio", "ua_desc_nacionalidad",
            //        "ua_pais_asesor","ua_estado_asesor","ua_delegacion_municipio_asesor","ua_colegio_procedencia"}));

            var xrNombreCuenta = _xrmServerConnection.Retrieve(Account.EntityLogicalName, id_Cuenta, new ColumnSet(new string[] { "name", "ua_colegio_asesor", "ua_promedio", "ua_desc_nacionalidad", "ua_colegio_procedencia" }));



            Direccion dirCuenta = null;
            if (xrNombreCuenta != null)
            {
                dirCuenta = new Direccion();
                cuenta.Direcciones = new List<Direccion>();
                dirCuenta.TipoDireccionId = "PR";
                if (xrNombreCuenta.Attributes.Contains("name"))
                    cuenta.Nombre = xrNombreCuenta.Attributes["name"].ToString();


                if (xrNombreCuenta.Attributes.Contains("ua_promedio"))
                    cuenta.Promedio = xrNombreCuenta.Attributes["ua_promedio"].ToString();

                if (xrNombreCuenta.Attributes.Contains("ua_desc_nacionalidad"))
                    cuenta.Nacionalidad = ((EntityReference)xrNombreCuenta.Attributes["ua_desc_nacionalidad"]).Id.ToString();
                if (!string.IsNullOrEmpty(cuenta.Nacionalidad))
                {
                    cuenta.Nacionalidad = GetDescripcionNacionalidad(cuenta.Nacionalidad);
                }


            }
            //respuestaIntegracion.Seguimientos = " idOportundiad Recibida " + pOportunidad.id_Oportunidad;
            if (!string.IsNullOrWhiteSpace(pOportunidad.id_Oportunidad))
            {
                id_OportunidadCreada = OportunityRepository.UpdateOportunidad(cuenta, new Guid(pOportunidad.id_Oportunidad), _entityReferenceTransformer, "");
                if (vsVersionAvanceFase == "1")
                {
                    string origen = OrigenOportundiad(new Guid(pOportunidad.id_Oportunidad));
                    if (origen != "3" && origen != "4")
                        //if (!string.IsNullOrEmpty(origen))
                        ActualizarFaseCono("Solicitante", "Proceso de cliente potencial a ventas de la oportunidad", new Guid(pOportunidad.id_Oportunidad));
                    else
                        ActualizarFaseCono("Solicitante", "Opportunity Sales Process", new Guid(pOportunidad.id_Oportunidad));
                }
                //Fase2
                else if (vsVersionAvanceFase == "2")
                    ActualizarFaseCono2(new Guid(pOportunidad.id_Oportunidad), OrigenOportundiad(new Guid(pOportunidad.id_Oportunidad)));

            }
            else//si no existe, se crea de cero
            {
                Opportunity op = new Opportunity();


                if (!ExisteOportunidad31(pOportunidad.VPD, idbanner, pOportunidad.Numero_Solicitud.ToString(), idp, out idOportunidadRegresar))
                {
                    Account c = new Account();


                    if (xrNombreCuenta != null)
                    {


                        string valorstrin = "";

                        if (xrNombreCuenta.Attributes.Contains("ua_colegio_asesor"))
                        {
                            ua_colegios_asesor cpl = new ua_colegios_asesor();

                            var entiticolegiocuenta = ((EntityReference)xrNombreCuenta.Attributes["ua_colegio_asesor"]);
                            var entitycolegio = GetDatosAsesorCuenta(ua_colegios_asesor.EntityLogicalName, entiticolegiocuenta, "ua_colegios_asesorid", "ua_colegio_procedencia", "ua_colegios_asesor", out valorstrin);
                            if (entitycolegio != null)
                            {
                                var newColegio = GetDatoAsesor(ua_colegios_asesor.EntityLogicalName, entitycolegio, idVPD, "ua_colegios_asesorid"
                            , "ua_colegio_procedencia", "ua_codigo_vpd", "ua_colegios_asesor", valorstrin, pOportunidad.VPD);

                                cuenta.Colegio_Procedencia = newColegio.Id.ToString();

                                if (xrNombreCuenta.Attributes.Contains("ua_colegio_procedencia"))
                                {
                                    var colcuentanormalito = ((EntityReference)xrNombreCuenta.Attributes["ua_colegio_procedencia"]);
                                    var colegionormalcuenta = _xrmServerConnection.Retrieve(ua_colegios.EntityLogicalName, colcuentanormalito.Id, new ColumnSet(new string[] { "ua_colegiosid", "ua_codigo_colegio" }));
                                    if (colegionormalcuenta != null)
                                    {
                                        if (colegionormalcuenta.Attributes.Contains("ua_codigo_colegio"))
                                            colegioprocedenciastring = colegionormalcuenta.Attributes["ua_codigo_colegio"].ToString();

                                    }
                                }




                            }

                        }


                        if (xrNombreContacto != null)
                        {
                            if (xrNombreContacto.Attributes.Contains("ua_pais_asesor"))
                            {
                                //Dato obtenido de la cuenta
                                var entitypaisCuenta = ((EntityReference)xrNombreContacto.Attributes["ua_pais_asesor"]);

                                valorstrin = "";


                                //  obtenemos el id del pais normalito que es una columna de pais asesor
                                var entitiPais = GetDatosAsesorCuenta(ua_pais_asesor.EntityLogicalName, entitypaisCuenta, "ua_pais_asesorid", "ua_pais", "ua_paises_asesor", out valorstrin);
                                if (entitiPais != null)
                                {
                                    var res = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitiPais, idVPD,
                                    "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", valorstrin, pOportunidad.VPD);
                                    dirCuenta.PaisId = res.Id.ToString();
                                }

                            }


                            if (xrNombreContacto.Attributes.Contains("ua_estados_asesor"))
                            {
                                // ua_estados_asesor est = new ua_estados_asesor();
                                valorstrin = "";
                                var entityestadocuenta = (EntityReference)xrNombreContacto.Attributes["ua_estados_asesor"];

                                var entitiEstado = GetDatosAsesorCuenta(ua_estados_asesor.EntityLogicalName, entityestadocuenta, "ua_estados_asesorid", "ua_estados", "ua_estados_asesor", out valorstrin);
                                if (entitiEstado != null)
                                {
                                    var estnew = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entitiEstado, idVPD,
                                     "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", valorstrin, pOportunidad.VPD);
                                    dirCuenta.Estado = estnew.Id.ToString();


                                }



                            }


                            if (xrNombreContacto.Attributes.Contains("ua_delegacion_municipio_asesor"))
                            {
                                //ua_delegacion_municipio_asesor del = new ua_delegacion_municipio_asesor();
                                valorstrin = "";
                                var entitymunicipioCuenta = (EntityReference)xrNombreContacto.Attributes["ua_delegacion_municipio_asesor"];

                                var entitymuni = GetDatosAsesorCuenta(ua_delegacion_municipio_asesor.EntityLogicalName, entitymunicipioCuenta, "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_delegacion_municipio_asesor", out valorstrin);
                                if (entitymuni != null)
                                {
                                    var muninew = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entitymuni, idVPD,
                                     "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", valorstrin, pOportunidad.VPD);
                                    dirCuenta.DelegacionMunicipioId = muninew.Id.ToString();
                                }
                            }

                        }

                        cuenta.Direcciones.Add(dirCuenta);

                    }

                    // id_ProspectoCreado = CrearProspectoGenerico(Prospec);

                    //id_OportunidadCreada = ConvertirProspectoEnOportunidad(id_Cuenta, id_ProspectoCreado);

                    string seop = "";
                    // OportunityRepository.UpdateOportunidad(cuenta, id_OportunidadCreada, _entityReferenceTransformer);
                    id_OportunidadCreada = OportunityRepository.CrearOportunidad(cuenta, id_Cuenta, _entityReferenceTransformer, false, colegioprocedenciastring);

                    //Creamos una nueva oportunidad
                    //id_OportunidadCreada = OportunityRepository.CrearOportunidad(cuenta, id_Cuenta, _entityReferenceTransformer);
                    // ActualizarFaseCono("Solicitante", "Proceso de cliente potencial a ventas de la oportunidad", id_OportunidadCreada);
                    if (vsVersionAvanceFase == "1")
                        ActualizarFaseCono("Solicitante", "Opportunity Sales Process", id_OportunidadCreada);
                    //Fase2
                    else if (vsVersionAvanceFase == "2")
                        ActualizarFaseCono2(id_OportunidadCreada, OrigenOportundiad(id_OportunidadCreada));
                    //respuestaIntegracion.Seguimientos += "   Creo una nueva oportundiad su id = " + id_OportunidadCreada;


                }
                else
                    throw new Exception(" ya existe la oportunidad que desea crear");
                //Actualizamos la oportunidad que acabamos de combertir con los datos recibidos

            }
            respuestaIntegracion.IdOportunidad = id_OportunidadCreada;

            return respuestaIntegracion;
        }
        #endregion

        #region Integracion 32 Estatus Alumno
        public bool UpdateEstatusAlumno(EstatusAlumno estatusalumno)
        {
            ValidarEstatusAlumno(estatusalumno);
            var obj = estatusalumno;
            // var cuenta = new Account();
            // cuenta.AccountId = RetrieveCuentaId(obj.Id_Banner);
            // cuenta.ua_idbanner = obj.Id_Banner;
            //Actualizar cuenta
            // _xrmServerConnection.Update(cuenta);

            EntityReference entityRef = null;

            var op = new Opportunity();
            // op.ua_idbanner = obj.Id_Banner;
            // op.ua_codigo_campus = _entityReferenceTransformer.GetCampus(obj.Campus);
            //op.ua_periodo = _entityReferenceTransformer.GetPeriodo(obj.Periodo);
            op.OpportunityId = new Guid(obj.Id_Oportunidad);
            entityRef = GetIdReferencia(ua_estatusalumno.EntityLogicalName, "ua_estatusalumnoid", "ua_codigo_estatus_alumno", obj.Estatus);
            //op.ua_estatus_alumno == entityRef ?? throw new Exception(string.Format("Estatus: {0} no existe en catálogo.", obj.Estatus));

            //Actualizamos los campos de las oportunidades ralacionadas con esta cuenta.
            _xrmServerConnection.Update(op);
            return true;
        }

        /// <summary>
        /// Valida que todos los atributos tengan valor asignado
        /// </summary>
        /// <param name="obj"></param>
        void ValidarEstatusAlumno(EstatusAlumno obj)
        {
            int cont = 0;
            // cont += string.IsNullOrEmpty(obj.Id_Banner.Trim()) ? 1 : 0;
            //  cont += string.IsNullOrEmpty(obj.Campus.Trim()) ? 1 : 0;
            //cont += string.IsNullOrEmpty(obj.Periodo.Trim()) ? 1 : 0;
            cont += string.IsNullOrEmpty(obj.Id_Oportunidad.Trim()) ? 1 : 0;
            cont += string.IsNullOrEmpty(obj.Estatus.Trim()) ? 1 : 0;
            if (cont > 0)
                throw new Exception("Campos obligatorios:Id_Oportunidad, Estatus");
        }



        #endregion


        #region Integracion 33 Envio Oportunidad
        /*
         * La integracion 33 se consume desde un plugin y es para el envio de oportunidad
         */
        #endregion

        #region Integracion 34 Validacion de Duplicados
        /*
          Esta en el repositorio del Oportunidad
          Metodo en Repositorio Oportunidad : ObtenerPreOportunidad
         */
        #endregion

        #region Integracion 35 Marcar registro como Transferencia

        /// <summary>
        /// Esta en el repositorio de la Oportunidad 
        /// Metodo en Repositorio Oportunidad: MarcarTransferido
        /// </summary>
        /// <param name="obj"></param>
        public bool MarcarTransferido(MarcaTransferido obj)
        {
            var op = GetDefaultOpportunity(obj.id_Oportunidad);
            op.ua_campus_origen = _entityReferenceTransformer.GetCampus(obj.Campus_Origen);
            _xrmServerConnection.Update(op);
            return true;
        }


        #endregion

        #region 36 Notifica Status Solicitu es un plug in

        #endregion

        #region 37 Cambio a fase  solicitante
        public bool CambioFaseSolicitante(CambioFaseSolicitante cambiofase)
        {
            var res = false;
            Guid idOportunidad = default(Guid);
            if (!string.IsNullOrWhiteSpace(cambiofase.id_oportunidad))
                idOportunidad = new Guid(cambiofase.id_oportunidad);
            switch (cambiofase.Fase)
            {
                //Avance de Fase
                case 100:
                    {
                        string origenOpo = OrigenOportundiad(idOportunidad);
                        ActualizarFaseCono2(idOportunidad, origenOpo);
                        break;
                        //Microsoft.Crm.Sdk.Messages.
                    }
                    //Guardar Fase Actual en variable.
                case 101:
                    var OpInscrito = GetOpenOpportunity(cambiofase.id_oportunidad);
                    string origen100 = OrigenOportundiad(idOportunidad);
                    #region Mapeo de campos
                    //idOportunidad = new Guid(idOportunidad);
                    OpInscrito.OpportunityId = idOportunidad;
                    OpInscrito.ua_faseactual = ObtenerFaseActual(idOportunidad, origen100);
                    #endregion
                    _xrmServerConnection.Update(OpInscrito);
                    break;
                    //Corrección de Fases.
                case 102:
                    string origen101 = OrigenOportundiad(idOportunidad);
                    CorreccionFases(idOportunidad, origen101);
                    break;

                case 31:
                    //if (vsVersionAvanceFase == "1")
                    //{
                    //    string origen = OrigenOportundiad(idOportunidad);
                    //    if (origen != "3" && origen != "4")
                    //        //if (!string.IsNullOrEmpty(origen))
                    //        ActualizarFaseCono("Solicitante", "Proceso de cliente potencial a ventas de la oportunidad", idOportunidad);
                    //    else
                    //        ActualizarFaseCono("Solicitante", "Opportunity Sales Process", idOportunidad);
                    //}
                    ////Fase2
                    //else if (vsVersionAvanceFase == "2")
                    //    ActualizarFaseCono2(idOportunidad, OrigenOportundiad(idOportunidad));
                    res = true;
                    break;
                case 4:
                case 2:
                case 3:
                case 6:
                case 7:

                    {
                        ////si el origen de la oportundiad es nullo o bacio, indica que nacio desde cuenta
                        //string origenOp = OrigenOportundiad(idOportunidad);
                        //if (!string.IsNullOrWhiteSpace(origenOp))
                        //{
                        //    string SubOrigenOp = SubOrigenOportundiad(idOportunidad);
                        //    if (SubOrigenOp == "Oportunidad")
                        //        //Cambiamos de fase para el proceso de creacion de nueva oportunidad  a la cuenta
                        //        ActualizarFaseCono("Admitido", "Opportunity Sales Process", idOportunidad);
                        //    else
                        //        //Cambiamos de fase para el proceso de creacion de nueva oportunidad  a la cuenta
                        //        ActualizarFaseCono("Admitido", "Proceso de cliente potencial a ventas de la oportunidad", idOportunidad);


                        //    res = true;
                        //}
                        //else
                        //{
                        //    ActualizarFaseCono("Admitido", "Opportunity Sales Process", idOportunidad);

                        //}
                        //Fase 4 JC
                        string Stage = "";
                        if (cambiofase.Fase == 2)
                            Stage = "Solicitante";
                        else if (cambiofase.Fase == 3)
                            Stage = "Examinado";
                        else if (cambiofase.Fase == 4)
                            Stage = "Admitido";
                        else if (cambiofase.Fase == 6)
                            Stage = "Inscrito";
                        else if (cambiofase.Fase == 7)
                            Stage = "Nuevo ingreso";
                        try
                        {
                            if (vsVersionAvanceFase == "1")
                            {
                                string origen = OrigenOportundiad(idOportunidad);
                                origen = OrigenOportundiad(idOportunidad);
                                if (origen != "3" && origen != "4")
                                    //if (!string.IsNullOrEmpty(origen))
                                    ActualizarFaseCono(Stage, "Proceso de cliente potencial a ventas de la oportunidad", idOportunidad);
                                else
                                    ActualizarFaseCono(Stage, "Opportunity Sales Process", idOportunidad);
                            }
                            //Fase2
                            else if (vsVersionAvanceFase == "2")
                                ActualizarFaseCono2(idOportunidad, OrigenOportundiad(idOportunidad));
                        }
                        catch (Exception ex)
                        {
                            throw new CRMException(String.Format("Ha ocurrido un error al actualizar el stage a Admitido{0}", ex.ToString()));
                        }
                        break;
                    }

                


            }

            //Cuando se crea de banner a una cuenta
            //ActualizarFaseCono("Solicitante", "Opportunity Sales Process", idOportunidad);


            return res;
        }
        #endregion

        #region 38 GetCatalogoPais

        public List<Pais> GetCatalogoPaises()
        {
            return GetAllPaises();

        }

        #endregion

        #region 38-1(39) Obtiene todos los registros del catálogo de Estados

        public List<Estado> GetCatalogoEstados()
        {
            return GetAllEstados();

        }

        #endregion

        #region 38-2(40) obtenemos todos los registros del catalogo de Municipios

        public List<Municipios> GetCatalogoMunicipios()
        {
            return GetallMunicipios();

        }

        #endregion

        #region 38-3(41) obtenemos todos los registros del catalogo de Colegios

        public List<Colegios> GetCatalogoColegios()
        {
            return GetallColegioProcedencia();

        }

        #endregion

        #region 42 Registrar nuevo prospecto(Formulario Becarios)

        public Guid FormulariosBecarios(Becario becario)
        {
            //Guid idBecario = default(Guid);

            Guid res = Guid.Empty;
            Lead Prospecto = new Lead();

            string CodigoPrograma = "";
            Prospecto.FirstName = becario.Nombre;
            Prospecto.MiddleName = becario.Segundo_Nombre;
            Prospecto.LastName = becario.Apellido_Paterno + " " + becario.Apellido_Materno;
            Prospecto.Subject = becario.Nombre + " " + becario.Segundo_Nombre + " " + Prospecto.LastName;

            var vpd = _entityReferenceTransformer.GetCampus(becario.VPD);
            EntityReference colegiop = !string.IsNullOrEmpty(becario.Colegio.Trim()) ? _entityReferenceTransformer.GetColegioProcedencia(becario.Colegio) : null;
            if (colegiop != null)
            {
                Prospecto.ua_colegio_asesor = GetDatoAsesor(ua_colegios_asesor.EntityLogicalName, colegiop, vpd, "ua_colegios_asesorid"
                       , "ua_colegio_procedencia", "ua_codigo_vpd", "ua_colegios_asesor", becario.Colegio, becario.VPD);
                Prospecto.ua_colegio_procedencia = _entityReferenceTransformer.GetColegioProcedencia(becario.Colegio);

            }

            if (!string.IsNullOrEmpty(becario.Grado))
                Prospecto.ua_grado = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Lead.EntityLogicalName, "ua_grado", becario.Grado));

            if (becario.Fecha_Nacimiento != null)
            {
                Prospecto.ua_fecha_nacimiento = ResolveFecha(becario.Fecha_Nacimiento);
                Prospecto.ua_fecha_de_nacimiento_flujo = ResolveFecha(becario.Fecha_Nacimiento);
                Prospecto.ua_Edadv2 = GetEdad(Prospecto.ua_fecha_nacimiento.Value);

            }

            if (!string.IsNullOrEmpty(becario.Sexo))
            {
                // var Sexol = becario.Sexo;
                Prospecto.ua_sexo = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Lead.EntityLogicalName, "ua_sexo", becario.Sexo));
            }

            Prospecto.MobilePhone = becario.Telefono_Lada + becario.Telefono_Numero;
            Prospecto.EMailAddress1 = becario.Correo_Electronico;


            if (becario.Nivel != null)
                Prospecto.ua_codigo_nivel = _entityReferenceTransformer.GetNivel(becario.Nivel.ToUpper());
            //Falta campo a tabla de campus
            if (becario.Campus != null)
                Prospecto.ua_codigo_campus = _entityReferenceTransformer.GetCampus(becario.Campus.ToUpper());


            Prospecto.ua_vpd = _entityReferenceTransformer.GetCampus(becario.VPD.ToUpper());
            Prospecto.ua_codigo_vpd2 = Prospecto.ua_vpd;
            //se usa para que el campo caiga en oportunidad
            Prospecto.ua_codigo_vpd = becario.VPD;

            //Obtenes el programa pro campus en bace a el código de carrera que se recibe
            if (!string.IsNullOrWhiteSpace(becario.Codigo) && becario.Campus != null)
            {

                var programabycarrera = RetrieveProgramaByCarreraWeb(RetrivePrograma(becario.Codigo), Prospecto.ua_codigo_campus, out CodigoPrograma, becario.Campus, becario.Codigo);
                //Get progama por campus idm
                Prospecto.ua_programav2 = GetProgramaByVPd(CodigoPrograma, Prospecto.ua_codigo_campus);
                ua_programas_por_campus_asesor co = new ua_programas_por_campus_asesor();

                //Get programa por campus asesor
                Prospecto.ua_programas_por_campus_asesor = GetDatoAsesor(ua_programas_por_campus_asesor.EntityLogicalName, Prospecto.ua_programav2, Prospecto.ua_vpd, "ua_programas_por_campus_asesorid",
                    "ua_programas_por_campus", "ua_codigo_vpd", "ua_programas_por_campus_asesor", CodigoPrograma, becario.VPD);

                Prospecto.ua_Programa = _entityReferenceTransformer.GetPrograma(CodigoPrograma);




            }

            if (!string.IsNullOrWhiteSpace(becario.Pais))
            {

                var entitypais = _entityReferenceTransformer.GetPais(becario.Pais);
                if (entitypais != null)
                    Prospecto.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitypais, Prospecto.ua_vpd,
                         "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", becario.Pais, becario.VPD);

                Prospecto.ua_codigo_pais = entitypais;
            }
            if (!string.IsNullOrWhiteSpace(becario.Estado))
            {
                var entityEstado = _entityReferenceTransformer.GetEstadoSE(becario.Estado.ToUpper());

                if (entityEstado != null)
                    Prospecto.ua_estado_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entityEstado, Prospecto.ua_vpd,
                        "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", becario.Estado, becario.VPD);

                Prospecto.ua_codigo_estado = entityEstado;

            }
            if (!string.IsNullOrWhiteSpace(becario.Municipio))
            {
                var entityDelegacionM = _entityReferenceTransformer.GetMunicipioId(becario.Municipio.ToUpper());
                if (entityDelegacionM != null)
                    Prospecto.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityDelegacionM, Prospecto.ua_vpd,
                        "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", becario.Municipio, becario.VPD);

                Prospecto.ua_codigo_delegacion = entityDelegacionM;
            }


            if (!string.IsNullOrEmpty(becario.Periodo))
                Prospecto.ua_periodo = _entityReferenceTransformer.GetPeriodo(becario.Periodo);


            //Banner =3
            //WEb o CRM = 1
            //Formulario APREU=2
            //if (becario.Origen != null)
            //    Prospecto.ua_origen = new OptionSetValue(int.Parse(becario.Origen));
            Prospecto.ua_suborigen = becario.SubOrigen;
            Prospecto.ua_origen = new OptionSetValue(5);

            Prospecto.ua_asignar_asesor = new OptionSetValue(2);

            //Prospecto.rs_informacioncorrecta = true;
            //Prospecto.ua_AsignarAsesor=


            res = _xrmServerConnection2.Create(Prospecto);



            return res;
        }

        #endregion

        #region 43 Notifica Conexion con el CRM
        public bool NotificaConexionCRM()
        {
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);

            try
            {
                _xrmServerConnection2 = this.service.OrganizationService;
            }
            catch (Exception ex)
            {
                var es = ex.Message;
            }
            return false;

        }
        #endregion


        #region 44 intetegracion nueva para algo

        public bool HaYRegistrosEnCuenta()
        {
            bool result = false;
            Opportunity pr = new Opportunity();

            Guid idCuentaExiste = default(Guid);
            bool bExisteOpor = false;
            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "opportunityid", "name", "ua_periodo", "ua_codigo_vpd" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal,  "00334254"),


                    }
                }
            };
            var Listcuent = _xrmServerConnection.RetrieveMultiple(QueryOportEx);
            if (Listcuent != null)
                if (Listcuent.Entities.Any())
                {
                    var opr = Listcuent.Entities.FirstOrDefault();

                    var perido = ((EntityReference)opr.Attributes["ua_periodo"]).Name;
                    idCuentaExiste = new Guid(opr["opportunityid"].ToString());
                    bExisteOpor = true;

                }

            return result;
        }
        #endregion





        #region 50 ObtenDatosReporte
        public List<RepOportunidades> ObtenerDatosReport()
        {
            List<RepOportunidades> listOp = null;

            Opportunity op = new Opportunity();



            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "name", "ua_idbanner", "ua_codigo_programa", "ua_codigo_vpd" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                TopCount = 5,
                Criteria = {
                    Conditions = {
                       // new ConditionExpression("StatusCode", ConditionOperator.Equal,  CuenteRef.Id),
                        
                    }
                }
            };
            var ListOportRel = _xrmServerConnection.RetrieveMultiple(QueryOportEx);

            if (ListOportRel != null)
                if (ListOportRel.Entities.Any())
                {
                    listOp = new List<RepOportunidades>();
                    RepOportunidades rep = null;
                    foreach (var item in ListOportRel.Entities)
                    {
                        rep = new RepOportunidades();
                        if (item.Attributes.Contains("name"))
                            rep.Nombre = item.Attributes["name"].ToString();
                        if (item.Attributes.Contains("ua_idbanner"))
                            rep.Idbaner = item.Attributes["ua_idbanner"].ToString();
                        if (item.Attributes.Contains("ua_codigo_programa"))
                            rep.Programa = item.Attributes["ua_codigo_programa"].ToString();
                        if (item.Attributes.Contains("ua_codigo_vpd"))
                            rep.Vpd = item.Attributes["ua_codigo_vpd"].ToString();

                        listOp.Add(rep);
                    }




                }
            return listOp;
        }
        #endregion




        #region Utilerias

        public Cuenta GetDatosCuentaByProspecto(Guid leadId)
        {
            Cuenta cuentp = null;





            var prospecto = _xrmServerConnection.Retrieve(Lead.EntityLogicalName, leadId, new ColumnSet { AllColumns = true });
            if (prospecto.Attributes.Any())
            {
                cuentp = new Cuenta();

                //Nacionalidad
                cuentp.Nacionalidad = " ";
                // idbanner
                if (prospecto.Attributes.ContainsKey("ua_id_banner"))
                    cuentp.IdBanner = prospecto.Attributes["ua_id_banner"].ToString();
                //Nombre
                if (prospecto.Attributes.ContainsKey("firstname"))
                    cuentp.Nombre = prospecto.Attributes["firstname"].ToString();
                //Segundo Nombre
                if (prospecto.Attributes.ContainsKey("middlename"))
                    cuentp.Segundo_Nombre = prospecto.Attributes["middlename"].ToString();
                //Apellidos
                if (prospecto.Attributes.ContainsKey("lastname"))
                    cuentp.Apellidos = prospecto.Attributes["lastname"].ToString();
                //Fecha de nacimiento
                if (prospecto.Attributes.ContainsKey("ua_fecha_nacimiento"))
                {
                    DateTime FN = ((DateTime)prospecto.Attributes["ua_fecha_nacimiento"]).Date;
                    cuentp.Fecha_de_nacimiento = new CustomDate { Year = FN.Year, Month = FN.Month, Day = FN.Day };
                }
                //Correo
                if (prospecto.Attributes.ContainsKey("emailaddress1"))
                {
                    List<Correo> listC = new List<Correo>();
                    listC.Add(new Correo { Correo_Electronico = prospecto.Attributes["emailaddress1"].ToString(), TipoCorreoElectronicoId = "PERS" });

                    cuentp.Correos = listC;
                }
                //Sexo
                if (prospecto.Attributes.ContainsKey("ua_sexo"))
                {
                    var Sexol = ((OptionSetValue)prospecto.Attributes["ua_sexo"]).Value;
                    cuentp.Sexo = Sexol == 2 ? "M" : "F";
                }
                else
                    throw new Exception("El prospecto no tiene sexo");

                //Religion
                if (prospecto.Attributes.ContainsKey("ua_religion"))
                    cuentp.ReligionId = prospecto.Attributes["ua_religion"].ToString();
                else
                    throw new Exception(" el prospecto no tiene religion");
                //Colegio procedencia

                if (prospecto.Attributes.ContainsKey("ua_colegio_procedencia"))
                {
                    var colpro = ((EntityReference)prospecto.Attributes["ua_colegio_procedencia"]).Id.ToString();
                    Guid idcol = new Guid(colpro);
                    var res = _xrmServerConnection.Retrieve(ua_colegios.EntityLogicalName, idcol, new ColumnSet(new string[] { "ua_codigo_colegio" }));
                    if (res.Attributes.Any())
                        cuentp.Colegio_Procedencia = res.Attributes["ua_codigo_colegio"].ToString();
                    else
                        throw new Exception("El prospecto no tiene colegio de procedencia");


                }
                else
                    throw new Exception("El prospecto no tienee colegio de procedencia");

                //Promedio
                if (prospecto.Attributes.ContainsKey("ua_promedio"))
                    cuentp.Promedio = prospecto.Attributes["ua_promedio"].ToString();
                else
                    throw new Exception("El prospecto no tiene promedio");


            }



            return cuentp;

        }

        private bool SetStageSolicitanteIdOpotunidad(string POportunidadID, out string snombrereturn)
        {
            bool PasarEtapaSolicitante = true;
            snombrereturn = "";
            // string op = GetidProspectoByName("", "","00328085");
            string idopr = "";
            Guid idop = new Guid(POportunidadID);
            var opor = _xrmServerConnection.Retrieve(Opportunity.EntityLogicalName, idop, new ColumnSet { AllColumns = true });
            if (opor != null)
            {
                //if (opor.Attributes.Contains("StageId"))
                //    stageid = opor.Attributes["stageid"].ToString();
                if (opor.Attributes.Contains("opportunityid"))
                    idopr = opor.Attributes["opportunityid"].ToString();
                if (opor.Attributes.Contains("stepname"))
                {
                    snombrereturn = opor.Attributes["stepname"].ToString();
                    PasarEtapaSolicitante = false;
                }

            }



            return PasarEtapaSolicitante;
        }

        private string FormarCorreoProspecto(string correoGuardado, string guidOportunidad)
        {
            string CorreoRetornar = "";
            var GuidConguienbajo = guidOportunidad.Replace('-', '_');
            CorreoRetornar = GuidConguienbajo + "_" + correoGuardado;
            if (CorreoRetornar.Length > 128)
                CorreoRetornar = GuidConguienbajo + "@anahuac.mx";

            return CorreoRetornar;


        }

        private void ActualizarFaseCono(string stage, string proceso, Guid idOportunidad)
        {
            var bpm = new BusinessProcessManager();
            try
            {
                bpm.UpdateStage(proceso, stage, Opportunity.EntityLogicalName, idOportunidad);
            }
            catch (Exception ex)
            {
                throw new CRMException(String.Format("ha ocurrido un error al actualizar el stage: {0}", ex.ToString()));
            }
        }
        private void ActualizarFaseCono2(Guid idOportunidad, string Origen)
        {
            var bpm = new BusinessProcessManager();
            try
            {
                bpm.UpdateStage2(idOportunidad, Origen);
            }
            catch (Exception ex)
            {
                throw new CRMException(String.Format("Ocurrió sl siguiente error en el proceso de Avance de Fase V2: {0}", ex.ToString()));
            }
        }
        private void CorreccionFases(Guid idOportunidad, string Origen)
        {
            var bpm = new BusinessProcessManager();
            try
            {
                bpm.correccionFasesProcesos(idOportunidad, Origen);
            }
            catch (Exception ex)
            {
                throw new CRMException(String.Format("Ocurrió sl siguiente error en el proceso de Avance de Fase V2: {0}", ex.ToString()));
            }
        }
        private string ObtenerFaseActual(Guid idOportunidad, string Origen)
        {
            var bpm = new BusinessProcessManager();
            try
            {
                return bpm.ObtenerFaseReal(idOportunidad, Origen);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        //private void ActualizarFaseProspecto(string stage, string proceso, Guid idProspecto)
        //{
        //    var bpm = new BusinessProcessManager();
        //    //string stage = "Solicitante";
        //    //string proceso = "Proceso de cliente potencial a ventas de la oportunidad";
        //    try
        //    {
        //        //Guid IdOportunidad = (Guid)idOportunidadRegresar;
        //        bpm.UpdateStage(proceso, stage, Lead.EntityLogicalName, idProspecto);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new CRMException(String.Format("ha ocurrido un error al actualizar el stage: {0}", ex.ToString()));
        //    }
        //}
        private bool ExisteCuenta(string pIdbanner, out Guid idCuentaExiste)
        {
            Account cuent = new Account();

            idCuentaExiste = default(Guid);
            bool bExisteOpor = false;
            QueryExpression QueryOportEx = new QueryExpression(Account.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "accountid" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal,  pIdbanner),

                    }
                }
            };
            var Listcuent = _xrmServerConnection.RetrieveMultiple(QueryOportEx);
            if (Listcuent != null)
                if (Listcuent.Entities.Any())
                {
                    var opr = Listcuent.Entities.FirstOrDefault();
                    idCuentaExiste = new Guid(opr["accountid"].ToString());
                    bExisteOpor = true;

                }


            return bExisteOpor;
        }

        private string GetDescripcionNacionalidad(string idnacionaldiad)
        {
            //ua_desc_nacionalidad b = new ua_desc_nacionalidad();

            string retn = "";
            //EntityReference refnacioi = new EntityReference(ua_desc_nacionalidad.EntityLogicalName, new Guid(idnacionaldiad));
            Guid idNacGuid = new Guid(idnacionaldiad);
            var descn = _xrmServerConnection.Retrieve(ua_desc_nacionalidad.EntityLogicalName, idNacGuid, new ColumnSet(new string[] { "ua_desc_nacionalidad", "ua_codigo_nacionalidad" }));
            if (descn != null)
            {
                retn = descn.Attributes["ua_codigo_nacionalidad"].ToString();
            }

            return retn;

        }

        private bool ExistenFechasExamen(Guid oportunidadID, string col1, string col2, string col3)
        {
            bool result = false;

            ColumnSet col = null;
            if (!string.IsNullOrEmpty(col3))
                col = new ColumnSet(new string[] { col1, col2, col3 });
            else if (!string.IsNullOrEmpty(col2))
                col = new ColumnSet(new string[] { col1, col2 });
            else if (string.IsNullOrWhiteSpace(col2))
                col = new ColumnSet(new string[] { col1 });

            Opportunity op = new Opportunity();

            //if (op.ua_fecha_paan != null && op.ua_fecha_para != null && op.ua_fecha_paav != null)

            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = col,
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("opportunityid", ConditionOperator.Equal,  oportunidadID),

                    }
                }
            };
            var Listcuent = _xrmServerConnection.RetrieveMultiple(QueryOportEx);
            if (Listcuent != null)
                if (Listcuent.Entities.Any())
                {
                    var opr = Listcuent.Entities.FirstOrDefault();

                    if (!string.IsNullOrEmpty(col3))//si biene la 3 indica que todas las columnas no bienen nullas
                    {
                        if (opr.Attributes.Contains(col1) && opr.Attributes.Contains(col2) && opr.Attributes.Contains(col3))
                            result = true;
                    }
                    else if (!string.IsNullOrEmpty(col2))//si biene la 2 
                    {
                        if (opr.Attributes.Contains(col1) && opr.Attributes.Contains(col2))
                            result = true;
                    }
                    else if (opr.Attributes.Contains(col1))
                        result = true;

                }
            return result;

        }

        private EntityReference GetVPDOfAccount(string idCuenta, out string DescVPd)
        {
            EntityReference vpdreturn = null;
            DescVPd = "";
            //EntityReference refnacioi = new EntityReference(ua_desc_nacionalidad.EntityLogicalName, new Guid(idnacionaldiad));
            Guid idCuentaGuid = new Guid(idCuenta);
            var vpdcuenta = _xrmServerConnection.Retrieve(Account.EntityLogicalName, idCuentaGuid, new ColumnSet(new string[] { "ua_vpd" }));
            if (vpdcuenta != null)
            {
                if (vpdcuenta.Attributes.Contains("ua_vpd"))
                {
                    vpdreturn = (EntityReference)vpdcuenta.Attributes["ua_vpd"];

                    var vpddes = _xrmServerConnection.Retrieve(BusinessUnit.EntityLogicalName, vpdreturn.Id, new ColumnSet { AllColumns = true });
                    if (vpddes != null)
                    {
                        DescVPd = vpddes.Attributes["name"].ToString();
                    }

                }
            }

            return vpdreturn;

        }

        private Guid CrearCuenta(Cuenta pCuenta, ResponseNewProspect respuestaIntegracion, EntityReference VPDp, string codigocolegiop)
        {

            // crear una cuenta solo se ocupa el nombre
            //Entity account = new Entity("account");
            //account["name"] = pCuenta.Cuenta_Nombre;
            Account cuenta = new Account();

            cuenta.Name = pCuenta.Nombre + "  " + pCuenta.Segundo_Nombre + "  " + SplitApellidoPaterno(pCuenta.Apellidos) + " " + SplitApellidoMaterno(pCuenta.Apellidos);
            cuenta.ua_nombre_completo = string.Format("{0} {1} {2} {3}", pCuenta.Nombre, pCuenta.Segundo_Nombre, SplitApellidoPaterno(pCuenta.Apellidos), SplitApellidoMaterno(pCuenta.Apellidos));
            cuenta.ua_idbanner = pCuenta.IdBanner;
            if (pCuenta.Correos != null)
                cuenta.EMailAddress1 = pCuenta.Correos.Where(x => x.TipoCorreoElectronicoId == "PERS").FirstOrDefault().Correo_Electronico;
            if (pCuenta.Fecha_de_nacimiento != null)
                cuenta.ua_fecha_nacimiento = ResolveFecha(pCuenta.Fecha_de_nacimiento);
            if (pCuenta.Sexo != null)
                cuenta.ua_sexo = new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Account.EntityLogicalName, "ua_sexo", pCuenta.Sexo));

            if (string.IsNullOrWhiteSpace(pCuenta.Nacionalidad)) respuestaIntegracion.Warnings.Add("la nacionalidad no puede ir vacia ");
            else
                cuenta.ua_desc_nacionalidad = _entityReferenceTransformer.GetNacionalidad(pCuenta.Nacionalidad);
            if (cuenta.ua_desc_nacionalidad == null) respuestaIntegracion.Warnings.Add("la nacionalidad " + pCuenta.Nacionalidad + "no se encontro en el catalogo ");

            cuenta.ua_VPD = _entityReferenceTransformer.GetCampus(pCuenta.CampusVPD);

            if (string.IsNullOrWhiteSpace(pCuenta.ReligionId)) respuestaIntegracion.Warnings.Add("la religion no puede ir vacia");
            else
                cuenta.ua_religion = _entityReferenceTransformer.GetReligion(pCuenta.ReligionId);

            if (string.IsNullOrWhiteSpace(pCuenta.Colegio_Procedencia)) respuestaIntegracion.Warnings.Add("el colegio de procedencia no puede ir vacia");
            else
                cuenta.ua_colegio_asesor = new EntityReference(ua_colegios_asesor.EntityLogicalName, new Guid(pCuenta.Colegio_Procedencia));

            //Colegio procedencia normalito
            var entitycolegiop = _entityReferenceTransformer.GetColegioProcedencia(codigocolegiop);
            if (entitycolegiop != null)
            {
                cuenta.ua_colegioguidstr = entitycolegiop.Id.ToString();
                cuenta.ua_colegio_procedencia = entitycolegiop;
            }


            cuenta.ua_promedio = pCuenta.Promedio;

            cuenta.ua_origen = new OptionSetValue(3);


            #region Pais,Estado y municipio

            //Direccion itemDir = pCuenta.Direcciones.Where(x => x.TipoDireccionId == "PR").FirstOrDefault();

            //if (itemDir != null)
            //{


            //    #region Direccion principal



            //    if (!string.IsNullOrWhiteSpace(itemDir.PaisId))
            //    {
            //        var entitiPais = _entityReferenceTransformer.GetPais(itemDir.PaisId);
            //        if (entitiPais != null)
            //            cuenta.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitiPais, VPDp,
            //             "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", itemDir.PaisId, pCuenta.CampusVPD);
            //    }
            //    if (cuenta.ua_pais_asesor == null)
            //        respuestaIntegracion.Warnings.Add(" el Pais " + itemDir.PaisId + " no fue encontrado en el catalogo para asignar asesor");

            //    //c.ua_Colonia_Extranjera1 = itemDir.Colonia;

            //    if (!string.IsNullOrWhiteSpace(itemDir.Estado))
            //    {
            //        var entitiestado = _entityReferenceTransformer.GetEstadoSE(itemDir.Estado);
            //        if (entitiestado != null)
            //            cuenta.ua_estado_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entitiestado, VPDp,
            //                                      "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", itemDir.Estado, pCuenta.CampusVPD);

            //    }
            //    if (cuenta.ua_estado_asesor == null) respuestaIntegracion.Warnings.Add(" el Estado " + itemDir.Estado + " no fue encontrado en el catalogo para asignar asesor");


            //    if (!string.IsNullOrWhiteSpace(itemDir.DelegacionMunicipioId))
            //    {
            //        var entityMunicipio = _entityReferenceTransformer.GetMunicipioId(itemDir.DelegacionMunicipioId);
            //        if (entityMunicipio != null)
            //            cuenta.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityMunicipio, VPDp,
            //                                         "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", itemDir.DelegacionMunicipioId, pCuenta.CampusVPD);
            //    }
            //    if (cuenta.ua_delegacion_municipio_asesor == null) respuestaIntegracion.Warnings.Add(" el Municipio " + itemDir.DelegacionMunicipioId + " no fue encontrado en el catalogo para asignar asesor");


            //    #endregion
            //    //}

            //    //}




            //}//fin del if direcciones personales
            //else
            //    respuestaIntegracion.Warnings.Add(" No se guardo Pais,Estado y municipio para cuentas");


            #endregion






            Guid idcuentaCreada = _xrmServerConnection.Create(cuenta);
            return idcuentaCreada;
        }

        private bool ConsultarCuenta(Guid pidCuenta)
        {
            bool bExisteCuenta = false;
            //Consultar cuenta creada
            Entity cuentaConsu = new Entity("account");
            ColumnSet attributes = new ColumnSet(new string[] { "name", "ownerid", "creditlimit" });
            cuentaConsu = _xrmServerConnection.Retrieve(cuentaConsu.LogicalName, pidCuenta, attributes);
            if (cuentaConsu != null)
            {
                if (cuentaConsu.Attributes.Count > 0)
                    bExisteCuenta = true;

            }

            return bExisteCuenta;
        }

        private Guid CrearContacto(Cuenta pContacto, Guid pIdcuenta, Dictionary<Guid, string> pRegistrosCreados, ResponseNewProspect respuestaIntegracion)
        {

            #region Mapeamos los campos para el contacto principal de la cuenta
            //crear nuevo contacto a una cuenta
            Contact c = new Contact();
            //Campo para referencia a la cuenta que pertence este contacto
            //c.ParentCustomerId = new EntityReference(Account.EntityLogicalName, pIdcuenta);

            //Mapeamos los campos
            var vpdentity = _entityReferenceTransformer.GetCampus(pContacto.CampusVPD);

            c.FirstName = pContacto.Nombre;
            c.MiddleName = pContacto.Segundo_Nombre;
            c.LastName = SplitApellidoPaterno(pContacto.Apellidos) + " " + SplitApellidoMaterno(pContacto.Apellidos);
            c.ua_idbanner = pContacto.IdBanner;
            if (pContacto.ReligionId != null)
                c.ua_desc_religion = GetIdReferencia(ua_religion.EntityLogicalName, "ua_religionid", "ua_codigo_religion", pContacto.ReligionId);

            if (pContacto.EstadoCivil != null)
                c.ua_desc_estado_civil = GetIdReferencia(ua_desc_estado_civil.EntityLogicalName, "ua_desc_estado_civilid", "ua_codigo_estado_civil", pContacto.EstadoCivil);

            c.GenderCode = !string.IsNullOrEmpty(pContacto.Sexo.Trim()) ? new OptionSetValue(_entityReferenceTransformer.GetConguntoOpsiones(Contact.EntityLogicalName, "gendercode", pContacto.Sexo)) : null;
            c.ua_desc_Nacionalidad = !string.IsNullOrEmpty(pContacto.Nacionalidad.Trim()) ? _entityReferenceTransformer.GetNacionalidad(pContacto.Nacionalidad) : null;
            c.BirthDate = ResolveFecha(pContacto.Fecha_de_nacimiento);
            c.ua_origen = new OptionSetValue(3);

            //Mapeamos la direccion principal del contacto principal

            if (pContacto.Direcciones != null)
            {
                int cont = 1;
                foreach (var itemDir in pContacto.Direcciones)
                {
                    if (itemDir.TipoDireccionId == "PR")
                    {

                        #region Direccion principal

                        //Direccion dirConPrin  = pContacto.Direcciones.Where(x => x.TipoDireccionId == "PR").FirstOrDefault();

                        c.ua_tipo_direccion1 = GetIdReferencia(ua_tipo_direccion.EntityLogicalName, "ua_tipo_direccionid", "ua_codigo_tipo_direccion", itemDir.TipoDireccionId);
                        //c.Address1_Name = "Principal";
                        c.Address1_Line1 = itemDir.Calle;
                        c.Address1_Line2 = itemDir.Numero;

                        if (itemDir.PaisId != null)
                        {
                            var entitypais = _entityReferenceTransformer.GetPais(itemDir.PaisId);
                            if (entitypais != null)
                                c.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitypais, vpdentity,
                                  "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", itemDir.PaisId, pContacto.CampusVPD);

                            c.ua_codigo_pais = entitypais;
                        }
                        if (c.ua_pais_asesor == null)
                            respuestaIntegracion.Warnings.Add(" el Pais " + itemDir.PaisId + " no fue encontrado en el catalogo");

                        //c.ua_Colonia_Extranjera1 = itemDir.Colonia;

                        if (itemDir.Estado != null)
                        {
                            var entityestado = _entityReferenceTransformer.GetEstadoSE(itemDir.Estado);
                            if (entityestado != null)
                                c.ua_estados_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entityestado, vpdentity,
                            "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", itemDir.Estado, pContacto.CampusVPD);

                            c.ua_codigo_estado = entityestado;

                        }

                        if (c.ua_estados_asesor == null) respuestaIntegracion.Warnings.Add(" el Estado " + itemDir.Estado + " no fue encontrado en el catalogo");

                        c.Address1_PostalCode = itemDir.CodigoPostal;

                        if (itemDir.DelegacionMunicipioId != null)
                        {
                            var entityDelegacionM = _entityReferenceTransformer.GetMunicipioId(itemDir.DelegacionMunicipioId);
                            if (entityDelegacionM != null)
                                c.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityDelegacionM, vpdentity,
                          "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", itemDir.DelegacionMunicipioId, pContacto.CampusVPD);

                            c.ua_codigo_delegacion = entityDelegacionM;

                        }
                        if (c.ua_delegacion_municipio_asesor == null) respuestaIntegracion.Warnings.Add(" el Municipio " + itemDir.DelegacionMunicipioId + " no fue encontrado en el catalogo");

                        if (itemDir.Colonia != null && itemDir.CodigoPostal != null && c.ua_estados_asesor != null && c.ua_pais_asesor != null)
                        {
                            Domicilio dom = new Domicilio
                            {
                                CP = itemDir.CodigoPostal,
                                Colonia = itemDir.Colonia,
                                Estado = itemDir.Estado,
                                Pais = itemDir.PaisId,
                                Municipio = itemDir.DelegacionMunicipioId
                            };

                            c.ua_colonia_1 = _entityReferenceTransformer.GetColonia(dom);
                            if (c.ua_colonia_1 == null) respuestaIntegracion.Warnings.Add(" la Colonia " + itemDir.Colonia + " no fue encontrado en el catalogo");
                        }
                        #endregion
                    }
                    else
                    {
                        if (cont == 2)
                        {
                            #region Direccion2



                            c.ua_tipo_direccion2 = GetIdReferencia(ua_tipo_direccion.EntityLogicalName, "ua_tipo_direccionid", "ua_codigo_tipo_direccion", itemDir.TipoDireccionId);
                            //c.Address1_Name = "Principal";
                            c.Address2_Line1 = itemDir.Calle;
                            c.Address2_Line2 = itemDir.Numero;

                            if (itemDir.PaisId != null)
                                c.ua_pais_direccion2 = _entityReferenceTransformer.GetPais(itemDir.PaisId);
                            if (c.ua_pais_direccion2 == null)
                                respuestaIntegracion.Warnings.Add(" el Pais " + itemDir.PaisId + " no fue encontrado en el catalogo");

                            c.ua_colonia_extranjera2 = itemDir.Colonia;

                            if (itemDir.Estado != null)
                                c.ua_estado_direccion2 = _entityReferenceTransformer.GetEstadoSE(itemDir.Estado);
                            if (c.ua_estado_direccion2 == null) respuestaIntegracion.Warnings.Add(" el Estado " + itemDir.Estado + " no fue encontrado en el catalogo");

                            c.Address2_PostalCode = itemDir.CodigoPostal;

                            if (itemDir.DelegacionMunicipioId != null)
                                c.ua_delegacion_direccion2 = _entityReferenceTransformer.GetMunicipioId(itemDir.DelegacionMunicipioId);
                            if (c.ua_delegacion_direccion2 == null) respuestaIntegracion.Warnings.Add(" el Municipio " + itemDir.DelegacionMunicipioId + " no fue encontrado en el catalogo");

                            if (itemDir.Colonia != null && itemDir.CodigoPostal != null && itemDir.Estado != null && itemDir.PaisId != null)
                            {
                                Domicilio dom = new Domicilio
                                {
                                    CP = itemDir.CodigoPostal,
                                    Colonia = itemDir.Colonia,
                                    Estado = itemDir.Estado,
                                    Pais = itemDir.PaisId,
                                    Municipio = itemDir.DelegacionMunicipioId
                                };
                                c.ua_colonia_2 = _entityReferenceTransformer.GetColonia(dom);
                                if (c.ua_colonia_1 == null) respuestaIntegracion.Warnings.Add(" la Colonia " + itemDir.Colonia + " no fue encontrado en el catalogo");
                            }
                            #endregion
                        }
                        else
                        {

                            #region Direccion3



                            c.ua_tipo_direccion3 = GetIdReferencia(ua_tipo_direccion.EntityLogicalName, "ua_tipo_direccionid", "ua_codigo_tipo_direccion", itemDir.TipoDireccionId);
                            //c.Address1_Name = "Principal";
                            c.Address3_Line1 = itemDir.Calle;
                            c.Address3_Line2 = itemDir.Numero;

                            if (itemDir.PaisId != null)
                                c.ua_pais_direccion3 = _entityReferenceTransformer.GetPais(itemDir.PaisId);
                            if (c.ua_pais_direccion3 == null)
                                respuestaIntegracion.Warnings.Add(" el Pais " + itemDir.PaisId + " no fue encontrado en el catalogo");

                            c.ua_colonia_extranjera3 = itemDir.Colonia;

                            if (itemDir.Estado != null)
                                c.ua_estado_direccion3 = _entityReferenceTransformer.GetEstadoSE(itemDir.Estado);
                            if (c.ua_estado_direccion3 == null) respuestaIntegracion.Warnings.Add(" el Estado " + itemDir.Estado + " no fue encontrado en el catalogo");

                            c.Address3_PostalCode = itemDir.CodigoPostal;

                            if (itemDir.DelegacionMunicipioId != null)
                                c.ua_delegacion_direccion3 = _entityReferenceTransformer.GetMunicipioId(itemDir.DelegacionMunicipioId);
                            if (c.ua_delegacion_direccion3 == null) respuestaIntegracion.Warnings.Add(" el Municipio " + itemDir.DelegacionMunicipioId + " no fue encontrado en el catalogo");

                            if (itemDir.Colonia != null && itemDir.CodigoPostal != null && itemDir.Estado != null && itemDir.PaisId != null)
                            {
                                Domicilio dom = new Domicilio
                                {
                                    CP = itemDir.CodigoPostal,
                                    Colonia = itemDir.Colonia,
                                    Estado = itemDir.Estado,
                                    Pais = itemDir.PaisId,
                                    Municipio = itemDir.DelegacionMunicipioId
                                };
                                c.ua_colonia_3 = _entityReferenceTransformer.GetColonia(dom);
                                if (c.ua_colonia_3 == null) respuestaIntegracion.Warnings.Add(" la Colonia " + itemDir.Colonia + " no fue encontrado en el catalogo");
                            }
                            #endregion
                        }
                    }

                    cont++;
                }




            }//fin del if direcciones personales

            #region Telefonos

            string Lada = "";
            //Telefono principal lada
            if (pContacto.Telefonos.Where(t => t.TipoTelefono == "PR").FirstOrDefault() != null)
                c.ua_lada_telefono = pContacto.Telefonos.Where(t => t.TipoTelefono == "PR").FirstOrDefault().LadaTelefono;
            //Telefono principal numero
            if (pContacto.Telefonos.Where(t => t.TipoTelefono == "PR").FirstOrDefault() != null)
                c.Telephone1 = pContacto.Telefonos.Where(t => t.TipoTelefono == "PR").FirstOrDefault().Telefono1;
            //Telefono movil
            //c.ua_lada_telefono = pContacto.Telefonos.Where(t => t.TipoTelefono == "PR").FirstOrDefault().LadaTelefono;
            if (pContacto.Telefonos.Where(t => t.TipoTelefono == "CE").FirstOrDefault() != null)
            {
                Lada = pContacto.Telefonos.Where(t => t.TipoTelefono == "CE").FirstOrDefault().LadaTelefono;
                //c.Telephone2 = pContacto.Telefonos.Where(t => t.TipoTelefono == "CE").FirstOrDefault().Telefono1;
                c.MobilePhone = pContacto.Telefonos.Where(t => t.TipoTelefono == "CE").FirstOrDefault().Telefono1;
            }
            //Otro telefono
            if (pContacto.Telefonos.Where(t => t.TipoTelefono == "BU").FirstOrDefault() != null)
            {
                Lada = pContacto.Telefonos.Where(t => t.TipoTelefono == "BU").FirstOrDefault().LadaTelefono;
                c.Telephone2 = pContacto.Telefonos.Where(t => t.TipoTelefono == "BU").FirstOrDefault().Telefono1;
            }
            //Crearle telefonos al contacto pricipal
            //var ListP = pContacto.Telefonos.Where(t => t.TipoTelefono == dirConPrin.TipoDireccionId).ToList();
            if (Lada != null)
            {
                //    int cont = 0;
                //    foreach (var itemtele in ListP)
                //    {

                //        c.ua_lada_telefono = itemtele.LadaTelefono;
                //        if (itemtele.PreferidoTelefono == "Y")
                //            c.Telephone1 = itemtele.Telefono1;
                //        else
                //            if (cont < ListP.Count)
                //            c.Telephone2 = itemtele.Telefono1;
                //        else
                //            c.Telephone3 = itemtele.Telefono1;
                //        cont++;

                //    }
            }

            #endregion

            var tipoCorreoP = pContacto.Correos.Where(co => co.TipoCorreoElectronicoId.ToUpper() == "PERS").FirstOrDefault();
            if (tipoCorreoP != null)
            {
                c.EMailAddress1 = tipoCorreoP.Correo_Electronico;
            }
            var correall = pContacto.Correos;
            if (correall != null)
            {
                if (correall.Count == 2)
                    c.EMailAddress2 = correall[1].Correo_Electronico;
                if (correall.Count == 3)
                {
                    c.EMailAddress2 = correall[1].Correo_Electronico;
                    c.EMailAddress3 = correall[2].Correo_Electronico;
                }
            }

            //Asociamos la cuenta a este contacto principal
            c.ParentCustomerId = new EntityReference(Account.EntityLogicalName, pIdcuenta);
            // = new EntityReference(Account.EntityLogicalName, pIdcuenta);
            //Guardamos el contacto principal

            Guid ContactoPrincipalId = _xrmServerConnection.Create(c);
            pRegistrosCreados.Add(ContactoPrincipalId, Contact.EntityLogicalName);

            //Actualizamos la cuenta asociandole un  contacto principal
            Account CuentacontactoPrincipal = new Account();
            CuentacontactoPrincipal.PrimaryContactId = new EntityReference(Contact.EntityLogicalName, ContactoPrincipalId);
            CuentacontactoPrincipal.AccountId = pIdcuenta;
            _xrmServerConnection.Update(CuentacontactoPrincipal);

            #endregion


            #region Creamos n contactos tantos parentescos nos llegen

            Contact contact = null;
            // ua_tipo_direccion d = new ua_tipo_direccion();
            if (pContacto.PadreoTutor != null)
            {
                foreach (var itemParent in pContacto.PadreoTutor)
                {
                    contact = new Contact();
                    //Campo para referencia a la cuenta que pertence este contacto
                    contact.ParentCustomerId = new EntityReference(Account.EntityLogicalName, pIdcuenta);
                    contact.FirstName = itemParent.FirstName;
                    contact.MiddleName = itemParent.MiddleName;
                    contact.LastName = SplitApellidoPaterno(itemParent.LastName) + " " + SplitApellidoMaterno(itemParent.LastName);
                    contact.ua_Fallecio = RetroveFallecio(itemParent.Vive);
                    contact.ua_idbanner = pContacto.IdBanner;
                    contact.ua_origen = new OptionSetValue(3);
                    var dir = itemParent.Direcciones;
                    if (dir != null)
                    {


                        contact.Address1_Line1 = dir.Calle;
                        contact.Address1_Line2 = dir.Numero;
                        contact.Address1_PostalCode = dir.CodigoPostal;


                        if (!string.IsNullOrEmpty(dir.PaisId))
                        {
                            var entitypais = _entityReferenceTransformer.GetPais(dir.PaisId);

                            if (entitypais != null)
                                contact.ua_pais_asesor = GetDatoAsesor(ua_pais_asesor.EntityLogicalName, entitypais, vpdentity,
                                  "ua_pais_asesorid", "ua_pais", "ua_codigo_vpd", "ua_paises_asesor", dir.PaisId, pContacto.CampusVPD);
                            contact.ua_codigo_pais = entitypais;

                        }
                        if (contact.ua_pais_asesor == null) respuestaIntegracion.Warnings.Add(" el Pais " + dir.PaisId + " no fue encontrado en el catalogo");

                        if (dir.Estado != null)
                        {
                            //contact.ua_codigo_estado = _entityReferenceTransformer.GetEstadoSE(dir.Estado);
                            var entityestado = _entityReferenceTransformer.GetEstadoSE(dir.Estado);
                            if (entityestado != null)
                                contact.ua_estados_asesor = GetDatoAsesor(ua_estados_asesor.EntityLogicalName, entityestado, vpdentity,
                            "ua_estados_asesorid", "ua_estados", "ua_codigo_vpd", "ua_estados_asesor", dir.Estado, pContacto.CampusVPD);

                            contact.ua_codigo_estado = entityestado;

                        }
                        if (contact.ua_estados_asesor == null) respuestaIntegracion.Warnings.Add(" el Estado " + dir.Estado + " no fue encontrado en el catalogo");


                        if (!string.IsNullOrWhiteSpace(dir.DelegacionMunicipioId))
                        {
                            var entityDelegacionM = _entityReferenceTransformer.GetMunicipioId(dir.DelegacionMunicipioId);
                            if (entityDelegacionM != null)
                                contact.ua_delegacion_municipio_asesor = GetDatoAsesor(ua_delegacion_municipio_asesor.EntityLogicalName, entityDelegacionM, vpdentity,
                          "ua_delegacion_municipio_asesorid", "ua_delegacion_municipio", "ua_codigo_vpd", "ua_delegacion_municipio_asesor", dir.DelegacionMunicipioId, pContacto.CampusVPD);

                            contact.ua_codigo_delegacion = entityDelegacionM;

                        }
                        if (contact.ua_delegacion_municipio_asesor == null) respuestaIntegracion.Warnings.Add(" el Municipio " + dir.DelegacionMunicipioId + " no fue encontrado en el catalogo");


                        if (dir.Colonia != null && dir.CodigoPostal != null && contact.ua_estados_asesor != null && contact.ua_delegacion_municipio_asesor != null)
                        {
                            Domicilio dom1 = new Domicilio
                            {
                                CP = dir.CodigoPostal,
                                Colonia = dir.Colonia,
                                Estado = dir.Estado,
                                Pais = dir.PaisId,
                                Municipio = dir.DelegacionMunicipioId
                            };

                            contact.ua_colonia_1 = _entityReferenceTransformer.GetColonia(dom1);
                            if (contact.ua_colonia_1 == null)
                            {
                                respuestaIntegracion.Warnings.Add("la colonia " + dir.Colonia + " no fue encontrado en el catalogo");
                                //contact.ua_Colonia_Extranjera1 = dir.Colonia;

                            }
                        }

                        contact.ua_codigo_postal = dir.CodigoPostal;


                        if (itemParent.Parentesco != null)
                            contact.ua_tipo_parentesco_cuenta = GetIdReferencia(ua_tipo_parentesco.EntityLogicalName, "ua_tipo_parentescoid", "ua_codigo_tipo_parentesco", itemParent.Parentesco);
                        if (dir.TipoDireccionId != null)
                            contact.ua_tipo_direccion1 = GetIdReferencia(ua_tipo_direccion.EntityLogicalName, "ua_tipo_direccionid", "ua_codigo_tipo_direccion", dir.TipoDireccionId);





                    }

                    //contact.ParentCustomerId =new EntityReference(Account.EntityLogicalName, pIdcuenta);
                    Guid idconCreado = _xrmServerConnection.Create(contact);
                    //Actualizamos la cuenta asociandole un  contacto 

                    //Agregamos la list de contactos creados
                    pRegistrosCreados.Add(idconCreado, Contact.EntityLogicalName);

                }
            }

            #endregion

            return ContactoPrincipalId;
        }

        private bool RetroveFallecio(string svalor)
        {
            //  return (svalor.ToUpper() == "Y") ? true : false;

            return string.IsNullOrWhiteSpace(svalor) ? false : true;
        }

        /// <summary>
        /// Valida si la oportunidad exis
        /// </summary>
        /// <param name="pVPDI"> </param>
        /// <param name="pPrograma"></param>
        /// <returns></returns>
        private bool HayOportunidadRelacionadaAcuenta(Guid pidCuentga)
        {
            EntityReference CuenteRef = new EntityReference(Account.EntityLogicalName, pidCuentga);
            //  Opportunity p = new Opportunity();

            bool bExisteOpor = false;
            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                //ColumnSet = new ColumnSet(new string[] { "Opportunityid" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("accountid", ConditionOperator.Equal,  CuenteRef.Id),
                        //new ConditionExpression("ua_numero_solicitud", ConditionOperator.Equal,  pNumSulicitud),
                        //new ConditionExpression("ua_periodo", ConditionOperator.Equal,  pPeriodo),
                       
                        // new ConditionExpression("ua_codigo_vpd", ConditionOperator.NotEqual, pVPDI.Id)
                    }
                }
            };
            var ListOportRel = _xrmServerConnection.RetrieveMultiple(QueryOportEx);

            if (ListOportRel != null)
                if (ListOportRel.Entities.Any())
                {
                    //var opr = ListOportRel.Entities.FirstOrDefault();
                    //idOporExiste = new Guid(opr["opportunityid"].ToString());
                    bExisteOpor = true;

                }


            return bExisteOpor;
        }

        private Guid GetProspectoByOportunidad(Guid pidCuentga)
        {
            EntityReference CuenteRef = new EntityReference(Account.EntityLogicalName, pidCuentga);
            Opportunity p = new Opportunity();
            Guid ProspectoRetorn = default(Guid);
            bool bExisteOpor = false;
            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "originatingleadid" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("accountid", ConditionOperator.Equal,  CuenteRef.Id),
                        //new ConditionExpression("ua_numero_solicitud", ConditionOperator.Equal,  pNumSulicitud),
                        //new ConditionExpression("ua_periodo", ConditionOperator.Equal,  pPeriodo),
                        
                        // new ConditionExpression("ua_codigo_vpd", ConditionOperator.NotEqual, pVPDI.Id)
                    }
                }
            };
            var ListOportRel = _xrmServerConnection.RetrieveMultiple(QueryOportEx);

            if (ListOportRel != null)
                if (ListOportRel.Entities.Any())
                {
                    var opr = ListOportRel.Entities.FirstOrDefault();
                    var ent = ((EntityReference)opr["originatingleadid"]).Id.ToString();
                    if (!string.IsNullOrEmpty(ent))
                        ProspectoRetorn = new Guid(ent);
                    //idOporExiste = new Guid(opr["opportunityid"].ToString());
                    bExisteOpor = true;

                }


            return ProspectoRetorn;
        }

        private bool ExisteOportunidad31(string pVPDI, string pIdbanner, string pNumSulicitud, Guid pPeriodo, out Guid idOporExiste)
        {

            Opportunity p = new Opportunity();


            idOporExiste = default(Guid);
            bool bExisteOpor = false;
            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                //ColumnSet = new ColumnSet(new string[] { "Opportunityid" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal,  pIdbanner),
                        new ConditionExpression("ua_numero_solicitud", ConditionOperator.Equal,  pNumSulicitud),
                        new ConditionExpression("ua_periodo", ConditionOperator.Equal,  pPeriodo),

                         new ConditionExpression("ua_codigo_vpd", ConditionOperator.Equal, pVPDI)
                    }
                }
            };
            var ListOport = _xrmServerConnection.RetrieveMultiple(QueryOportEx);

            if (ListOport != null)
                if (ListOport.Entities.Any())
                {
                    var opr = ListOport.Entities.FirstOrDefault();
                    idOporExiste = new Guid(opr["opportunityid"].ToString());
                    bExisteOpor = true;

                }


            return bExisteOpor;
        }

        public Guid IdCuentaByOportunid(Guid opportunityid)
        {


            Guid idcuenta = default(Guid);

            Opportunity op = new Opportunity();

            QueryExpression QueryStage = new QueryExpression(Opportunity.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "parentaccountid" }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression("opportunityid", ConditionOperator.Equal,opportunityid),

                            },

                        }
                #endregion
            };
            var res = _xrmServerConnection.RetrieveMultiple(QueryStage);

            if (res.Entities != null && res.Entities.Any())
            {
                var c = res.Entities.FirstOrDefault();
                string v = ((EntityReference)c.Attributes["parentaccountid"]).Id.ToString();
                if (!string.IsNullOrEmpty(v))
                    idcuenta = new Guid(v);
            }

            return idcuenta;
        }

        private Guid RetriveContactoPrincipalCuenta(Guid pCuenta)
        {
            Guid resultado = default(Guid);

            //Cuenta Existe este PrimaryContactId
            Account c = new Account();

            QueryExpression Query = new QueryExpression(Account.EntityLogicalName)
            {
                NoLock = true,
                //ColumnSet = new ColumnSet(new string[] { "accountid", "primarycontactid" }),
                ColumnSet = new ColumnSet(new string[] { "primarycontactid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("accountid", ConditionOperator.Equal, pCuenta),

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Contactoprim = ec.Entities.FirstOrDefault();

                //resultado = new Guid(Contactoprim.Attributes["primarycontactid"].ToString());
                if (Contactoprim.Attributes.Contains("primarycontactid"))
                    resultado = ((EntityReference)Contactoprim.Attributes["primarycontactid"]).Id;

            }
            else
                throw new Exception("La cuenta " + pCuenta + " no existe");

            //consultar cuenta


            return resultado;
        }

        private PreUniversitario GetDatosBasicosProspecto(Guid pidContacto)
        {

            ColumnSet col = new ColumnSet(new string[] { "firstname", "middlename", "lastname", "emailaddress1", "telephone1", "ua_lada_telefono" });
            PreUniversitario p = null;

            QueryExpression Query = new QueryExpression(Contact.EntityLogicalName)
            {
                NoLock = true,
                // ColumnSet = new ColumnSet(new string[] { "firstname", "middlename", "lastname", "emailaddress1", "telephone1", "ua_lada_telefono" }),
                ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                        Conditions = {

                            new ConditionExpression("contactid", ConditionOperator.Equal, pidContacto),

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Contactoprim = ec.Entities.FirstOrDefault();
                p = new PreUniversitario();
                //Nombre
                if (Contactoprim.Attributes.ContainsKey("firstname"))
                    p.Nombre = Contactoprim.Attributes["firstname"].ToString();
                //Segundo nombre
                if (Contactoprim.Attributes.ContainsKey("middlename"))
                    p.Segundo_Nombre = Contactoprim.Attributes["middlename"].ToString();
                //Apellidos
                if (Contactoprim.Attributes.ContainsKey("lastname"))
                {
                    p.Apellido_Paterno = Contactoprim.Attributes["lastname"].ToString();
                    p.Apellido_Materno = Contactoprim.Attributes["lastname"].ToString();
                }
                //Estado
                if (Contactoprim.Attributes.ContainsKey("ua_codigo_estado"))
                    p.Estado = ((EntityReference)Contactoprim.Attributes["ua_codigo_estado"]).Id.ToString();
                //Estado civil
                if (Contactoprim.Attributes.ContainsKey("ua_codigo_delegacion"))
                    p.Municipio = ((EntityReference)Contactoprim.Attributes["ua_codigo_delegacion"]).Id.ToString();
                //Correo
                if (Contactoprim.Attributes.ContainsKey("emailaddress1"))
                    p.Correo_Electronico = Contactoprim.Attributes["emailaddress1"].ToString();
                //Telefono
                if (Contactoprim.Attributes.ContainsKey("telephone1"))
                    p.Telefono_Numero = Contactoprim.Attributes["telephone1"].ToString();
                //Lada
                if (Contactoprim.Attributes.ContainsKey("ua_lada_telefono"))
                    p.Telefono_Lada = Contactoprim.Attributes["ua_lada_telefono"].ToString();




                //p = new PreUniversitario
                //{
                //    Nombre = Contactoprim.Attributes["firstname"].ToString(),
                //    Segundo_Nombre = Contactoprim.Attributes["middlename"].ToString(),
                //    Correo_Electronico = Contactoprim.Attributes["emailaddress1"].ToString(),
                //    Telefono_Numero = Contactoprim.Attributes["telephone1"].ToString(),

                //};
                // resultado = new Guid(Contactoprim.Attributes["PrimaryContactid"].ToString());

            }
            return p;
        }

        private Guid ConvertirProspectoEnOportunidad(Guid idCuenta, Guid idProspecto)
        {
            #region Calificar prospecto y convertirlo en oportunidad

            Guid idOportunidadRegresar = default(Guid);
            //Convertir la cuenta en oportunidad
            var query = new QueryExpression("organization");
            query.ColumnSet = new ColumnSet("basecurrencyid");
            var result = _xrmServerConnection.RetrieveMultiple(query);
            var currencyId = (EntityReference)result.Entities[0]["basecurrencyid"];

            var qualifyIntoOpportunityReq = new QualifyLeadRequest
            {
                CreateOpportunity = true,
                OpportunityCurrencyId = currencyId,
                OpportunityCustomerId = new EntityReference(
                        Account.EntityLogicalName,
                        idCuenta),
                Status = new OptionSetValue(3),
                LeadId = new EntityReference(Lead.EntityLogicalName, idProspecto)
            };



            //Mandamos Calificar el prospecto para convertirlo en oportunidad
            var qualifyIntoOpportunityRes =
                (QualifyLeadResponse)_xrmServerConnection.Execute(qualifyIntoOpportunityReq);
            idOportunidadRegresar = new Guid(qualifyIntoOpportunityRes.CreatedEntities[0].Id.ToString());



            return idOportunidadRegresar;

            #endregion
        }

        private Guid GetIDProspecto(Guid pidCuenta)
        {
            Guid idCuenta = default(Guid);
            Lead P = new Lead();

            EntityReference cuentare = new EntityReference(Account.EntityLogicalName, pidCuenta);
            QueryExpression Query = new QueryExpression(Lead.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_id_banner", "parentaccountid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("parentaccountid", ConditionOperator.Equal, cuentare.Id)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                var r = ((EntityReference)prospecto.Attributes["accountid"]).Id.ToString();
                if (string.IsNullOrEmpty(r))
                    idCuenta = new Guid(r);

            }
            return idCuenta;
        }

        private string GetEscuelaProcedenciaCuenta(Guid idcuenta, out string promediop)
        {
            Account c = new Account();

            string escp = "";
            promediop = "";
            ColumnSet col = new ColumnSet(new string[] { "ua_colegio_procedencia", "ua_promedio" });

            QueryExpression Query = new QueryExpression(Account.EntityLogicalName)
            {
                NoLock = true,
                // ColumnSet = new ColumnSet(new string[] { "firstname", "middlename", "lastname", "emailaddress1", "telephone1", "ua_lada_telefono" }),
                ColumnSet = col,
                Criteria = {
                        Conditions = {

                            new ConditionExpression("accountid", ConditionOperator.Equal, idcuenta),

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                if (prospecto.Attributes.Contains("ua_colegio_procedencia"))
                    escp = ((EntityReference)prospecto.Attributes["ua_colegio_procedencia"]).Id.ToString();

                if (prospecto.Attributes.Contains("ua_promedio"))
                    promediop = prospecto.Attributes["ua_promedio"].ToString();


            }
            return escp;

        }

        public void ReOpenProspecto(Guid idProspecto)
        {
            /* Reopen the Opportunity record */
            EntityReference oppRef = new EntityReference()
            {
                LogicalName = Lead.EntityLogicalName,
                Id = idProspecto
            };
            SetStateRequest openOpp = new SetStateRequest();
            openOpp.EntityMoniker = oppRef;
            openOpp.State = new OptionSetValue(0); // Open
            openOpp.Status = new OptionSetValue(1); // In Progress

            _xrmServerConnection.Execute(openOpp);
        }

        private Guid GetIdPariente(Guid idcuentap, EntityReference parentesco)
        {
            Guid idcontactoReturn = default(Guid);
            string name = "";
            Contact cont = new Contact();
            EntityReference cuentaref = new EntityReference(Account.EntityLogicalName, idcuentap);
            ColumnSet col = new ColumnSet(new string[] { "contactid", "fullname", "ua_tipo_parentesco_cuenta" });

            QueryExpression Query = new QueryExpression(Contact.EntityLogicalName)
            {
                NoLock = true,
                // ColumnSet = new ColumnSet(new string[] { "firstname", "middlename", "lastname", "emailaddress1", "telephone1", "ua_lada_telefono" }),
                ColumnSet = col,
                Criteria = {
                        Conditions = {

                            new ConditionExpression("accountid", ConditionOperator.Equal, cuentaref.Id),
                            new ConditionExpression("ua_tipo_parentesco_cuenta", ConditionOperator.Equal, parentesco.Id)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                if (prospecto.Attributes.Contains("contactid"))
                    idcontactoReturn = new Guid(prospecto.Attributes["contactid"].ToString());

                if (prospecto.Attributes.Contains("fullname"))
                    name = prospecto.Attributes["fullname"].ToString();


            }
            return idcontactoReturn;
        }

        #region Catalogos Pais, Estado, Municipio, Delegacion

        private List<Pais> GetAllPaises()
        {
            dynamic listP = null;



            QueryExpression QueryPais = new QueryExpression(ua_pais.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "ua_codigo_pais", "ua_desc_pais" }),

            };
            var ListOportRel = _xrmServerConnection.RetrieveMultiple(QueryPais);

            if (ListOportRel != null)
                if (ListOportRel.Entities.Any())
                {
                    listP = new List<Pais>();
                    Pais PaisR = null;
                    foreach (var item in ListOportRel.Entities)
                    {
                        PaisR = new Pais();
                        if (item.Attributes.Contains("ua_codigo_pais"))
                            PaisR.Clave = item.Attributes["ua_codigo_pais"].ToString();
                        if (item.Attributes.Contains("ua_desc_pais"))
                            PaisR.Descripcion = item.Attributes["ua_desc_pais"].ToString();


                        listP.Add(PaisR);
                    }

                }
            return listP;
        }

        private List<Estado> GetAllEstados()
        {
            dynamic listE = null;



            Entity Estados = new Entity();
            Estados.LogicalName = "ua_estados";

            Entity SystemPais = new Entity();
            SystemPais.LogicalName = "ua_pais";

            QueryExpression query = new QueryExpression()
            {
                Distinct = false,
                EntityName = Estados.LogicalName,
                ColumnSet = new ColumnSet(new string[] { "ua_codigo_estado", "ua_desc_estado", "ua_codigo_pais" }),

                LinkEntities =
                {
             new LinkEntity {
                        JoinOperator = JoinOperator.Inner,
                         LinkFromAttributeName = "ua_codigo_pais",
                        LinkFromEntityName = Estados.LogicalName,
                        LinkToAttributeName = "ua_paisid",
                        LinkToEntityName = SystemPais.LogicalName,
                        Columns = new ColumnSet(new string[]{ "ua_codigo_pais"}),
                    }
                 },

                Criteria = {
    //Filters = {
    //  new FilterExpression {
    //    FilterOperator = LogicalOperator.And, Conditions = {
    //      new ConditionExpression("systemuserid", ConditionOperator.Equal, "9b1bf31d-ac29-e211-9826-00155d0a0b0f"),

    //    },
    //  },

    //}
                    }
            };


            var ec = _xrmServerConnection.RetrieveMultiple(query);
            if (ec.Entities.Any())
            {
                listE = new List<Estado>();
                foreach (var Est in ec.Entities)
                {
                    var mun1 = new Estado()
                    {
                        Clave = Est["ua_codigo_estado"].ToString(),
                        Descripcion = Est["ua_desc_estado"].ToString(),
                        Pais = ((AliasedValue)Est["ua_pais1.ua_codigo_pais"]).Value.ToString(),
                    };
                    listE.Add(mun1);
                }

            }


            return listE;
        }

        private List<Municipios> GetallMunicipios()
        {
            dynamic listMun = null;


            Entity Municipio = new Entity();
            Municipio.LogicalName = "ua_delegacion_municipio";

            Entity Estados = new Entity();
            Estados.LogicalName = "ua_estados";

            Entity SystemPais = new Entity();
            SystemPais.LogicalName = "ua_pais";

            QueryExpression query = new QueryExpression()
            {
                Distinct = false,
                EntityName = Municipio.LogicalName,
                ColumnSet = new ColumnSet(new string[] { "ua_codigo_municipio", "ua_desc_municipio", "ua_desc_pais" }),

                LinkEntities =
                {
             new LinkEntity {
                        JoinOperator = JoinOperator.Inner,
                         LinkFromAttributeName = "ua_desc_estado",
                        LinkFromEntityName = Municipio.LogicalName,
                        LinkToAttributeName = "ua_estadosid",
                        LinkToEntityName = Estados.LogicalName,
                        Columns = new ColumnSet(new string[]{ "ua_codigo_estado"}),
                        EntityAlias="Estado",
                    },
             new LinkEntity {
                        JoinOperator = JoinOperator.Inner,
                         LinkFromAttributeName = "ua_desc_pais",
                        LinkFromEntityName = Municipio.LogicalName,
                        LinkToAttributeName = "ua_paisid",
                        LinkToEntityName = SystemPais.LogicalName,
                        Columns = new ColumnSet(new string[]{ "ua_codigo_pais"}),
                        EntityAlias="Pais",
                    }
                 },


            };


            var ec = _xrmServerConnection.RetrieveMultiple(query);
            if (ec.Entities.Any())
            {
                listMun = new List<Municipios>();
                foreach (var cat in ec.Entities)
                {
                    var mun = new Municipios()
                    {
                        Clave = cat["ua_codigo_municipio"].ToString(),
                        Descripcion = cat["ua_desc_municipio"].ToString(),
                        Estado = ((AliasedValue)cat["Estado.ua_codigo_estado"]).Value.ToString(),
                        Pais = ((AliasedValue)cat["Pais.ua_codigo_pais"]).Value.ToString()
                    };
                    listMun.Add(mun);
                }

            }


            return listMun;
        }

        private List<Colegios> GetallColegioProcedencia()
        {
            dynamic listColegio = null;

            ua_colegios co = new ua_colegios();


            QueryExpression QueryPais = new QueryExpression(ua_colegios.EntityLogicalName)
            {
                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "ua_codigo_colegio", "ua_desc_colegios" }),

            };

            var ListOportRel = _xrmServerConnection.RetrieveMultiple(QueryPais);

            if (ListOportRel != null)
                if (ListOportRel.Entities.Any())
                {
                    listColegio = new List<Colegios>();
                    Colegios Colegio = null;
                    foreach (var item in ListOportRel.Entities)
                    {
                        Colegio = new Colegios();
                        if (item.Attributes.Contains("ua_codigo_colegio"))
                            Colegio.Clave = item.Attributes["ua_codigo_colegio"].ToString();
                        if (item.Attributes.Contains("ua_desc_colegios"))
                            Colegio.Descripcion = item.Attributes["ua_desc_colegios"].ToString();


                        listColegio.Add(Colegio);
                    }

                }
            return listColegio;
        }



        #endregion

        #region Catalogos asesor

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
                if (programa.Attributes.Contains("ua_programas_por_campus_asesor"))
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
                        msjReturn = "El código de Pais  " + descF1 + " con la Vpd " + descF2 + " no existe en el catálogo";
                        break;
                    case "ua_estados_asesor":
                        msjReturn = "El código de estado  " + descF1 + " con la Vpd " + descF2 + " no existe en el catálogo";
                        break;
                    case "ua_delegacion_municipio_asesor":
                        msjReturn = "El código de Municipio  " + descF1 + " con la  Vpd " + descF2 + " no existe en el catálogo";
                        break;
                    case "ua_programas_por_campus_asesor":
                        msjReturn = "El código de programa  " + descF1 + " con la vpd " + descF2 + " no existe en el catálogo";
                        break;
                    case "ua_colegios_asesor":
                        msjReturn = "El código colegio   " + descF1 + " con la vpd " + descF2 + " no existe en el catálogo";
                        break;

                    default:
                        break;
                }

                throw new LookupException(msjReturn);
            }

            return new EntityReference(EntityLogicalName, idp);

        }

        private EntityReference GetDatosAsesorCuenta(string EntityLogicalName, EntityReference CampoIdValor, string campoidNombre, string campoReturn, string campostrinName, out string campstrinValor)
        {
            ua_estados_asesor es = new ua_estados_asesor();
            ua_pais_asesor p = new ua_pais_asesor();

            EntityReference result = new EntityReference();
            campstrinValor = "";
            Guid idp = default(Guid);

            QueryExpression Query = new QueryExpression(EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { campoidNombre, campoReturn, campostrinName }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression(campoidNombre, ConditionOperator.Equal, CampoIdValor.Id)


                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var programa = ec.Entities.FirstOrDefault();
                if (programa.Attributes.Contains(campoReturn))
                    idp = ((EntityReference)programa.Attributes[campoReturn]).Id;
                if (programa.Attributes.Contains(campostrinName))
                    campstrinValor = programa.Attributes[campostrinName].ToString();



            }
            if (idp != Guid.Empty)
                return new EntityReference(ua_pais_asesor.EntityLogicalName, idp);
            else return null;
        }


        #endregion



        private string SubOrigenOportundiad(Guid idOportundiad)
        {
            string SubOrigen = "", origen = ""; ;
            //Guid idOportundiad = new Guid(idOp);
            Opportunity op = new Opportunity();




            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner", "ua_suborigen", "ua_origen" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("opportunityid", ConditionOperator.Equal, idOportundiad)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Opportunity = ec.Entities.FirstOrDefault();
                if (Opportunity.Attributes.Contains("ua_suborigen"))
                    SubOrigen = Opportunity.Attributes["ua_suborigen"].ToString();
                else
                    SubOrigen = "Prospecto";
                if (Opportunity.Attributes.Contains("ua_origen"))
                    origen = ((OptionSetValue)Opportunity.Attributes["ua_origen"]).Value.ToString();

            }
            return SubOrigen;

        }

        //Metodo para validar la 31, y saber si la oportidad tiene origen o es nulloi
        //si el origen es nullo, fue creada desde cuenta
        private string OrigenOportundiad(Guid idOportundiad)
        {
            string SubOrigen = "", origen = ""; ;
            //Guid idOportundiad = new Guid(idOp);
            Opportunity op = new Opportunity();




            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner", "ua_suborigen", "ua_origen" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("opportunityid", ConditionOperator.Equal, idOportundiad)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Opportunity = ec.Entities.FirstOrDefault();

                //if (Opportunity.Attributes.Contains("ua_origen"))
                //    origen = Opportunity.Attributes["ua_origen"].ToString();
                //else
                //    origen = "Prospecto";

                if (Opportunity.Attributes.Contains("ua_origen"))
                    origen = ((OptionSetValue)Opportunity.Attributes["ua_origen"]).Value.ToString();

            }
            return origen;

        }


        /// <summary>
        /// Inicializa el objeto oportunidad segun el id recibido
        /// </summary>
        /// <param name="Id">Id de oportunidad</param>
        Opportunity GetDefaultOpportunity(string Id)
        {
            if (!string.IsNullOrEmpty(Id.Trim()))
                return new Opportunity() { OpportunityId = new Guid(Id.Trim()) };
            else
                throw new Exception("El campo id oportunidad no puede ir vacio");
        }

        /// <summary>
        /// Obtiene la oportunidad cuyo estatus es abierta (0)
        /// </summary>
        /// <param name="Id">Id de Oportunidad </param>
        Opportunity GetOpenOpportunity(string Id)
        {
            var op = new Opportunity();
            if (!string.IsNullOrEmpty(Id.Trim()))
                op.OpportunityId = new Guid(Id.Trim());
            else
                throw new Exception("El campo id oportunidad no puede ir vacio");

            var opportunityId = (Guid)op.OpportunityId;

            if (_OpportunityRepository.IsClosed(opportunityId))
                throw new CRMExceptionB("La oportunidad  se encuentra cerrada." + Id);
            return op;
            //if (_OpportunityRepository.IsOpen(opportunityId))
            //    return op;
            //else
            // return new Opportunity() { OpportunityId = null, StatusCode = _OpportunityRepository.RetrieveStatusById(opportunityId) };
        }

        /// <summary>
        /// Metodo que por medio de  idBanner obtiene el ProspectoId que es el mismo identificador para solicitante, busca apartir de este y otros parametros de entrada y regresa el Id de la entidad 
        /// </summary>
        /// <param name="parametros"> IdBanner, VPDI, Tipo y Secuencia </param>  
        /// <param name="columna">columna o clave principal de la Entidad</param>  
        /// <param name="condicion">nombre de la columna en donde se buscará</param>  
        /// <param name="EntityLogicalName">Entidad donde se realizará la consulta</param>  
        /// <returns></returns>
        private Guid RetriveCorreoId(string idBanner, EntityReference VPDI, EntityReference Tipo, string Secuencia, string columna, string[] condicion, string EntityLogicalName)
        {
            Guid resultado = default(Guid);
            Guid solicitante = RetrieveCuentaId(idBanner);
            QueryExpression Query = new QueryExpression(EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { columna }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression(condicion[0], ConditionOperator.Equal, solicitante),
                              new ConditionExpression(condicion[1], ConditionOperator.Equal,VPDI.Id),
                                new ConditionExpression(condicion[2], ConditionOperator.Equal, Tipo.Id),
                                  new ConditionExpression(condicion[3], ConditionOperator.Equal, Secuencia)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var periodoent = ec.Entities.FirstOrDefault();
                resultado = new Guid(periodoent.Attributes[columna].ToString());

            }
            return resultado;
        }

        /// <summary>
        /// Metodo que por medio de  idBanner obtiene el ProspectoId que es el mismo identificador para solicitante, busca apartir de este y otros parametros de entrada y regresa el Id de la entidad 
        /// </summary>
        /// <param name="parametros"> IdBanner, VPDI, Tipo y Secuencia </param>  
        /// <param name="columna">columna o clave principal de la Entidad</param>  
        /// <param name="condicion">nombre de la columna en donde se buscará</param>  
        /// <param name="EntityLogicalName">Entidad donde se realizará la consulta</param>  
        /// <returns></returns>
        private Guid RetriveTelefonoId(string idBanner, EntityReference VPDI, EntityReference Tipo, int? Secuencia, string columna, string[] condicion, string EntityLogicalName)
        {
            Guid resultado = default(Guid);
            Guid solicitante = RetrieveCuentaId(idBanner);
            QueryExpression Query = new QueryExpression(EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { columna }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression(condicion[0], ConditionOperator.Equal, solicitante),
                              new ConditionExpression(condicion[1], ConditionOperator.Equal,VPDI.Id),
                                new ConditionExpression(condicion[2], ConditionOperator.Equal, Tipo.Id),
                                  new ConditionExpression(condicion[3], ConditionOperator.Equal, Secuencia)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var periodoent = ec.Entities.FirstOrDefault();
                resultado = new Guid(periodoent.Attributes[columna].ToString());

            }
            return resultado;
        }

        /// <summary>
        ///  Metodo que recibe contactoo y regresa el Id de la entidad contacto
        /// </summary>
        /// <param name="contacto"></param>
        /// <returns></returns>
        private Guid RetriveContactoId(string contacto, int tipoContacto, EntityReference VPDI)
        {
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(Contact.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "contactid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("fullname", ConditionOperator.Equal, contacto),
                               new ConditionExpression("rs_tipocontacto", ConditionOperator.Equal, tipoContacto),
                                  new ConditionExpression("rs_vpdid", ConditionOperator.Equal, VPDI.Id)
                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var periodoent = ec.Entities.FirstOrDefault();
                resultado = new Guid(periodoent.Attributes["contactid"].ToString());

            }
            return resultado;
        }

        /// <summary>
        /// Metodo que recibe Clave Colegio y regresa el Id de la entidad Account
        /// </summary>
        /// <param name="claveColegio"></param>       
        /// <returns></returns>
        private Guid RetriveAccountId(string claveColegio)
        {
            Guid resultado = default(Guid);

            QueryExpression Query = new QueryExpression(ua_colegios.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_colegiosid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_codigo_colegio", ConditionOperator.Equal, claveColegio)
                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var periodoent = ec.Entities.FirstOrDefault();
                resultado = new Guid(periodoent.Attributes["ua_colegiosid"].ToString());

            }
            return resultado;
        }

        /// <summary>
        /// Metodo que recibe periodo y regresa el Id de la entidad rs_periodo
        /// </summary>
        /// <param name="periodo"></param>       
        /// <returns></returns>
        private Guid RetrivePeriodoId(string periodo)
        {
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(ua_periodo.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_periodoid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_periodo", ConditionOperator.Equal, periodo)
                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var periodoent = ec.Entities.FirstOrDefault();
                resultado = new Guid(periodoent.Attributes["ua_periodoid"].ToString());

            }
            else
                throw new LookupException("el  periodo " + periodo + " no fue encontro en el catalogo");

            return resultado;
        }

        private Guid RetrivePeriodo30(string periodo)
        {
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(ua_periodo.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_periodoid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_periodo", ConditionOperator.Equal, periodo)
                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var periodoent = ec.Entities.FirstOrDefault();
                resultado = new Guid(periodoent.Attributes["ua_periodoid"].ToString());

            }


            return resultado;
        }

        /// <summary>
        /// Metodo que recibe idBanner de prospecto, tipo parentesco y  regresa LeadId (Guid) de la entidad Lead
        /// </summary>
        /// <param name="idBanner"></param>       
        /// <param name="parentesco">valor del parentesco OptionSetValue</param>
        private Guid RetriveLeadId(Guid solicitanteid, string RowId)
        {
            //Guid solicitante = RetrieveProspectId(idBanner);//ProspectID se relaciona directamente con solicitanteid (rs_prospectid=rs_solicitanteid)
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(Lead.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "leadid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("rs_solicitante", ConditionOperator.Equal, solicitanteid),
                            new ConditionExpression("rs_rowid", ConditionOperator.Equal, RowId)
                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Item = ec.Entities.FirstOrDefault();
                resultado = new Guid(Item.Attributes["leadid"].ToString());

            }
            return resultado;
        }
        //obtiene el progama en bace  al código de carrera
        private EntityReference RetrivePrograma(string sCodigoPograma)
        {

            EntityReference resultado = default(EntityReference);
            //Guid idPrograma = default(Guid);
            Guid idCarreraweb = default(Guid);
            ua_carreras_web c = new ua_carreras_web();


            QueryExpression QueryCarr = new QueryExpression(ua_carreras_web.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_carreras_webid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_codigo_carrera_web", ConditionOperator.Equal, sCodigoPograma),

                        }
                    }
            };
            var listCarre = _xrmServerConnection.RetrieveMultiple(QueryCarr);

            if (listCarre.Entities.Any())
            {
                var periodoent = listCarre.Entities.FirstOrDefault();
                idCarreraweb = new Guid(periodoent.Attributes["ua_carreras_webid"].ToString());
                resultado = new EntityReference(ua_carreras_web.EntityLogicalName, idCarreraweb);
            }

            //ua_programa pr = new ua_programa();



            //QueryExpression Query = new QueryExpression(ua_programaV2.EntityLogicalName)
            //{

            //    NoLock = true,
            //    ColumnSet = new ColumnSet(new string[] { "ua_programav2id", "ua_codigo_del_programa" }),
            //    Criteria = {
            //            Conditions = {

            //                new ConditionExpression("ua_codigo_carrera_web", ConditionOperator.Equal, idCarreraweb),

            //            }
            //        }
            //};
            //var listPrograma = _xrmServerConnection.RetrieveMultiple(Query);

            //if (listPrograma.Entities.Any())
            //{
            //    var Programas = listPrograma.Entities.FirstOrDefault();
            //    //pCodigoPrograma = Programas.Attributes["ua_codigo_del_programa"].ToString();
            //    //idPrograma = new Guid(Programas.Attributes["ua_programav2id"].ToString());
            //   // resultado = new EntityReference(ua_carreras_web.EntityLogicalName, idPrograma);
            //}

            return resultado;

        }

        private Guid GetProgramaId(string pcodigoPrograma)
        {
            Guid idProgrma = default(Guid);


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

            }
            else
                throw new LookupException("el  Programa " + pcodigoPrograma + " no fue encontro en el catalogo");
            return idProgrma;
        }


        private EntityReference GetProgramaByVPd(string pcodigoPrograma, EntityReference Campus)
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
                    throw new LookupException("El id campus  " + Campus.Id + " y el id programa " + idProgrma + " no existe en el catálogo");
            }
            //else
            //    throw new LookupException("el  Programa " + pcodigoPrograma + " no fue encontro en el catalogo");
            return Progr;
        }



        public Guid CrearProspectoGenericoMalo(PreUniversitario preUniversitario)
        {



            Guid res = default(Guid);
            Lead Prospecto = new Lead();

            Prospecto.FirstName = preUniversitario.Nombre;
            Prospecto.MiddleName = preUniversitario.Segundo_Nombre;
            Prospecto.LastName = SplitApellidoPaterno(preUniversitario.Apellido_Paterno) + " " + SplitApellidoMaterno(preUniversitario.Apellido_Materno);
            Prospecto.Subject = preUniversitario.Nombre + " " + preUniversitario.Segundo_Nombre + " " + Prospecto.LastName;

            Prospecto.Telephone1 = preUniversitario.Telefono_Lada + preUniversitario.Telefono_Numero;
            Prospecto.ua_lada_telefono = preUniversitario.Telefono_Lada;
            Prospecto.EMailAddress1 = preUniversitario.Correo_Electronico;

            if (preUniversitario.Nivel != null)
                Prospecto.ua_codigo_nivel = _entityReferenceTransformer.GetNivel(preUniversitario.Nivel.ToUpper());

            if (preUniversitario.Campus != null)
                Prospecto.ua_codigo_campus = _entityReferenceTransformer.GetCampus(preUniversitario.Campus.ToUpper());

            Guid idp = GetProgramaId(preUniversitario.Codigo);
            if (!string.IsNullOrWhiteSpace(preUniversitario.Codigo))
                // Prospecto.ua_programav2 = RetrieveProgramaByCarreraWeb(new EntityReference(ua_programaV2.EntityLogicalName, idp), Prospecto.ua_codigo_campus, preUniversitario.Campus, preUniversitario.Codigo);
                //Prospecto.ua_codigo_del_programa = CodigoPrograma;
                if (preUniversitario.Estado != null)
                    Prospecto.ua_codigo_estado = new EntityReference(ua_estados.EntityLogicalName, new Guid(preUniversitario.Estado));
            //Prospecto.ua_codigo_estado = _entityReferenceTransformer.GetEstadoSE(preUniversitario.Estado.ToUpper());
            if (preUniversitario.Municipio != null)
                Prospecto.ua_codigo_delegacion = new EntityReference(ua_delegacion_municipio.EntityLogicalName, new Guid(preUniversitario.Municipio));
            //Prospecto.ua_codigo_delegacion = _entityReferenceTransformer.GetMunicipioId(preUniversitario.Municipio.ToUpper());
            // Prospecto.ua_Otro_Estado = preUniversitario.OtroEstado;

            //Banner =3
            //WEb o CRM = 1
            //Formulario APREU=2
            if (preUniversitario.Origen != null)
                Prospecto.ua_origen = new OptionSetValue(int.Parse(preUniversitario.Origen));
            Prospecto.ua_suborigen = preUniversitario.SubOrigen;
            Prospecto.ua_codigo_vpd = preUniversitario.VPD;


            //Prospecto.rs_informacioncorrecta = true;
            //Prospecto.ua_AsignarAsesor=


            res = _xrmServerConnection.Create(Prospecto);





            return res;
        }

        /// <summary>
        /// Metodo para Separar los apellidos y regresar el apellido paterno
        /// </summary>
        /// <param name="apellido"></param>
        /// <returns></returns>
        private string SplitApellidoPaterno(string apellido)
        {
            var appellidos = apellido.Split('*');

            if (appellidos != null && appellidos.Length > 0)
                return appellidos[0];

            throw new InvalidCastException("No se pudo extraer el apellido paterno del tutor");
        }

        /// <summary>
        /// Metodo para Separar los apellidos y regresar el apellido materno
        /// </summary>
        /// <param name="apellido"></param>
        /// <returns></returns>
        private string SplitApellidoMaterno(string apellido)
        {
            var appellidos = apellido.Split('*');

            if (appellidos != null && appellidos.Length == 2)
                return appellidos[1];

            return string.Empty;
        }

        private DateTime? ResolveFecha(CustomDate Fecha)
        {
            if (Fecha != null)
                return new DateTime(Fecha.Year, Fecha.Month, Fecha.Day);
            else
                return null;
        }

        private int GetEdad(DateTime FechaNacimiento)
        {
            int edad = 0;
            DateTime hoy = DateTime.Now;
            var Anos = hoy.Year - FechaNacimiento.Year;
            var meses = hoy.Month - FechaNacimiento.Month;

            if (meses < 0 || (meses == 0 && FechaNacimiento.Day < hoy.Day))
                edad += Anos + 1;
            else
                edad += Anos;
            return edad;

        }

        private OptionSetValue ResolveSexo(string Sexo)
        {
            if (!string.IsNullOrWhiteSpace(Sexo))
            {
                var item = _picklistRepository.ListaSexo();
                if (item.ContainsValue(Sexo))
                    return new OptionSetValue(item.FirstOrDefault(i => i.Value == Sexo).Key);
                else
                    throw new PickListException("No se pudo resolver el picklist de Sexo");
            }
            else
                return null;

        }

        private EntityReference GetIdReferencia(string EntyLogicaname, string Codigoid, string CampoComparar, string valor)
        {
            //return ListaCatalogo("NivelesEd", ua_niveles.EntityLogicalName, "ua_codigo_nivel", "ua_nivelesid");
            ua_periodo p = new ua_periodo();
            EntityReference resultado = null;
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


        /// <summary>
        /// Metodo que recibe idBanner de prospecto y regresa rs_becacreditoid (Guid) de la entidad rs_becacredito
        /// </summary>
        /// <param name="idBanner"></param>
        /// <param name="periodotipo">tipo de periodo de la entidad beca/crédito</param>
        /// <param name="periodo">valor del periodo EntityReference</param>

        private Guid RetriveBecaCreditoId(string idBanner, string pDescipCredito, EntityReference VPD, EntityReference periodo)
        {
            //Guid solicitante = RetrieveOpportunityId(idBanner);//ProspectID se relaciona directamente con solicitanteid (rs_prospectid=rs_solicitanteid)
            // Guid solicitante = RetrieveCuentaId(idBanner);//ProspectID se relaciona directamente con solicitanteid (rs_prospectid=rs_solicitanteid)
            ua_credito_educativo_otorgado c = new ua_credito_educativo_otorgado();

            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(ua_credito_educativo_otorgado.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner", "ua_credito_educativo_otorgadoid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_idbanner", ConditionOperator.Equal, idBanner),
                            new ConditionExpression("ua_credito_periodo", ConditionOperator.Equal, periodo.Id),
                            new ConditionExpression("ua_credito_vpd", ConditionOperator.Equal,VPD.Id),
                             new ConditionExpression("ua_credito_desc", ConditionOperator.Equal,pDescipCredito)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var becaCredito = ec.Entities.FirstOrDefault();
                resultado = new Guid(becaCredito.Attributes["ua_credito_educativo_otorgadoid"].ToString());

            }
            return resultado;
        }

        private Guid ExisteBecaOtorgada(string idBanner, EntityReference vpd, string becaTipo, EntityReference periodo, Guid pCuenta)
        {
            //Guid solicitante = RetrieveOpportunityId(idBanner);//ProspectID se relaciona directamente con solicitanteid (rs_prospectid=rs_solicitanteid)
            // Guid solicitante = RetrieveCuentaId(idBanner);//ProspectID se relaciona directamente con solicitanteid (rs_prospectid=rs_solicitanteid)
            //ua_credito_educativo_otorgado c = new ua_credito_educativo_otorgado();

            //BecOtorg.ua_beca_ot_vpd
            //BecOtorg.ua_beca_ot_periodo
            //BecOtorg.ua_BecasOtorgadasId
            //BecOtorg.ua_beca_ot_tipo

            ua_becaotorgada b = new ua_becaotorgada();

            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(ua_becaotorgada.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner", "ua_becaotorgadaid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_idbanner", ConditionOperator.Equal, idBanner),
                            new ConditionExpression("ua_beca_ot_periodo", ConditionOperator.Equal, periodo.Id),
                             new ConditionExpression("ua_beca_ot_vpd", ConditionOperator.Equal, vpd.Id),
                              new ConditionExpression("ua_beca_ot_tipo", ConditionOperator.Equal,becaTipo)
                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var becaCredito = ec.Entities.FirstOrDefault();
                resultado = new Guid(becaCredito.Attributes["ua_becaotorgadaid"].ToString());

            }
            return resultado;
        }

        private Guid ExisteSolBeca(string idBanner, EntityReference vpd, string becaTipo, string periodo, Guid pCuenta)
        {
            var resultado = default(Guid);
            var Query = new QueryExpression(ua_solicituddebeca.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner", "ua_solicituddebecaid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal, idBanner),
                        new ConditionExpression("ua_beca_sol_periodo", ConditionOperator.Equal, periodo),
                        new ConditionExpression("ua_beca_sol_vpd", ConditionOperator.Equal, vpd.Id),
                        new ConditionExpression("ua_tipodebeca", ConditionOperator.Equal,becaTipo)
                    }
                }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var becaCredito = ec.Entities.FirstOrDefault();
                resultado = new Guid(becaCredito.Attributes["ua_solicituddebecaid"].ToString());
            }
            return resultado;
        }

        /// <summary>
        /// Determina el programa de acuerdo a la carrera web y el campus 
        /// </summary>
        /// <param name="carreraWeb"></param>
        /// <param name="Campus"></param>
        /// <returns></returns>
        public EntityReference RetrieveProgramaByCarreraWeb(EntityReference carreraWeb, EntityReference Campus, out string pCodigoPrograma, string pCampus, string pPrograma)
        {
            pCodigoPrograma = "";
            Guid idp = default(Guid);
            if (carreraWeb == null)
                throw new LookupException("La carrera web es requerida para recuperar el programa");
            if (Campus == null)
                throw new LookupException("El Campus es requerido para recuperar el programa");
            EntityReference result = new EntityReference();
            //ua_programas_por_campus pc = new ua_programas_por_campus();

            ua_ppc_programav2 pc = new ua_ppc_programav2();

            //pc.ua_codigo_carrera_web

            QueryExpression Query = new QueryExpression(ua_ppc_programav2.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_ppc_programav2id", "ua_codigo_programa", "statecode", "statuscode" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_ppc_campus", ConditionOperator.Equal, Campus.Id),
                            new ConditionExpression("ua_codigo_carrera_web", ConditionOperator.Equal, carreraWeb.Id),
                            new ConditionExpression("statecode", ConditionOperator.Equal, 0)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var programa = ec.Entities.FirstOrDefault();
                idp = new Guid(programa.Attributes["ua_ppc_programav2id"].ToString());
                pCodigoPrograma = programa.Attributes["ua_codigo_programa"].ToString();

                //result = (EntityReference)programa.Attributes["ua_ppc_programaid"];
                //result = new EntityReference(ua_ppc_programa.EntityLogicalName, idp);

            }
            if (idp == default(Guid))
                throw new LookupException("El id campus  " + pCampus + " y el id programa " + pPrograma + " no existe en el catálogo");
            return new EntityReference(ua_ppc_programav2.EntityLogicalName, idp);

        }



        public EntityReference RetrieveProgramaByCarreraWeb(EntityReference carreraWeb, EntityReference Campus, string pCampus, string pPrograma)
        {

            Guid idp = default(Guid);
            if (carreraWeb == null)
                throw new LookupException("La carrera web es requerida para recuperar el programa");
            if (Campus == null)
                throw new LookupException("El Campus es requerido para recuperar el programa");
            EntityReference result = new EntityReference();
            //ua_programas_por_campus pc = new ua_programas_por_campus();

            ua_ppc_programav2 pc = new ua_ppc_programav2();

            //pc.ua_codigo_carrera_web

            QueryExpression Query = new QueryExpression(ua_ppc_programav2.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_ppc_programav2id", "ua_codigo_programa" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_ppc_campus", ConditionOperator.Equal, Campus.Id),
                            new ConditionExpression("ua_programa", ConditionOperator.Equal, carreraWeb.Id)

                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var programa = ec.Entities.FirstOrDefault();
                idp = new Guid(programa.Attributes["ua_ppc_programav2id"].ToString());


                //result = (EntityReference)programa.Attributes["ua_ppc_programaid"];
                //result = new EntityReference(ua_ppc_programa.EntityLogicalName, idp);

            }
            if (idp == default(Guid))
                throw new LookupException("El campus " + pCampus + " y el id programa " + pPrograma + " no existe en el catálogo");
            return new EntityReference(ua_ppc_programav2.EntityLogicalName, idp);

        }

        public EntityReference RetrieveProgramaByCarreraWebAsesor(EntityReference Programa, EntityReference vpd, string pCampus, string pPrograma)
        {

            Guid idp = default(Guid);
            if (Programa == null)
                throw new LookupException("el programa es requerida para recuperar el programa");
            if (vpd == null)
                throw new LookupException("El Campus es requerido para recuperar el programa");
            EntityReference result = new EntityReference();
            //ua_programas_por_campus pc = new ua_programas_por_campus();


            ua_programas_por_campus_asesor pca = new ua_programas_por_campus_asesor();


            QueryExpression Query = new QueryExpression(ua_programas_por_campus_asesor.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_programas_por_campus_asesorid", "ua_codigo_del_programa", "ua_programa_campus_asesor_desc" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("ua_codigo_vpd", ConditionOperator.Equal, vpd.Id),
                            new ConditionExpression("ua_programaporcampusasesorid", ConditionOperator.Equal, Programa.Id)


                        }
                    }
            };
            var ec = _xrmServerConnection.RetrieveMultiple(Query);

            foreach (var entity in ec.Entities)
            {
                if (entity.Attributes["ua_programa_campus_asesor_desc"].ToString().Contains(pCampus))
                {
                    var programa = entity;
                    idp = new Guid(programa.Attributes["ua_programas_por_campus_asesorid"].ToString());
                    break;
                }
            }

            //if (ec.Entities.Any())
            //{
            //    var programa = ec.Entities.FirstOrDefault();
            //    idp = new Guid(programa.Attributes["ua_programas_por_campus_asesorid"].ToString());

            //}
            if (idp == default(Guid))
                //throw new LookupException("El campus " + pCampus + " y el id programa " + pPrograma + " no existe en el catálogo");
                throw new LookupException("No existe una combinación para el programa " + pPrograma + ", campus " + pCampus + " y la VPDI " + vpd.Name + " dentro del catálogo Programa por Campus Asesor.");
            return new EntityReference(ua_programas_por_campus_asesor.EntityLogicalName, idp);

        }


        private Guid RetrieveCuentaId(string idBanner)
        {
            Guid resultado = default(Guid);

            QueryExpression Query = new QueryExpression("account")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "accountid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal, idBanner)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                resultado = new Guid(prospecto.Attributes["accountid"].ToString());

            }
            return resultado;

        }

        private string RetrieveBannerCuentaID(Guid idCuenta)
        {

            string idBanner = "";
            Account c = new Account();

            QueryExpression Query = new QueryExpression(Account.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner" }),
                Criteria = {
                    Conditions = {

                        new ConditionExpression("accountid", ConditionOperator.Equal, idCuenta)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                idBanner = prospecto.Attributes["ua_idbanner"].ToString();


            }
            else
                throw new Exception("La cuenta " + idCuenta.ToString() + "no fue encontrada");
            return idBanner;

        }

        /// <summary>
        /// Metodo que recibe el Id banner y retorna un Guid correspondiente a opportunityid
        /// </summary>
        /// <param name="idBanner"></param>
        /// <returns></returns>
        public Guid RetrieveOpportunityId(string idBanner)
        {
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "opportunityid" }),
                Criteria = {
                    Conditions = {

                        new ConditionExpression("rs_idbanner", ConditionOperator.Equal, idBanner),
                         new ConditionExpression("statuscode", ConditionOperator.NotEqual, 717810002)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                resultado = new Guid(prospecto.Attributes["opportunityid"].ToString());

            }
            return resultado;

        }


        private List<Guid> RetrieveOpportunityByVPD(string idBanner, string VPD)
        {
            Opportunity op = new Opportunity();


            if (VPD == null)
                throw new LookupException("VPDI es requerido para recuperar OpportnityId");
            List<Guid> lstRes = new List<Guid>();
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "opportunityid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal, idBanner),
                        //new ConditionExpression("rs_idbanner", ConditionOperator.Equal, idBanner),
                        new ConditionExpression("statecode", ConditionOperator.Equal, 0),
                         new ConditionExpression("ua_codigo_vpd", ConditionOperator.Equal,VPD ),
                       // new ConditionExpression("ua_codigo_vpd_pl", ConditionOperator.Equal, VPD.Id)
                        //new ConditionExpression("rs_vpdid", ConditionOperator.Equal, VPD.Id)
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
                throw new Exception("No se encontró ninguna oportunidad que coincida con el idbanner y VPDI proporcionados");
            }
            return lstRes;
        }

        private Guid RetrieveDireccionId(Guid IdProspecto, Guid VPDI, Guid TipoDireccion, int SecuenciaDir)
        {
            Guid resultado = default(Guid);
            QueryExpression Query = new QueryExpression("EntidadDireccion")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "rs_direccionid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("rs_solicitante", ConditionOperator.Equal, IdProspecto),
                        new ConditionExpression("rs_vpdid", ConditionOperator.Equal, VPDI),
                        new ConditionExpression("rs_tipodireccionid", ConditionOperator.Equal, TipoDireccion),
                        new ConditionExpression("rs_secuenciadireccion", ConditionOperator.Equal, SecuenciaDir)
                    }
                }
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var direccion = ec.Entities.FirstOrDefault();
                resultado = new Guid(direccion.Attributes["rs_direccionid"].ToString());

            }
            return resultado;

        }

        private void ChangeRecordStatus(string EntityName, Guid RecordId, int status)
        {
            SetStateRequest setStateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference(EntityName, RecordId),
                State = new OptionSetValue(0),//1=disabled, 0=enabled
                Status = new OptionSetValue(status), //-1
            };
            _xrmServerConnection.Execute(setStateRequest);

        }

        private EntityReference RetrieveAsesor(Guid prospectoId)
        {
            EntityReference resultado = default(EntityReference);

            var ec = _xrmServerConnection.Retrieve("EntidadProspecto.logicaname", prospectoId, new ColumnSet(new string[] { "ownerid" }));
            if (ec != null && ec.Id != Guid.Empty)
            {

                resultado = ec.GetAttributeValue<EntityReference>("ownerid");

            }
            return resultado;

        }
        private EntityReference RetrieveContactoId(Guid prospectoId)
        {
            EntityReference resultado = default(EntityReference);

            var ec = _xrmServerConnection.Retrieve("RS_Prospecto.EntityLogicalName", prospectoId, new ColumnSet(new string[] { "rs_contactoid" }));
            if (ec != null && ec.Id != Guid.Empty)
            {

                resultado = ec.GetAttributeValue<EntityReference>("rs_contactoid");

            }
            return resultado;

        }


        private Dictionary<string, string> GetAllOption()
        {
            Dictionary<string, string> dicn = new Dictionary<string, string>();
            //ua_colegios co = new ua_colegios();
            //ua_periodo p = new ua_periodo();
            //p.ua_Tipo_Periodo
            // Create the request
            // LogicalName = "ua_colegio_tipo_colegio",
            RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
            {

                EntityLogicalName = ua_periodo.EntityLogicalName,
                LogicalName = "ua_tipo_periodo",
                RetrieveAsIfPublished = true
            };

            // Execute the request
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
                        dicn.Add(sNameOpt, valorOpt.ToString());
                        conOp++;
                    }

                }
            }

            // Use RetrieveAllOptionSetsRequest to retrieve all global option sets.
            // Create the request.

            RetrieveAllOptionSetsRequest retrieveAllOptionSetsRequest =
                new RetrieveAllOptionSetsRequest();

            // Execute the request
            RetrieveAllOptionSetsResponse retrieveAllOptionSetsResponse =
                (RetrieveAllOptionSetsResponse)_xrmServerConnection.Execute(
                retrieveAllOptionSetsRequest);
            Dictionary<string, string> optSet = new Dictionary<string, string>();
            // Now you can use RetrieveAllOptionSetsResponse.OptionSetMetadata property to 
            // work with all retrieved option sets.
            if (retrieveAllOptionSetsResponse.OptionSetMetadata.Count() > 0)
            {
                Console.WriteLine("All the global option sets retrieved as below:");
                int count = 1;
                foreach (OptionSetMetadataBase optionSetMetadata in
                    retrieveAllOptionSetsResponse.OptionSetMetadata)
                {
                    //    Console.WriteLine("{0} {1}", count++,
                    //        (optionSetMetadata.DisplayName.LocalizedLabels.Count > 0) ? optionSetMetadata.DisplayName.LocalizedLabels[0].Label : String.Empty);
                    if (optionSetMetadata.DisplayName.LocalizedLabels.Count > 0)
                        if (!optSet.ContainsKey(optionSetMetadata.DisplayName.LocalizedLabels[0].Label))
                            optSet.Add(optionSetMetadata.DisplayName.LocalizedLabels[0].Label, optionSetMetadata.DisplayName.LocalizedLabels[0].LanguageCode.ToString());
                    //else
                    //optSet.Add("Nada", "Nada");

                    // (optionSetMetadata.DisplayName.LocalizedLabels.Count > 0) ? optSet.Add(count.ToString(), optionSetMetadata.DisplayName.LocalizedLabels[0].Label) : optSet.Add(count.ToString(), "NAda");

                }
            }
            return optSet;
        }
        enum TipoColegio { H, C, S };
        enum TipoPerodo : int { Anual = 953880000, Cuatrimestral = 953880001, Intensivo = 953880005, Semestral = 953880002, Trimestral = 953880003, Verano = 953880004, S = 953880006, T = 953880007 }
    }
    #endregion

}

