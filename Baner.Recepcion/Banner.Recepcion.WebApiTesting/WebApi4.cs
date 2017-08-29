using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApi4
    {
        srvCambioAdmitidoController srvAdmitido;
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;

        Admitido admitido;

        [TestInitialize]
        public void Initializate()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            srvAdmitido = new srvCambioAdmitidoController(_logger, _prospectprocessor);

            srvAdmitido.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
        }
        [TestMethod]
        public void AdmitidoController()
        {
            admitido = new Admitido
            {
                id_oportunidad = "34afb93b-942a-e711-8114-e0071b669e71",
                Campus = "UAN",
                Escuela = "EN",
                Programa = "LC-DER-16",
                DesicionAdmision = "no",
                TipoAdmision = "2"
            };

            var res = srvAdmitido.ActualizaAdmitido(admitido);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
