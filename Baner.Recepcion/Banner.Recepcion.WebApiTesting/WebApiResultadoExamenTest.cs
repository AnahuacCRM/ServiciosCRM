using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.Services.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using System.Net;
using Baner.Recepcion.Services.Models;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Banner.Recepcion.WebApiTesting
{
    /// <summary>
    /// Summary description for WebApiResultadoExamenTest
    /// </summary>
    [TestClass]
    public class WebApiResultadoExamenTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateResultadoExamenController _ResultadoExamenController;
        Token token;
        #endregion

        ResultadoExamen resultadoexamen;

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
            _ResultadoExamenController = new UpdateResultadoExamenController(_logger, _prospectprocessor);
            _ResultadoExamenController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
            resultadoexamen = new ResultadoExamen();
            resultadoexamen.IdBanner = "0213";
            resultadoexamen.VPDI = "UAC";
            var r = new InformacionResultado();
            r.BanderaScore = "Y";
            r.CodigoExamen = "XX";
            r.FechaResultado = new CustomDate()
            {
                Year = 2016,
                Month = 06,
                Day = 1
            };
            
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);




            /*
                 * En codigo Examen Vendra:
                 * PAAN
                 * PAAV
                 * PARA
                 * MMPI
                 * ESCM
                 * ESCP
                 */
            ArrangeSecurity();

        }
        private void ArrangeSecurity()
        {
            var Uri = "http://localhost:21292/";
            var client = new HttpClient();
            var secretrhino = ConfigurationManager.AppSettings["secret"];

            //Generacion del Token
            var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Rhino:{0}", secretrhino)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);

            var usr = ConfigurationManager.AppSettings["oauthusr"];

            var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", usr},
               };
            //Recuperacion del Token
            var tokenResponse = client.PostAsync(Uri + "o/Server", new FormUrlEncodedContent(form)).Result;

            var resultado = tokenResponse.Content.ReadAsStringAsync().Result;
            token = JsonConvert.DeserializeObject<Token>(resultado);
        }

        #region Pruebas Satisfactorias

        [TestMethod]
        public void UpdatefromURLResultadoExamenPAANSuccessTest()
        {
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "PAAN";
            //Arrange
            var url = "http://localhost:21292/Api/UpdateResultadoExamen";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(resultadoexamen);
            var response = proxy.PostAsJsonAsync(url, resultadoexamen).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);

        }


        [TestMethod]
        public void ResultadoExamenPAANSuccessTest()
        {
            //Arrange
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "PAAN";
            //Act
            var resultado = _ResultadoExamenController.Post(resultadoexamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }

        [TestMethod]
        public void ResultadoExamenAAVSuccessTest()
        {
            //Arrange
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "PAAV";
            //Act
            var resultado = _ResultadoExamenController.Post(resultadoexamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }
        [TestMethod]
        public void ResultadoExamenPARASuccessTest()
        {
            //Arrange
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "PARA";
            //Act
            var resultado = _ResultadoExamenController.Post(resultadoexamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }

        [TestMethod]
        public void ResultadoExamenMMPISuccessTest()
        {
            //Arrange
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "MMPI";
            //Act
            var resultado = _ResultadoExamenController.Post(resultadoexamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }

        [TestMethod]
        public void ResultadoExamenESCMSuccessTest()
        {
            //Arrange
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "ESCM";
            //Act
            var resultado = _ResultadoExamenController.Post(resultadoexamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }

        [TestMethod]
        public void ResultadoExamenESCPSuccessTest()
        {
            //Arrange
            resultadoexamen.ResultadosdeExamen[0].CodigoExamen = "ESCP";
            //Act
            var resultado = _ResultadoExamenController.Post(resultadoexamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }
        #endregion




    }
}
