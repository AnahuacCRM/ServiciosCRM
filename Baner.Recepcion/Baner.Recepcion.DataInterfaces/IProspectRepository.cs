using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataInterfaces
{
    public interface IProspectRepository
    {
        void Conexion2(string svpd);
        ResponseNewProspect Create(NewProspect prospect);

        ResponseNewProspect CreateCuenta(Cuenta prospect);
        ResponseNewProspect AltaOportunidad(Oportunidad pOportunidad);

        bool UpdateExaminado(Examinado examinado);
        bool UpdateInscrito(Inscrito inscrito);
        bool FechaExamenAdmision(FechaExamenAdmision FechaExamenAdmision);
        bool UpdateAdmitido(Admitido admitido);


        bool UpdateResultadoExamen(ResultadoExamen resultado);

        bool UpdatePropedeutico(Propedeutico propedeutico);
        bool UpdateExaminadoPI(ExaminadoPI examinadoPI);
        bool UpdateRechazado(Rechazado rechazado);
        Guid CreateSolicitaBeca(SolicitaBeca solicitaBeca);
        Guid CreatePreUniversitario(PreUniversitario preUniversitario);
        Guid CreateOtorgaCredito(OtorgaCredito otorgaCredito);
        bool UpdateTransferencia(Transferencia transferencia);
        bool UpdateCambioSGASTDN(CambioSGASTDN cambio);


        bool OtorgamientoaBeca(OtorgamientoBeca OtorgaBeca);

        bool UpdateNuevoIngreso(NuevoIngreso nuevoingreso);

        bool UpdateCambiosTipoAdmision(CambiosTipoAdmision cambiotipoadmision);

        bool UpdateDatosPersona(DatosPersona datospersona);

        bool UpdateCambiaSolicitudAdmision(CambiaSolicitudAdmision cambiaSolicitudAdmision);

        bool UpdateDatosPrepa(DatosPrepa datosPrepa);

        bool UpdateCatalogoPeriodos(CatalogoPeriodos catalogoPeriodos);
        bool UpdateCatalogoColegios(CatalogoColegios catalogoColegios);

        bool UpdateEstatusAlumno(EstatusAlumno estatusalumno);
        bool CambiaTelefono(CambiaTelefono cambiatelefono);

        Guid RetrieveOpportunityId(string idBanner);


        bool UpdateCambiaDomicilio(CambiaDomicilio domicilio);
        bool UpdateCambiaEmail(CambiaEmail cambiaEmail);

        bool MarcarTransferido(MarcaTransferido transferido);


        bool CambioFaseSolicitante(CambioFaseSolicitante cambiosolicitante);


        GestionContactosWarning GestionContactos(Parentesco parentesco);

        List<Pais> GetCatalogoPaises();

        List<Estado> GetCatalogoEstados();

        List<Municipios> GetCatalogoMunicipios();

        List<Colegios> GetCatalogoColegios();

        Guid FormulariosBecarios(Becario becario);

        List<RepOportunidades> ObtenerDatosReport();

    }
}
