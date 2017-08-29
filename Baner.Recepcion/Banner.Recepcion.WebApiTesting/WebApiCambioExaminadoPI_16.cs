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
    public class WebApiCambioProcesoIncompleto_16
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvCambioProcesoIncompletoController _servCambioProcesoIncompleto;

        ExaminadoPI entity;

        [TestInitialize]
        public void Inittializate()
        {
            _logger = new DebugerLogger();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _servCambioProcesoIncompleto = new srvCambioProcesoIncompletoController(_logger, _prospectprocessor);

            _servCambioProcesoIncompleto.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            entity = new ExaminadoPI("e975b76f-a22a-e711-8117-e0071b6a8131", "PI");
        }

        [TestMethod]
        public void CambioProcesoIncompleto_16()
        {
            var resultado = _servCambioProcesoIncompleto.ActualizarProcesoIncompleto(entity);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
