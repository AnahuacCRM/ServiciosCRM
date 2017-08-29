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
    public class WebApiCambiaDomicilio
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateCambioDireccionController _updateCambioDireccionController;
        Token token;
        #endregion

        CambiaDomicilio cambiadomicilio;

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
            _updateCambioDireccionController = new UpdateCambioDireccionController(_logger, _prospectprocessor);

            _updateCambioDireccionController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            #region Informacion de Domicilio
            List<Domicilio> lstDomicilio = new List<Domicilio>();
            Domicilio dom = new Domicilio()
            {
                Calle1 = "negocio",
                Calle2 = "",
                //Ciudad = "INFONAVIT",
                Colonia = "INFONAVIT",
                CP = "01029",
                Estado = "DF",
                Municipio = "09001",
                Pais = "99",
                SecuenciaDireccion = "3",
                TipoDireccion = "PR",
                TipoOperacion = "U",
                VPDI = "UAN"
            };
            lstDomicilio.Add(dom);
            Domicilio dom2 = new Domicilio()
            {
                Calle1 = "AV. MIGUEL BERNARD SDSAD",
                Calle2 = "555",
                //Ciudad = "LA ESCALERA",
                Colonia = "LA ESCALERA",
                CP = "07320",
                Estado = "DF",
                Municipio = "09007",
                Pais = "99",
                SecuenciaDireccion = "1",
                TipoDireccion = "PR",
                TipoOperacion = "U",
                VPDI = "UAN"
            };
            lstDomicilio.Add(dom2);
            Domicilio dom3 = new Domicilio()
            {
                Calle1 = "CALLE PRUEBA FINAL hector 7",
                Calle2 = "NUMERO PRUEBA FINAL hector 8",
                //Ciudad = "Real del Valle 1a Seccion",
                Colonia = "Real del Valle 1a Seccion",
                CP = "55883",
                Estado = "MEX",
                Municipio = "15002",
                Pais = "99",
                SecuenciaDireccion = "2",
                TipoDireccion = "PR",
                TipoOperacion = "I",
                VPDI = "UAN"
            };
            lstDomicilio.Add(dom3);
            Domicilio dom4 = new Domicilio()
            {
                Calle1 = "CALLE PRUEBA FINAL Roma 7 update",
                Calle2 = "NUMERO PRUEBA FINAL hector 8",
                //Ciudad = "Real del Valle 1a Seccion",
                Colonia = "Real del Valle 1a Seccion",
                CP = "55883",
                Estado = "MEX",
                Municipio = "15002",
                Pais = "99",
                SecuenciaDireccion = "2",
                TipoDireccion = "PR",
                TipoOperacion = "U",
                VPDI = "UAN"
            };
            lstDomicilio.Add(dom4);

            cambiadomicilio = new CambiaDomicilio()
            {
                IdBanner = "00019990",
                lstDomicilio = lstDomicilio
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
        public void URLCambiaDomicilioSucessTest()
        {
            //Arrange
            var url = "http://localhost:21292/Api/UpdateCambioDireccion";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            //Act
            string json = JsonConvert.SerializeObject(cambiadomicilio);
            var response = proxy.PostAsJsonAsync(url, cambiadomicilio).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void CambiaDomicilioControllerSuccessTest()
        {
            //Arrange


            //Act
            var resultado = _updateCambioDireccionController.Post(cambiadomicilio);

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
