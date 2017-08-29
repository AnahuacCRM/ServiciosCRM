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
using System.Configuration;
using System.Text;
using System.Net.Http.Headers;
using Baner.Recepcion.Services.Models;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCambiaTelefonoTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        CambiaTelefonoController _cambiaTelefonoController;
        Token token;
        #endregion

        CambiaTelefono cambiaTelefono;

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
            _cambiaTelefonoController = new CambiaTelefonoController(_logger, _prospectprocessor);

            _cambiaTelefonoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de CambiaTelefono


            cambiaTelefono = new CambiaTelefono();

            cambiaTelefono.IdBanner = "00019990";
            var lst_CambiaTelefonos = new ListaCambiaTelefonos();
            lst_CambiaTelefonos.VPDI = "UAC";
            lst_CambiaTelefonos.TipoTelefono = "PD";
            lst_CambiaTelefonos.SecuenciaTelefono = 16;
            lst_CambiaTelefonos.TipoOperacion = "I";
            lst_CambiaTelefonos.TelefonoArea = "099";
            lst_CambiaTelefonos.Telefono = "5512122365";
            lst_CambiaTelefonos.TelefonoPreferido = "Y";

            var lst_CambiaTelefonos2 = new ListaCambiaTelefonos();
            lst_CambiaTelefonos2.VPDI = "UAN";
            lst_CambiaTelefonos2.TipoTelefono = "EM";
            lst_CambiaTelefonos2.SecuenciaTelefono = 2;
            lst_CambiaTelefonos2.TipoOperacion = "I";
            lst_CambiaTelefonos2.TelefonoArea = default(string); 
            lst_CambiaTelefonos2.Telefono = "666666256";
            lst_CambiaTelefonos2.TelefonoPreferido = default(string);

            cambiaTelefono.lstInformacionTelefonos = new List<ListaCambiaTelefonos>();
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos);
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos2);

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
        public void InsertfromURLCambiaTelefonoSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/CambiaTelefono";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(cambiaTelefono);
            var response = proxy.PostAsJsonAsync(url, cambiaTelefono).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }
      
        [TestMethod]
        public void InsertfromControllerCambiaTelefonoSuccessTest()
        {
            //Arrange



            //Act
            var resultado = _cambiaTelefonoController.Post(cambiaTelefono);

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
        public void UpdatefromControllerCambiaTelefonoSuccessTest()
        {
            //Arrange
            #region Informacion de CambiaTelefono



            cambiaTelefono.IdBanner = "00019990";
            var lst_CambiaTelefonos = new ListaCambiaTelefonos();
            lst_CambiaTelefonos.VPDI = "UAC";
            lst_CambiaTelefonos.TipoTelefono = "PD";
            lst_CambiaTelefonos.SecuenciaTelefono = 16;
            lst_CambiaTelefonos.TipoOperacion = "U";
            lst_CambiaTelefonos.TelefonoArea = "52";
            lst_CambiaTelefonos.Telefono = "1111111111";
            lst_CambiaTelefonos.TelefonoPreferido = "Y";

            var lst_CambiaTelefonos2 = new ListaCambiaTelefonos();
            lst_CambiaTelefonos2.VPDI = "UAN";
            lst_CambiaTelefonos2.TipoTelefono = "EM";
            lst_CambiaTelefonos2.SecuenciaTelefono = 2;
            lst_CambiaTelefonos2.TipoOperacion = "U";
            lst_CambiaTelefonos2.TelefonoArea = "52";
            lst_CambiaTelefonos2.Telefono = "2222222222";
            lst_CambiaTelefonos2.TelefonoPreferido = default(string);

            cambiaTelefono.lstInformacionTelefonos = new List<ListaCambiaTelefonos>();
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos);
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos2);


            #endregion


            //Act
            var resultado = _cambiaTelefonoController.Post(cambiaTelefono);

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
        public void DeletefromControllerCambiaTelefonoSuccessTest()
        {
            //Arrange
            #region Informacion de CambiaTelefono


            cambiaTelefono = new CambiaTelefono();

            cambiaTelefono.IdBanner = "00019990";
            var lst_CambiaTelefonos = new ListaCambiaTelefonos();
            lst_CambiaTelefonos.VPDI = "UAC";
            lst_CambiaTelefonos.TipoTelefono = "PD";
            lst_CambiaTelefonos.SecuenciaTelefono = 16;
            lst_CambiaTelefonos.TipoOperacion = "D";
            lst_CambiaTelefonos.TelefonoArea = "099";
            lst_CambiaTelefonos.Telefono = "5512122365";
            lst_CambiaTelefonos.TelefonoPreferido = "Y";
            var lst_CambiaTelefonos2 = new ListaCambiaTelefonos();
            lst_CambiaTelefonos2.VPDI = "UAN";
            lst_CambiaTelefonos2.TipoTelefono = "EM";
            lst_CambiaTelefonos2.SecuenciaTelefono = 2;
            lst_CambiaTelefonos2.TipoOperacion = "D";
            lst_CambiaTelefonos2.TelefonoArea = "052";
            lst_CambiaTelefonos2.Telefono = "666666256";
            lst_CambiaTelefonos2.TelefonoPreferido = "Y";

            cambiaTelefono.lstInformacionTelefonos = new List<ListaCambiaTelefonos>();
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos);
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos2);


            #endregion


            //Act
            var resultado = _cambiaTelefonoController.Post(cambiaTelefono);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);

        }
        #endregion

    }
}
