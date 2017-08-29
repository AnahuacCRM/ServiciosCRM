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
    public class WebApiCambioSGASTDN_21
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvCambioSGASTDNController _servCambioSGASTDN;

        CambioSGASTDN entity;

        [TestInitialize]
        public void Inittializate()
        {
            _logger = new DebugerLogger();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _servCambioSGASTDN = new srvCambioSGASTDNController(_logger, _prospectprocessor);

            _servCambioSGASTDN.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            entity = new CambioSGASTDN()
            {
                id_Oportunidad = new System.Guid("b48b4382-3372-e711-8113-e0071b66bfc1"),
                Programa = "LC-DEEN-16",
                Campus = "UAN",
                Escuela = "CC"
            };
        }

        [TestMethod]
        public void CambioSGASTDN_21()
        {
            var resultado = _servCambioSGASTDN.ActualizaSGASTDN(entity);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
