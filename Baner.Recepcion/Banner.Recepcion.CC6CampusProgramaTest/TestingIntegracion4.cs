using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.BusinessInterfaces;
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
    public class TestingIntegracion4
    {

        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateAdmitidoController _admitidocontroller;
        #endregion

        Admitido admitido;

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
            _admitidocontroller = new UpdateAdmitidoController(_logger, _prospectprocessor);

            _admitidocontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Fecha de Examen de Admision
            admitido = new Admitido()
            {
                OportunidadIdCRM = new Guid("32d035fe-d76f-e611-80f8-6c3be5a8e1ac"),
                IdBanner = "00299820",
                NumeroSolicitud = 99,
                DesicionAdmision = "AA",
                Programa = "LC-DERE-16",
                Periodo = "201810",
                VPDI = "UAM",
                Campus = "UAM",
                Escuela = "AC",
                TipoAdmision = "AA",
                PuntualizacionSobresaliente = "Y"
            };
            #endregion

        }

        [TestMethod]
        public void TestIntegracion4()
        {
            //Arrange


            //Act
            var resultado = _admitidocontroller.Post(admitido);

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
