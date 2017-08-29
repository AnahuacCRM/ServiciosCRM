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
    public class WebApiFechaExamenTest
    {

        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateFechaExamenAdmisionController _fechaExamencontroller;
        Token token;
        #endregion

        FechaExamenAdmision fechaExamen;

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
            _fechaExamencontroller = new UpdateFechaExamenAdmisionController(_logger, _prospectprocessor);

            _fechaExamencontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Fecha de Examen de Admision

            #region Lista de Examenes
            List<Examenes> lstExam = new List<Examenes>();
            Examenes exam = new Examenes()
            {
                ClaveExamen = "PAA",
                FechaExamen = new CustomDate() { Day = 09, Month = 09, Year = 1999 }
            };
            lstExam.Add(exam);
            Examenes exam2 = new Examenes()
            {
                ClaveExamen = "EOV",
                FechaExamen = new CustomDate() { Day = 03, Month = 02, Year = 1981 }
            };

            lstExam.Add(exam2);
            //Examenes exam3 = new Examenes()
            //{
            //    ClaveExamen = "PAAN",
            //    FechaExamen = new CustomDate() { Day = 04, Month = 03, Year = 1982 }
            //};
            //lstExam.Add(exam3);
            //Examenes exam4 = new Examenes()
            //{
            //    ClaveExamen = "PAAV",
            //    FechaExamen = new CustomDate() { Day = 05, Month = 04, Year = 1983 }
            //};
            //lstExam.Add(exam4);
            //Examenes exam5 = new Examenes()
            //{
            //    ClaveExamen = "PARA",
            //    FechaExamen = new CustomDate() { Day = 06, Month = 05, Year = 1984 }
            //};
            //lstExam.Add(exam5);
            //Examenes exam6 = new Examenes()
            //{
            //    ClaveExamen = "ESCM",
            //    FechaExamen = new CustomDate() { Day = 07, Month = 06, Year = 1985 }
            //};
            //lstExam.Add(exam6);
            //Examenes exam7 = new Examenes()
            //{
            //    ClaveExamen = "ESCEI",
            //    FechaExamen = new CustomDate() { Day = 08, Month = 07, Year = 1986 }
            //};
            //lstExam.Add(exam7);
            #endregion

            fechaExamen = new FechaExamenAdmision()
            {
                OportunidadIdCRM = new Guid("5D806EF7-123E-E611-80F0-A45D36FCEECC"),
                IdBanner = "0213",
                //IdBanner = "00258112",
                NumeroSolicitud = 14,
                lstExamenes = lstExam,
                SessionExamen = "1",
                VPDI="UAS",
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
        public void URLFechaClaveExamenSucessTest()
        {
            //Arrange
            var url = "http://localhost:21292/Api/UpdateFechaExamenAdmision";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            fechaExamen.lstExamenes[0].ClaveExamen = "PAA";
            fechaExamen.lstExamenes[0].FechaExamen = new CustomDate()
            {
                Year = 1980,
                Month = 1,
                Day = 2
            };
            //Act
            string json = JsonConvert.SerializeObject(fechaExamen);
            var response = proxy.PostAsJsonAsync(url, fechaExamen).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }


        [TestMethod]
        public void FechaExamenControlerSuccessTest()
        {
            //Arrange



            //Act
            var resultado = _fechaExamencontroller.Post(fechaExamen);

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
        public void FechaExamenIDBANNERInexistenteSuccessTest()
        {
            //Arrange

            fechaExamen.IdBanner = "123";
            //Act
            var resultado = _fechaExamencontroller.Post(fechaExamen);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
                var expectedErrorMessage = "El IdBanner de la oportunidad no coincide con el IdBanner proporcionado";


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
