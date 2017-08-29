using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using Baner.Recepcion.BusinessTypes;
using System.Net;

namespace Banner.Recepcion.CC6CampusProgramaTest
{
    [TestClass]
    public class TestingIntegracion3
    {

        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateExaminadoController _updateexaminadocontroller;
        //Token token;

        Examinado examinado = default(Examinado);
        #endregion

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
            _updateexaminadocontroller = new UpdateExaminadoController(_logger, _prospectprocessor);

            _updateexaminadocontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            #region Informacion de Examinado
            examinado = new Examinado()
            {
                OportunidadIdCRM = new Guid("8f4ae500-5d70-e611-80f8-6c3be5a8e1ac"),
                IdBanner = "00299821",
                NumeroSolicitud = 1,
                Programa = "LC-DERE-16",
                PromedioPreparatoria = "8.3",
                Periodo = "201810",
                TipoAlumno = "S",
                VPDI = "UAM",
                Campus="UAM",
            };
            
            #endregion


        }


        [TestMethod]
        public void TestIntegracion3()
        {
            //Act
            var resultado = _updateexaminadocontroller.Post(examinado);

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
