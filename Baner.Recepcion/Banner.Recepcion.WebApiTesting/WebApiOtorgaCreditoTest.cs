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
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using Baner.Recepcion.Services.Models;
using System.Configuration;
using System.Text;
using System.Net.Http.Headers;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiOtorgaCreditoTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        CreateOtorgaCreditoController _createOtorgaCreditoController;
        Token token;
        #endregion

        OtorgaCredito otorgaCredito;

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
            _createOtorgaCreditoController = new CreateOtorgaCreditoController(_logger, _prospectprocessor);

            _createOtorgaCreditoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };



            #region Informacion OtorgaCredito

            otorgaCredito = new OtorgaCredito();
            otorgaCredito.IdBanner = "00294870";
            #region InformacionBeca
            var infoCredito = new InformacionOtorgaCredito();


           
            infoCredito.DescripcionCredito = "credito academico";
            infoCredito.CampusVPDI = "UAC";
            infoCredito.Periodo = "111111";

            infoCredito.FechaOtorgaCredito = new CustomDate()
            {
                Day = 6,
                Month = 6,
                Year = 2016

            };
            #endregion
            #region InformacionBeca
            var infoCredito2 = new InformacionOtorgaCredito();


           
            infoCredito2.DescripcionCredito = "credito Academico";
            infoCredito2.CampusVPDI = "UAN";
            infoCredito2.Periodo = "196810";

            infoCredito2.FechaOtorgaCredito = new CustomDate()
            {
                Day = 7,
                Month = 7,
                Year = 2016

            };
            #endregion
            otorgaCredito.InfoCreditos = new List<InformacionOtorgaCredito>();
            otorgaCredito.InfoCreditos.Add(infoCredito);
            otorgaCredito.InfoCreditos.Add(infoCredito2);
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



        #region Pruebas de Creación
        [TestMethod]
        public void CreatefromURLOtorgaCreditoSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/CreateOtorgaCredito";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(otorgaCredito);
            var response = proxy.PostAsJsonAsync(url, otorgaCredito).Result;
            var result = response.Content.ReadAsAsync<Guid>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreNotEqual(Guid.Empty, result);
        }


        [TestMethod]
        public void CreatefromControllerOtorgaCreditoSuccessTest()
        {
            //Arrange

            //Act
            var resultado = _createOtorgaCreditoController.Create(otorgaCredito);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            Assert.AreNotEqual(Guid.Empty, resultado);

        }
        #endregion

    }
}
