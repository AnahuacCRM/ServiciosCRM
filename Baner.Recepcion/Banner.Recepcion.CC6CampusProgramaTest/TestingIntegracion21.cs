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

namespace Banner.Recepcion.CC6CampusProgramaTest
{
    [TestClass]
    public class TestingIntegracion21
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateCambioSGASTDNController _updateCambioSGASTDNController;
        
        #endregion

        CambioSGASTDN cambio;

        [TestInitialize]
        public void Initialize()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository( _catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _updateCambioSGASTDNController = new UpdateCambioSGASTDNController(_logger, _prospectprocessor);

            _updateCambioSGASTDNController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de CambioSGASTDN
            cambio = new CambioSGASTDN()
            {
                OportunidadIdCRM = new Guid("8698d456-6170-e611-80f8-6c3be5a8e1ac"),
                IdBanner = "00299826",
                Periodo = "201810",
                Programa = "LC-DERE-16",
                Campus = "UAM",
                Escuela = "AS",
                VPDI = "UAM"


            };
            #endregion
            

        }
        [TestMethod]
        public void TestIntegracion21()
        {
            //Arrange

            //Act
            var resultado = _updateCambioSGASTDNController.Post(cambio);

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
