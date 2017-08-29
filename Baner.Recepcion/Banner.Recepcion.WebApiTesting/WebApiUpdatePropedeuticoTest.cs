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
using Newtonsoft.Json;
using System.Net;
using Baner.Recepcion.Services.Models;
using System.Configuration;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiUpdatePropedeuticoTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdatePropedeuticoController _updatePropedeuticoController;
        Token token;
        #endregion

        Propedeutico propedeutico;

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
            _updatePropedeuticoController = new UpdatePropedeuticoController(_logger, _prospectprocessor);

            _updatePropedeuticoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


           
            #region Informacion de Propedeutico
            propedeutico = new Propedeutico()
            {                
                IdBanner = "0213",
                NumeroSolicitud = 23,
                PeriodoPL = "201645",
                SolicitudAdmisionPL = "Y",
                DecisionAdmision = "AA",
                VPDI = "UAC",
                VPDIPL="UAS",
                CampusAdmisionPL = "UAC",
                ProgramaPL = "LC-ADMI-04",
                Periodo="111111"

            };
            #endregion

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


        #region Pruebas de Edición
        [TestMethod]
        public void UpdatefromURLPropedeuticoSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/UpdatePropedeutico";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(propedeutico);
            var response = proxy.PostAsJsonAsync(url, propedeutico).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }


        [TestMethod]
        public void UpdatefromControlerPropedeuticoSuccessTest()
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

        //[TestMethod]
        //public void PropedeuticoIDBANNERInexistenteSuccessTest()
        //{
        //    //Arrange

        //    propedeutico.IdBanner = "123";
        //    //Act
        //    var resultado = _updatePropedeuticoController.Post(propedeutico);

        //    //Assert

        //    if (resultado.StatusCode != HttpStatusCode.OK)
        //    {
        //        var msgresultado = resultado.Content.ReadAsStringAsync().Result;
        //        _logger.Infomacion(msgresultado);
        //        var expectedErrorMessage = "No se encontró ninguna oportunidad que coincida con el idbanner y VPDI proporcionados";


        //        var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
        //        var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
        //        var actualErrorMessage = httperrormessage.Message;

        //        //Assert
        //        Assert.AreEqual(false, resultado.IsSuccessStatusCode);

        //        Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        //    }
        //    Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
        //    Assert.AreNotEqual(false, resultado);   

        //}
        #endregion

    }
}
