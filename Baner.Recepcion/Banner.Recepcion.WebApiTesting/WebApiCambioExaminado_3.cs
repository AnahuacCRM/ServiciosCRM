using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.Services.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCambioExaminado_3
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvCambioExaminadoController _servCambioExaminado;
        srvCambioAdmitidoController _srvcambioadmitido;
        Examinado entity;

        [TestInitialize]
        public void Inittializate()
        {
            _logger = new DebugerLogger();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _servCambioExaminado = new srvCambioExaminadoController(_logger, _prospectprocessor);
            _srvcambioadmitido = new srvCambioAdmitidoController(_logger, _prospectprocessor);
            _servCambioExaminado.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            entity = new Examinado("a66c75a9-5a63-e711-811b-e0071b6a9211", "UAN", "LC-CFAM-16", "6.6", "N","N");
        }

        [TestMethod]
        public void CambioAExaminado_3()
        {
            var resultado = _servCambioExaminado.ActualizaAExaminado(entity);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
        

        [TestMethod]
        public void CambioAdmitidoB()
        {
            _srvcambioadmitido.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            var admi = new Admitido
            {
                id_oportunidad = "e787a773-163b-e711-810e-e0071b6a82a1",
                DesicionAdmision = "AA",
                Programa = "LC-ADTU-16",
                Campus = "UAM",
                Escuela = "AT",
                TipoAdmision = "AA",
               
               


            };
            var resultado = _srvcambioadmitido.ActualizaAdmitido(admi);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
