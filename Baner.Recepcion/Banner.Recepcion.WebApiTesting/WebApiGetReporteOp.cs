using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiGetReporteOp
    {

        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;

        IOpportunityRepository opportunityRepository;
        srvObtenerReporteOportunidadesController srvobtenerreporte;

        #endregion
       // RepOportunidades preuniversitario;

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
            srvobtenerreporte = new srvObtenerReporteOportunidadesController(_logger, _prospectprocessor);

            srvobtenerreporte.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

        }
        [TestMethod]
        public void GetReporteOportunidad()
        {

            var datosR = srvobtenerreporte.ObtenerReporte();

            if (datosR.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = datosR.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, datosR.StatusCode);
            Assert.AreNotEqual(false, datosR);
        }
    }
}
