using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessInterfaces
{
    public interface IProspectProcessor
    {
        ResponseNewProspect Create(Cuenta prospect);
        ResponseNewProspect AltaOportunidad(Oportunidad pOportunidad);
        bool UpdateExaminado(Examinado exam);
        bool UpdateInscrito(Inscrito inscrito);
        bool FechaExamenAdmision(FechaExamenAdmision FechaExamenAdmision);
        bool UpdateAdmitido(Admitido admitido);

        bool UpdatePropedeutico(Propedeutico propedeutico);

        bool UpdateResultadoExamen(ResultadoExamen resultadoExamen);
        bool UpdateExaminadoPI(ExaminadoPI examinadoPI);
        bool UpdateRechazado(Rechazado rechazado);
        Guid CreateSolicitaBeca(SolicitaBeca solicitaBeca);
        Guid CreatePreUniversitario(PreUniversitario preUniversitario);
        Guid CreateOtorgaCredito(OtorgaCredito otorgaCredito);
        bool UpdateTransferencia(Transferencia transferencia);
        bool OtorgamientoaBeca(OtorgamientoBeca OtorgaBeca);
        bool UpdateCambioSGASTDN(CambioSGASTDN cambio);

        bool UpdateNuevoIngreso(NuevoIngreso nuevoingreso);

        bool UpdateCambiosTipoAdmision(CambiosTipoAdmision cambiotipoadmision);

        bool UpdateDatosPersona(DatosPersona datospersona);

        bool UpdateCambiaSolicitudAdmision(CambiaSolicitudAdmision cambiaSolicitudAdmision);

        bool UpdateDatosPrepa(DatosPrepa datosPrepa);

        
        GestionContactosWarning GestionContactos(Parentesco parentesco);
        bool UpdateCatalogoPeriodos(CatalogoPeriodos catalogoPeriodos);

        bool UpdateCatalogoColegios(CatalogoColegios catalogoColegios);


        bool UpdateEstatusAlumno(EstatusAlumno estatusalumno);
        bool CambiaTelefono(CambiaTelefono cambiatelefono);

        bool UpdateCambiaDomicilio(CambiaDomicilio domicilio);
        bool UpdateCambiaEmail(CambiaEmail cambiaEmail);

        //  bool MarcarTransferido(List<Guid> OportunidadesId);

        bool MarcarTransferido(MarcaTransferido transferido);



        bool CambioFaseSolicitante(CambioFaseSolicitante cambiosolicitante);


        List<Pais> GetCatalogoPaises();

        List<Estado> GetCatalogoEstados();

        List<Municipios> GetCatalogoMunicipios();

        List<Colegios> GetCatalogoColegios();

        Guid FormulariosBecarios(Becario becario);

        List<RepOportunidades> ObtenerDatosReport();



    }
}
