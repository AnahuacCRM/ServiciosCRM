using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataInterfaces
{
    public interface IPickListRepository
    {
        Dictionary<int, string> ListaEstatusSolicitud();

        Dictionary<int, string> ListaSexo();

        Dictionary<int, string> ListaEstadoCivil();

        Dictionary<int, string> ListaTelefonoPreferido();

        Dictionary<int, string> ListaCorreoPreferido();

        Dictionary<int, string> ListaTipoAlumno();

        Dictionary<int, string> ListaEstatusAlumno();

        Dictionary<int, string> ListaTipoAdmision();

        Dictionary<int, string> ListaTipoDesicionAdmision();

        Dictionary<int, string> ListaParentesco();

        Dictionary<int, string> ListaVive();

        Dictionary<int, string> ListTipoAdmision(string Entidad, string Atributo);

        // Dictionary<int, string> ListaDIreccionPreferido();

        Dictionary<int, string> ListOrigen(string Entidad, string Atributo);

        Dictionary<int, string> ListaTipoBeca(string Entidad, string Atributo);
        Dictionary<int, string> ListaTipoColegio();
        Dictionary<int, string> ListaTipoContacto();


    }
}
