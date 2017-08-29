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
    public class WebApiOtorgamientoBecaTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        OtorgamientoBecaController _otorgamientoBecaController;
        Token token;
        #endregion

        OtorgamientoBeca otorgamientoBeca;

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
            _otorgamientoBecaController = new OtorgamientoBecaController(_logger, _prospectprocessor);

            _otorgamientoBecaController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Otorgamiento de beca
            otorgamientoBeca = new OtorgamientoBeca()
            {
                IdBanner = "00294811",
                lstBeca = new List<BecaOtorga>()
                 {
                     new BecaOtorga()
                     {
                         Beca = new BecaClass() { TipoBeca = "A", DescripcionBeca="DescripcionBeca",Periodo="196810",CampusVDI="UAN" },
                         FechaOtorgaBeca = default(CustomDate),// new CustomDate(){ Year = 1980, Month =1, Day=2 },
                         FechaVencimientoBeca = default(CustomDate)// new CustomDate(){ Year = 1980, Month =3, Day=2 }
                     },
                     new BecaOtorga()
                     {
                         Beca = new BecaClass() { TipoBeca = "AB", DescripcionBeca="DescripcionBeca2",Periodo="196810",CampusVDI="UAS" },
                         FechaOtorgaBeca = new CustomDate(){ Year = 2016, Month = 8, Day = 6 },
                         FechaVencimientoBeca = new CustomDate(){ Year = 2016, Month = 9, Day = 6 }
                     }
                 }
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
        public void URLOtorgamientoBecaSucessTest()
        {
            //Arrange
            var url = "http://localhost:21292/Api/OtorgamientoBeca";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(otorgamientoBeca);
            var response = proxy.PostAsJsonAsync(url, otorgamientoBeca).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void OtorgamientoBecaControlerSuccessTest()
        {
            //Arrange


            //Act
            var resultado = _otorgamientoBecaController.Post(otorgamientoBeca);

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
