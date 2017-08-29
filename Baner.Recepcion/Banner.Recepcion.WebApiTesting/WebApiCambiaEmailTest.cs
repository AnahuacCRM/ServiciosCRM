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
    public class WebApiCambiaEmailTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateCambiaEmailController _updateCambiaEmailController;
        Token token;
        #endregion

        CambiaEmail cambiaEmail;

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
            _updateCambiaEmailController = new UpdateCambiaEmailController(_logger, _prospectprocessor);

            _updateCambiaEmailController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de CambiaEmail


            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";
            var infoCambiaEmail = new infoCambiaEmails();
            infoCambiaEmail.VPDI = "UAC";
            infoCambiaEmail.TipoCorreoElectronico = "FACE";
            infoCambiaEmail.SecuenciaCorreo = "AIIUDIJEDKMD/SDS";
            infoCambiaEmail.TipoOperacion = "I";
            infoCambiaEmail.CorreoElectronico = "loco15@hotmail.com";
            infoCambiaEmail.CorreoElectronicoIndPreferido = "Y";

            var infoCambiaEmail2 = new infoCambiaEmails();
            infoCambiaEmail2.VPDI = "UAN";
            infoCambiaEmail2.TipoCorreoElectronico = "CASA";
            infoCambiaEmail2.SecuenciaCorreo = "AIIUDIJEDKM2/SDS";
            infoCambiaEmail2.TipoOperacion = "I";
            infoCambiaEmail2.CorreoElectronico = "psico@hotmail.com";
            infoCambiaEmail2.CorreoElectronicoIndPreferido = "Y";

            cambiaEmail.lstinfoCambiaEmails = new List<infoCambiaEmails>();
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail);
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail2);


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
        public void InsertfromURLCambiaEmailSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/UpdateCambiaEmail";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(cambiaEmail);
            var response = proxy.PostAsJsonAsync(url, cambiaEmail).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void InsertfromControllerCambiaEmailSuccessTest()
        {
            //Arrange
          

            //Act
            var resultado = _updateCambiaEmailController.Post(cambiaEmail);

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
        public void UpdatefromControllerCambiaEmailSuccessTest()
        {
            //Arrange

            #region Informacion de CambiaEmail


            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";
            var infoCambiaEmail = new infoCambiaEmails();
            infoCambiaEmail.VPDI = "UAC";
            infoCambiaEmail.TipoCorreoElectronico = "FACE";
            infoCambiaEmail.SecuenciaCorreo = "AIIUDIJEDKMD/SDS";
            infoCambiaEmail.TipoOperacion = "U";
            infoCambiaEmail.CorreoElectronico = "JUPITER@hotmail.com";
            infoCambiaEmail.CorreoElectronicoIndPreferido = "N";

            var infoCambiaEmail2 = new infoCambiaEmails();
            infoCambiaEmail2.VPDI = "UAN";
            infoCambiaEmail2.TipoCorreoElectronico = "CASA";
            infoCambiaEmail2.SecuenciaCorreo = "AIIUDIJEDKM2/SDS";
            infoCambiaEmail2.TipoOperacion = "U";
            infoCambiaEmail2.CorreoElectronico = "saturno@hotmail.com";
            infoCambiaEmail2.CorreoElectronicoIndPreferido = "Y";

            cambiaEmail.lstinfoCambiaEmails = new List<infoCambiaEmails>();
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail);
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail2);


            #endregion



            //Act
            var resultado = _updateCambiaEmailController.Post(cambiaEmail);

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
        public void DeletefromControllerCambiaEmailSuccessTest()
        {
            //Arrange
            #region Informacion de CambiaEmail


            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";
            var infoCambiaEmail = new infoCambiaEmails();
            infoCambiaEmail.VPDI = "UAC";
            infoCambiaEmail.TipoCorreoElectronico = "FACE";
            infoCambiaEmail.SecuenciaCorreo = "AIIUDIJEDKMD/SDS";
            infoCambiaEmail.TipoOperacion = "D";
            infoCambiaEmail.CorreoElectronico = "JUPITER@hotmail.com";
            infoCambiaEmail.CorreoElectronicoIndPreferido = "N";

            var infoCambiaEmail2 = new infoCambiaEmails();
            infoCambiaEmail2.VPDI = "UAN";
            infoCambiaEmail2.TipoCorreoElectronico = "CASA";
            infoCambiaEmail2.SecuenciaCorreo = "AIIUDIJEDKM2/SDS";
            infoCambiaEmail2.TipoOperacion = "D";
            infoCambiaEmail2.CorreoElectronico = "saturno@hotmail.com";
            infoCambiaEmail2.CorreoElectronicoIndPreferido = "Y";

            cambiaEmail.lstinfoCambiaEmails = new List<infoCambiaEmails>();
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail);
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail2);


            #endregion



            //Act
            var resultado = _updateCambiaEmailController.Post(cambiaEmail);

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
