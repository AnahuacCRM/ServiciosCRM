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
    public class WebApiCambioResultadoExamen_14
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvCambioResultadoExamenController _servCambioResultadoExamen;

        ResultadoExamen entity;

        [TestInitialize]
        public void Inittializate()
        {
            _logger = new DebugerLogger();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _servCambioResultadoExamen = new srvCambioResultadoExamenController(_logger, _prospectprocessor);

            _servCambioResultadoExamen.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            entity = new ResultadoExamen()
            {
                id_Cuenta = "721f0324-e981-e711-80ee-3863bb2e34c0",
                VPDI = "UAM",
                ResultadosdeExamen = new System.Collections.Generic.List<InformacionResultado>() {
                   new InformacionResultado("MMPI", new CustomDate(){ Year = 2017, Month = 9, Day = 4 }),
                    //new InformacionResultado("PARA", new CustomDate(){ Year = 2017, Month = 5, Day = 18 }),
                    //new InformacionResultado("PAAV", new CustomDate(){ Year = 2017, Month = 5, Day = 18 }),
                    //new InformacionResultado("PAAN", new CustomDate(){ Year = 2017, Month = 5, Day = 18 })
                }
            };
        }

        [TestMethod]
        public void CambioResultadoExamen_14()
        {
            var resultado = _servCambioResultadoExamen.ActualizaResultadoExamen(entity);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
