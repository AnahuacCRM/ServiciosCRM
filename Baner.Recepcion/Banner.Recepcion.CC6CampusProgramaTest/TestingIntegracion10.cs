using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Web.Http.Hosting;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using Rhino.Crm2016Connector;

namespace Banner.Recepcion.CC6CampusProgramaTest
{
    [TestClass]
    public class TestingIntegracion10
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateCambiaSolicitudAdmisionController _updateCambiaSolicitudAdmisionController;
       
        #endregion
        CambiaSolicitudAdmision cambiasolicitudadmision;

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
            _updateCambiaSolicitudAdmisionController = new UpdateCambiaSolicitudAdmisionController(_logger, _prospectprocessor);

            _updateCambiaSolicitudAdmisionController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            #region Informacion de Cambia Solicitud Admision
            cambiasolicitudadmision = new CambiaSolicitudAdmision()
            {
                OportunidadIdCRM = new Guid("3419a5cc-5e70-e611-80f8-6c3be5a8e1ac"),
                IdBanner = "00299822",
                Programa = "LC-DERE-16",
                Periodo = "201810",
                VPDI = "UAM",
                Campus = "UAM",
                Escuela = "AC",
                NumeroSolicitud = 1
            };
            #endregion

          

        }

        [TestMethod]
        public void TestIntegracion10()
        {

            //Arrange


            //Act
            var resultado = _updateCambiaSolicitudAdmisionController.Post(cambiasolicitudadmision);

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
