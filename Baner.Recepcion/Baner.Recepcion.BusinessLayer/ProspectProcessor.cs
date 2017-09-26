using Baner.Recepcion.BusinessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessTypes.Extensions;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Baner.Recepcion.OperationalManagement.Extensions;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;

namespace Baner.Recepcion.BusinessLayer
{
    public class ProspectProcessor : IProspectProcessor
    {
        private readonly ILogger _logger;
        private readonly IProspectRepository _prospectRepository;

        public ProspectProcessor(ILogger logger, IProspectRepository prospectRepository)
        {
            _logger = logger;
            _prospectRepository = prospectRepository;

        }

        public ResponseNewProspect Create(Cuenta prospect)
        {

            try
            { //Validar Modelo
                //Validar la fecha cuando no sea vacia

                prospect.Validate();

                #region Validar entidades relacionadas
                if (prospect.Direcciones != null)
                    prospect.Direcciones.ForEach(d => d.Validate());

                if (prospect.Telefonos != null)
                    prospect.Telefonos.ForEach(d => d.Validate());

                if (prospect.Correos != null)
                    prospect.Correos.ForEach(d => d.Validate());

                if (prospect.PadreoTutor != null)
                    prospect.PadreoTutor.ForEach(d => d.Validate());

                #endregion




                return _prospectRepository.CreateCuenta(prospect);
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }


        }

        public ResponseNewProspect AltaOportunidad(Oportunidad pOportunidad)
        {
            try
            {
                pOportunidad.Validate();

                return _prospectRepository.AltaOportunidad(pOportunidad);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateResultadoExamen(ResultadoExamen resultadoExamen)
        {
            bool resultado = false;

            try
            {
                resultadoExamen.Validate();

                if (resultadoExamen.ResultadosdeExamen != null)
                    resultadoExamen.ResultadosdeExamen.ForEach(d => d.Validate());

                if (resultadoExamen.ResultadosdeExamen != null)
                    resultadoExamen.ResultadosdeExamen.ForEach(d => d.FechaResultado.Validate());


                resultado = _prospectRepository.UpdateResultadoExamen(resultadoExamen);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Build());
                throw;
            }

            return resultado;
        }

        public bool UpdateExaminado(Examinado exam)
        {
            try
            {
                exam.Validate();
                return _prospectRepository.UpdateExaminado(exam);
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdatePropedeutico(Propedeutico propedeutico)
        {
            try
            {
                propedeutico.Validate();
                return _prospectRepository.UpdatePropedeutico(propedeutico);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateInscrito(Inscrito inscrito)
        {
            try
            {
                inscrito.Validate();
                if (inscrito.FechaPagoInscripcion != null)
                    inscrito.FechaPagoInscripcion.Validate();

                return _prospectRepository.UpdateInscrito(inscrito);
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool FechaExamenAdmision(FechaExamenAdmision FechaExamenAdmision)
        {
            try
            {
                // Se Valida el Modelo

                //if (FechaExamenAdmision.lstExamenes != null)
                //{
                //    foreach (var examen in FechaExamenAdmision.lstExamenes)
                //    {
                //        examen.FechaExamen.Validate();
                //        //examen.ClaveExamen.Validate();
                //    }

                //}

                FechaExamenAdmision.Validate();

                return _prospectRepository.FechaExamenAdmision(FechaExamenAdmision);
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateAdmitido(Admitido admitido)
        {
            try
            {
                // Se Valida el Modelo
                admitido.Validate();

                return _prospectRepository.UpdateAdmitido(admitido);
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateExaminadoPI(ExaminadoPI examinadoPI)
        {
            try
            {
                examinadoPI.Validate();
                return _prospectRepository.UpdateExaminadoPI(examinadoPI);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateRechazado(Rechazado rechazado)
        {
            try
            {
                rechazado.Validate();
                return _prospectRepository.UpdateRechazado(rechazado);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public Guid CreateSolicitaBeca(SolicitaBeca solicitaBeca)
        {
            try
            {
                solicitaBeca.Validate();
                #region Validar entidades relacionadas
                if (solicitaBeca.SolicitudBecas != null)
                {
                    solicitaBeca.SolicitudBecas.ForEach(b => b.Validate());

                    //foreach (var item in solicitaBeca.SolicitudBecas)//la fecha no es requerida pero cuando se envia hay que validar que tenga el formato correcto
                    //{
                    //    if (item.FechaSolicitudBeca != null)
                    //        solicitaBeca.SolicitudBecas.ForEach(b => b.FechaSolicitudBeca.Validate());
                    //}

                }




                #endregion
                return _prospectRepository.CreateSolicitaBeca(solicitaBeca);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public Guid CreatePreUniversitario(PreUniversitario preUniversitario)
        {
            try
            {
                // Se Valida el Modelo
                preUniversitario.Validate();
                //Asignar nuevo usuario creador por vpd
                _prospectRepository.Conexion2(preUniversitario.VPD);
                return _prospectRepository.CreatePreUniversitario(preUniversitario);
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool OtorgamientoaBeca(OtorgamientoBeca OtorgaBeca)
        {
            try
            {

                #region Validar objeto
                OtorgaBeca.Validate();

                if (OtorgaBeca.lstBeca != null)
                {
                    foreach (var item in OtorgaBeca.lstBeca)
                    {
                        //item.Beca.Validate();
                        if (item.Beca != null)
                            item.Beca.Validate();

                        if (item.FechaOtorgaBeca != null)
                            item.FechaOtorgaBeca.Validate();

                        if (item.FechaVencimientoBeca != null)
                            item.FechaVencimientoBeca.Validate();
                    }

                }

                #endregion
                return _prospectRepository.OtorgamientoaBeca(OtorgaBeca);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public Guid CreateOtorgaCredito(OtorgaCredito otorgaCredito)
        {
            try
            {
                otorgaCredito.Validate();
                #region Validar entidades relacionadas
                if (otorgaCredito.InfoCreditos != null)
                {
                    otorgaCredito.InfoCreditos.ForEach(c => c.Validate());

                    foreach (var item in otorgaCredito.InfoCreditos)//la fecha no es requerida pero cuando se envia hay que validar que tenga el formato correcto
                    {
                        if (item.FechaOtorgaCredito != null)
                            otorgaCredito.InfoCreditos.ForEach(b => b.FechaOtorgaCredito.Validate());
                    }

                }
                #endregion
                return _prospectRepository.CreateOtorgaCredito(otorgaCredito);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateTransferencia(Transferencia transferencia)
        {
            try
            {
                transferencia.Validate();
                return _prospectRepository.UpdateTransferencia(transferencia);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateCambioSGASTDN(CambioSGASTDN cambio)
        {
            try
            {
                cambio.Validate();
                return _prospectRepository.UpdateCambioSGASTDN(cambio);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateNuevoIngreso(NuevoIngreso nuevoingreso)
        {
            //bool res = false;
            try
            {

                #region Validar objeto

                nuevoingreso.Validate();

                #endregion

                return _prospectRepository.UpdateNuevoIngreso(nuevoingreso);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }

            //return res;
        }

        public bool UpdateCambiosTipoAdmision(CambiosTipoAdmision cambiotipoadmision)
        {
            bool res = false;
            try
            {

                #region Validar objeto

                cambiotipoadmision.Validate();

                #endregion

                res = _prospectRepository.UpdateCambiosTipoAdmision(cambiotipoadmision);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }

            return res;
        }

        public bool UpdateDatosPersona(DatosPersona datospersona)
        {
            bool res = false;
            try
            {

                #region Validar objeto

                datospersona.Validate();
                if (datospersona.Fecha_Nacimiento != null)
                    datospersona.Fecha_Nacimiento.Validate();

                #endregion

                res = _prospectRepository.UpdateDatosPersona(datospersona);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }

            return res;
        }

        public bool UpdateCambiaSolicitudAdmision(CambiaSolicitudAdmision cambiaSolicitudAdmision)
        {
            try
            {
                cambiaSolicitudAdmision.Validate();
                return _prospectRepository.UpdateCambiaSolicitudAdmision(cambiaSolicitudAdmision);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateDatosPrepa(DatosPrepa datosPrepa)
        {
            try
            {
                datosPrepa.Validate();
                return _prospectRepository.UpdateDatosPrepa(datosPrepa);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public GestionContactosWarning GestionContactos(Parentesco parentesco)
        {
            try
            {
                parentesco.Validate();
                #region Validar entidades relacionadas
                if (parentesco.PadreoTutor != null)
                    parentesco.PadreoTutor.ForEach(l => l.Validate());

                #endregion
                return _prospectRepository.GestionContactos(parentesco);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }
       
        public bool UpdateCatalogoPeriodos(CatalogoPeriodos catalogoPeriodos)
        {
            try
            {
                catalogoPeriodos.Validate();
                catalogoPeriodos.Fecha_de_Inicio_Periodo.Validate();
                catalogoPeriodos.Fecha_de_Fin_Periodo.Validate();
                catalogoPeriodos.Fecha_Inicio_Alojamiento.Validate();
                catalogoPeriodos.Fecha_Fin_Alojamiento.Validate();


                return _prospectRepository.UpdateCatalogoPeriodos(catalogoPeriodos);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateCatalogoColegios(CatalogoColegios catalogoColegios)
        {
            try
            {
                catalogoColegios.Validate();
                #region Validar entidades relacionadas
                //if (catalogoColegios.lstContactos != null)
                //    catalogoColegios.lstContactos.ForEach(l => l.Validate());

                #endregion

                return _prospectRepository.UpdateCatalogoColegios(catalogoColegios);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateEstatusAlumno(EstatusAlumno estatusalumno)
        {
            try
            {
                estatusalumno.Validate();
                return _prospectRepository.UpdateEstatusAlumno(estatusalumno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateCambiaDomicilio(CambiaDomicilio domicilio)
        {
            try
            {
                domicilio.Validate();
                if (domicilio.lstDomicilio != null)
                {
                    foreach (var item in domicilio.lstDomicilio)
                    {
                        if (item != null)
                        {
                            item.Validate();
                        }
                    }
                }

                return _prospectRepository.UpdateCambiaDomicilio(domicilio);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }
        public bool CambiaTelefono(CambiaTelefono cambiatelefono)
        {
            try
            {
                cambiatelefono.Validate();
                #region Validar entidades relacionadas
                if (cambiatelefono.lstInformacionTelefonos != null)
                    cambiatelefono.lstInformacionTelefonos.ForEach(c => c.Validate());

                #endregion
                return _prospectRepository.CambiaTelefono(cambiatelefono);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool UpdateCambiaEmail(CambiaEmail cambiaEmail)
        {
            try
            {
                cambiaEmail.Validate();
                #region Validar entidades relacionadas
                if (cambiaEmail.lstinfoCambiaEmails != null)
                    cambiaEmail.lstinfoCambiaEmails.ForEach(c => c.Validate());

                #endregion
                return _prospectRepository.UpdateCambiaEmail(cambiaEmail);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }
        }

        public bool MarcarTransferido(MarcaTransferido transferido)
        {
            try
            {
                return _prospectRepository.MarcarTransferido(transferido);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw;
            }

        }



        
        public bool CambioFaseSolicitante(CambioFaseSolicitante cambiosolicitante)
        {
            try
            {
                return _prospectRepository.CambioFaseSolicitante(cambiosolicitante);
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }

        public List<Pais> GetCatalogoPaises()
        {
            try
            {
                return _prospectRepository.GetCatalogoPaises();
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }

        public List<Estado> GetCatalogoEstados()
        {
            try
            {
                return _prospectRepository.GetCatalogoEstados();
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }
        public List<Municipios> GetCatalogoMunicipios()
        {
            try
            {
                return _prospectRepository.GetCatalogoMunicipios();
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }
        public List<Colegios> GetCatalogoColegios()
        {
            try
            {
                return _prospectRepository.GetCatalogoColegios();
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }

        public Guid FormulariosBecarios(Becario becario)
        {
            try
            {
                becario.Validate();
                //Asignar nuevo usuario creador por vpd
                _prospectRepository.Conexion2(becario.VPD);
                return _prospectRepository.FormulariosBecarios(becario);
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }


        public bool HaYRegistrosEnCuenta()
        {
            try
            {

                //Asignar nuevo usuario creador por vpd

                return _prospectRepository.HaYRegistrosEnCuenta();
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }

        }
        public List<RepOportunidades> ObtenerDatosReport()
        {
            try
            {
                return _prospectRepository.ObtenerDatosReport();
            }
            catch (Exception Ex)
            {
                _logger.Error(Ex.Build());
                throw;
            }
        }


        

    }
}
