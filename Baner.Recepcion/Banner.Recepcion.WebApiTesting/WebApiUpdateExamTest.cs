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
    public class WebApiUpdateExamTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateExaminadoController _updateexaminadocontroller;
        Token token;
        #endregion

        Examinado exam;

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
            _updateexaminadocontroller = new UpdateExaminadoController(_logger, _prospectprocessor);

            _updateexaminadocontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Examinado
            exam = new Examinado()
            {
                OportunidadIdCRM = new Guid("EEC074FC-7036-E611-80EB-6C3BE5A84798"),
                IdBanner = "0212",
                NumeroSolicitud = 13,
                Programa = "LC-INMT-90",
                PromedioPreparatoria = "8.3",
                Periodo = "201560",
                TipoAlumno = "S",
                VPDI="UAS"
               
            };
            //IdBanner = "00165740",
            //    NumeroSolicitud = 2,
            //    Programa = "LC-TINT-10",
            //    PromedioPreparatoria = "8.4",
            //    Periodo = "201560",
            //    TipoAlumno = "N"
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
        public void UpdatefromURLExaminadoSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/UpdateExaminado";
            HttpClient proxy = new HttpClient();
            // proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            exam.OportunidadIdCRM = new Guid("98257340-19b8-471e-b0ea-85fbc9b69177");
            exam.PromedioPreparatoria="9";
            //Act
            string json = JsonConvert.SerializeObject(exam);
            var response = proxy.PostAsJsonAsync(url, exam).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }


        [TestMethod]
        public void UpdatefromControlerExaminadotSuccessTest()
        {
            //Arrange



            //Act
            var resultado = _updateexaminadocontroller.Post(exam);

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
        public void UpdateWithoutNoRequiredExaminadoSucessTest()
        {

            //Arrange
            #region Limpiar campos no requeridos

            exam.Periodo = default(string);
      
            #endregion


            //Act
            var resultado = _updateexaminadocontroller.Post(exam);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);

        }

        #endregion
    }
}
