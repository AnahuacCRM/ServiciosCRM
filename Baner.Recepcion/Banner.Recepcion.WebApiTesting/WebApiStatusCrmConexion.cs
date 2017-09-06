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

        srvStatusConexionCRMController _SrvStatusCrm;

        [TestInitialize]
        public void Initializate()
        {
            _SrvStatusCrm = new srvStatusConexionCRMController();
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
               
            }

        }
    }
}
