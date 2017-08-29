using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.Services.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Baner.Recepcion.Services.Models;
using System.Configuration;
using System.Net.Http.Headers;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiPreUniversitarioTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        CreatePreUniversitarioController _preUniversitariocontroller;
        Token token;
        #endregion
        PreUniversitario preUniversitario;

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
            _preUniversitariocontroller = new CreatePreUniversitarioController(_logger, _prospectprocessor);

            _preUniversitariocontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de PreUniversitario
            preUniversitario = new PreUniversitario()
            {
                Nombre = "hector",
                SegundoNombre = "",
                ApellidoPaterno = "solis",
                ApellidoMaterno = "",
                TelefonoLada = "45",
                TelefonoNumero = "45454545",
                CorreoElectronico = "hector@yahoo.mx",
                Nivel = "LC",
                Codigo = "9",
                Descripcion = "Derecho",
                Campus = "UAN",
                Estado = "M18",
                //Estado = "COA",
                //Municipio = "15121",
                Municipio = "18017",//apaseo el alto
                //Municipio = "1419",
                OtroEstado = "",
                Origen = "W",
                SubOrigen = "SubOrigen",
                VPD = "UAN"
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
        public void URLPreUniversitarioSucessTest()
        {
            //Arrange
            var url = "http://localhost:21292/Api/CreatePreUniversitario";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(preUniversitario);
            var response = proxy.PostAsJsonAsync(url, preUniversitario).Result;
            var result = response.Content.ReadAsAsync<Guid>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void PreUniversitarioControlerSuccessTest()
        {
            //Arrange

           
            //Act
            var resultado = _preUniversitariocontroller.Post(preUniversitario);

            //Assert
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);

        }

        [TestMethod]
        public void PreUniversitarioCampusErroneoControlerSuccessTest()
        {
            //Arrange

            preUniversitario.Campus = "321";
            //Act
            var resultado = _preUniversitariocontroller.Post(preUniversitario);

            //Assert
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
                var expectedErrorMessage = "No se pudo resolver el Lookup de Campus: 321";


                var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
                var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
                var actualErrorMessage = httperrormessage.Message;

                //Assert
                Assert.AreEqual(false, resultado.IsSuccessStatusCode);
                
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
            }
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);

        }

        [TestMethod]
        public void PreUniversitarioCodigoCarreraInvalidoControlerSuccessTest()
        {
            //Arrange

            preUniversitario.Codigo = "321";
            //preUniversitario.Campus = "321";
            //Act
            var resultado = _preUniversitariocontroller.Post(preUniversitario);

            //Assert
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
                var expectedErrorMessage = "La carreara web es requerida para recuperar el programa";


                var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
                var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
                var actualErrorMessage = httperrormessage.Message;

                //Assert
                Assert.AreEqual(false, resultado.IsSuccessStatusCode);

                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
            }
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);

        }


        #endregion
    }
}
