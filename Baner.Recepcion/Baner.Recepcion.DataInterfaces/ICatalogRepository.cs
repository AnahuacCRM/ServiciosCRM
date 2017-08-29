using Baner.Recepcion.BusinessTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataInterfaces
{
    public interface ICatalogRepository
    {
        List<Colonia> ListaColonias(Colonia colonia);

        List<Municipio> ListaMunicipio(Municipio municipio);
        Dictionary<string, string> GetIdMunicipio(string sClaveDom);

        Dictionary<string, string> GetMunicipioAsesor(string sCalveMunicipio);

        Dictionary<string, string> ListaEstado();

        Dictionary<string, string> ListaCodigoEstado();
        Dictionary<string, string> ListaPais();
        Dictionary<string, string> ListaCarreraWebCodigo();
        Dictionary<string, string> ListaCampus();

        Dictionary<string, string> ListaNiveles();
         

        Dictionary<string, string> ListaNacionalidad();

        Dictionary<string, string> ListaColegio();

        Dictionary<string, string> ListaPeriodo();
        

        Dictionary<string, string> ListaReligion();

        Dictionary<string, string> ListaEscuela();

        Dictionary<string, string> ListaPrograma();

        


        Dictionary<string, string> ConjutoOpciones(string EntityLoginame, string sCampoDeConjunto);




    }
}
