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
    public class WebApiCambioPropedeutico_15
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvCambioPropedeuticoController _servCambioPropedeutico;

        Propedeutico entity;

        [TestInitialize]
        public void Inittializate()
        {
            _logger = new DebugerLogger();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _servCambioPropedeutico = new srvCambioPropedeuticoController(_logger, _prospectprocessor);

            _servCambioPropedeutico.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            entity = new Propedeutico()
            {
                id_Oportunidad = "89791d61-a352-e711-8111-e0071b6a70f1",
                PeriodoPL = "201766",
                SolicitudAdmisionPL = 1,
                DecisionAdmision = "AD",
                VPDI = "UAX",
                CampusAdmisionPL = "UAX",
                ProgramaPL = "PL-PRCS-16"
            };
        }

        [TestMethod]
        public void CambioPropedeutico_15()
        {
            var resultado = _servCambioPropedeutico.ActualizarPropedeutico(entity);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
