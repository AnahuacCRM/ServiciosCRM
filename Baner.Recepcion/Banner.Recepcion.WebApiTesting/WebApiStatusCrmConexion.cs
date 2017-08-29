using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.DataLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiStatusCrmConexion
    {

        ILogger _logger;
        IMonitoreoConexion _MonitoreoConexion;
        IMonitoreoProcessor _MonitoreoProcessor;
       
        srvStatusConexionCRMController _SrvStatusCrm;

        [TestInitialize]
        public void Initializate()
        {
            _logger = new DebugerLogger();
            //_catalogrepository = new CatalogRepository();
            //_picklistrepository = new PickListRepository();
            //opportunityRepository = new OpportunityRepository();
            //_prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _MonitoreoConexion = new MonitorearConexionCrm();
            //_prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _MonitoreoProcessor = new MonitoreoProcessor(_logger, _MonitoreoConexion);
            //_servCambioExaminado = new srvCambioExaminadoController(_logger, _prospectprocessor);
            _SrvStatusCrm = new srvStatusConexionCRMController(_logger, _MonitoreoConexion);
            _SrvStatusCrm.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

        }

        [TestMethod]
        public void StatusCRM()
        {
            var resultado = _SrvStatusCrm.StatusCrmConexion();
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

        }
    }
}
