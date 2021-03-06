﻿using System;
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
    public class WebApiAdmitidoTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateAdmitidoController _admitidocontroller;
        Token token;
        #endregion

        Admitido admitido;

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
            _admitidocontroller = new UpdateAdmitidoController(_logger, _prospectprocessor);

            _admitidocontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Fecha de Examen de Admision
            admitido = new Admitido()
            {                                
                OportunidadIdCRM = new Guid("1313a365-5bed-e611-8106-e0071b6a2351"),
                IdBanner = "00278813",
                NumeroSolicitud = 2,
                DesicionAdmision = "AD",
                Programa = "LC-DERE-16",
                Periodo = "201710",
                Campus = "UAQ",
                VPDI = "UAMQ",
                Escuela = "DE",
                TipoAdmision = "AD",
                PuntualizacionSobresaliente = "N"
            };
            #endregion
            //ArrangeSecurity();

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
        public void URLUpdateAdmitidoSucessTest()
        {
            //Arrange
            var url = "http://localhost:21292/Api/UpdateAdmitido";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(admitido);
            var response = proxy.PostAsJsonAsync(url, admitido).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void AdmitidoControlerSuccessTest()
        {
            //Arrange


            //Act
            var resultado = _admitidocontroller.Post(admitido);

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
