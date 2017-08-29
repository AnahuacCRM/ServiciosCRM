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
    public class WebApiParentescoTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        UpdateParentescoController _updateParentescoController;
        Token token;
        #endregion

        Parentesco parentesco;

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
            _updateParentescoController = new UpdateParentescoController(_logger, _prospectprocessor);

            _updateParentescoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
            #region Informacion de Parentesco
            parentesco = new Parentesco();
            parentesco.IdBanner = "00246676";
            #region listParentesco
            var listParentesco = new InfoParentesto();
            listParentesco.ParentescoTipoRelacion = "P";
            listParentesco.ParentescoNombre = "Sandra";
            listParentesco.ParentescoApellidos = "Juarez*Juarez";
            listParentesco.ParentescoDireccion = "BI";
            listParentesco.ParentescoVive = "Y";
            listParentesco.VPDI = "UAN";
            //var listParentescoM = new InfoParentesto();
            //listParentescoM.ParentescoTipoRelacion = "M";
            //listParentescoM.ParentescoNombre = "Maria Petra";
            //listParentescoM.ParentescoApellidos = "Juarez*Lopez";
            //listParentescoM.ParentescoDireccion = "E2";
            //listParentescoM.ParentescoVive = "Y";

            #endregion
            parentesco.ListInfoParentesco = new List<InfoParentesto>();
            parentesco.ListInfoParentesco.Add(listParentesco);
            //parentesco.ListInfoParentesco.Add(listParentescoM);
            #endregion

            //#region Informacion de Parentesco
            //parentesco = new Parentesco();
            //parentesco.IdBanner = "00242860";
            //#region listParentesco
            //var listParentesco = new InfoParentesto();
            //listParentesco.ParentescoTipoRelacion = "P";
            //listParentesco.ParentescoNombre = "Juan Adolfo";
            //listParentesco.ParentescoApellidos = "Peña*Lopez";
            //listParentesco.ParentescoDireccion = "CW";
            //listParentesco.ParentescoVive = "Y";
            //listParentesco.ParentescoVive = "UAS";
            //#endregion
            //parentesco.ListInfoParentesco = new List<InfoParentesto>();
            //parentesco.ListInfoParentesco.Add(listParentesco);
            //#endregion

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
        public void UpdatefromURLParentescoSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/UpdateParentesco";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(parentesco);
            var response = proxy.PostAsJsonAsync(url, parentesco).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }


        [TestMethod]
        public void UpdatefromControllerParentescoSuccessTest()
        {
            //Arrange
            #region Informacion de Parentesco
            parentesco = new Parentesco();
            parentesco.IdBanner = "00242860";
            #region listParentesco
            var listParentesco = new InfoParentesto();
            listParentesco.ParentescoTipoRelacion = "P";
            listParentesco.ParentescoNombre = "Juan Adolfo";
            listParentesco.ParentescoApellidos = "Juarez*Juarez";
            listParentesco.ParentescoDireccion = "BI";
            listParentesco.ParentescoVive = "";
            listParentesco.VPDI = "UAN";
            //var listParentescoM = new InfoParentesto();
            //listParentescoM.ParentescoTipoRelacion = "M";
            //listParentescoM.ParentescoNombre = "Maria Petra";
            //listParentescoM.ParentescoApellidos = "Juarez*Lopez";
            //listParentescoM.ParentescoDireccion = "E2";
            //listParentescoM.ParentescoVive = "Y";

            #endregion
            parentesco.ListInfoParentesco = new List<InfoParentesto>();
            parentesco.ListInfoParentesco.Add(listParentesco);
            //parentesco.ListInfoParentesco.Add(listParentescoM);
            #endregion


            //Act
            var resultado = _updateParentescoController.Post(parentesco);

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
