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
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCatalogoPeriodosTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        srvGestionPeriodoController _srvGestionPeriodoController;
        Token token;
        #endregion

        CatalogoPeriodos catalogoPeriodos;

        [TestInitialize]
        public void Initialize()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _srvGestionPeriodoController = new srvGestionPeriodoController(_logger, _prospectprocessor);

            _srvGestionPeriodoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de CatalogoPeriodos
            catalogoPeriodos = new CatalogoPeriodos()
            {
                Periodo = "202010",
                Tipo_Periodo = "Anual",
                Descripcion = "POS. Sem. (Ago-Dic 1993) 94-1",
                Fecha_de_Inicio_Periodo = new CustomDate() { Day = 31, Month = 12, Year = 1995 },
                Fecha_de_Fin_Periodo = new CustomDate() { Day = 1, Month = 8, Year = 1995 },
                Fecha_Inicio_Alojamiento = new CustomDate() { Day = 1, Month = 5, Year = 2015 },
                Fecha_Fin_Alojamiento = new CustomDate() { Day = 1, Month = 5, Year = 2015 }

            };
            #endregion
            // ArrangeSecurity();

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
        public void UpdatefromURLcatalogoPeriodosSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/UpdateCatalogoPeriodos";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(catalogoPeriodos);
            var response = proxy.PostAsJsonAsync(url, catalogoPeriodos).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }


        [TestMethod]
        public void UpdatefromControllercatalogoPeriodosSuccessTest()
        {
            //Arrange
            #region Informacion de CatalogoPeriodos
            catalogoPeriodos = new CatalogoPeriodos()
            {
                Periodo = "123456",
                Tipo_Periodo = "Anual",
                Ano_academico = "2016",
                //ClavePeriodo = "201810",
                Descripcion = "ing. (Ago-Dic 2017) 17-1",
                Fecha_de_Inicio_Periodo = new CustomDate() { Day = 1, Month = 8, Year = 2017 },
                Fecha_de_Fin_Periodo = new CustomDate() { Day = 23, Month = 12, Year = 2017 },
                Fecha_Inicio_Alojamiento = new CustomDate() { Day = 1, Month = 5, Year = 2017 },
                Fecha_Fin_Alojamiento = new CustomDate() { Day = 1, Month = 5, Year = 2018 }

            };
            #endregion

            //_updateCatalogoPeriodosController.ControllerContext.Request.
            //Act
            var resultado = _srvGestionPeriodoController.Post(catalogoPeriodos);

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
