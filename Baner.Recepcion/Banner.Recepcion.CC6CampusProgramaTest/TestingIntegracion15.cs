using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;
using Rhino.Crm2016Connector;

namespace Banner.Recepcion.CC6CampusProgramaTest
{
    [TestClass]
    public class TestingIntegracion15
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdatePropedeuticoController _updatePropedeuticoController;
        
        #endregion

        Propedeutico propedeutico;

        [TestInitialize]
        public void Initialize()
        {
            _logger = new DebugerLogger();
            serverconnection = XRMServerConnection.GetInstance as IServerConnection;
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository( _catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _updatePropedeuticoController = new UpdatePropedeuticoController(_logger, _prospectprocessor);

            _updatePropedeuticoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };



            #region Informacion de Propedeutico
            propedeutico = new Propedeutico()
            {
                IdBanner = "00299823",
                NumeroSolicitud = 1,
                PeriodoPL = "201810",
                SolicitudAdmisionPL = "Y",
                DecisionAdmision = "AA",
                VPDI = "UAM",
                VPDIPL = "UAM",
                CampusAdmisionPL = "UAM",
                ProgramaPL = "LC-DERE-16",
                Periodo = "201810"

            };
            #endregion

            

        }

        [TestMethod]
        public void TestIntegracion15()
        {
            //Arrange

            //Act
            var resultado = _updatePropedeuticoController.Post(propedeutico);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);
        }
    }
}
