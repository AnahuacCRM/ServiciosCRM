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
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Net.Http.Headers;
using Baner.Recepcion.Services.Models;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCatalogoColegiosTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        srvGestionColegiosController _srvGestionColegioColegiosController;
        Token token;
        #endregion

        CatalogoColegios catalogoColegios;

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
            _srvGestionColegioColegiosController = new srvGestionColegiosController(_logger, _prospectprocessor);

            _srvGestionColegioColegiosController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
            #region Informacion de CatalogoColegios
            catalogoColegios = new CatalogoColegios();


            //catalogoColegios.ClaveColegio = "0001";
            catalogoColegios.Clave_Colegio = "11"; ;
            //catalogoColegios.NombreColegio = "I ANGLO MEXICANO (HERMOSILLO)";
            catalogoColegios.Nombre_Colegio = "ACADEMIA MADDOX";
            catalogoColegios.Calle = "Cto. Circunvalación Poniente 3811_";
            catalogoColegios.Numero = "Naucalpan De Juárezpp_";
            catalogoColegios.Colonia = "Naucalpan De Juárez_";
            catalogoColegios.Municipio = "11001";
            catalogoColegios.Estado = "AK";
            catalogoColegios.Pais = "99";
            catalogoColegios.Codigo_Postal = "53120";
            catalogoColegios.Tipo_Colegio = "H";
            //Contactos contactos = new Contactos();
            //contactos.VPDI = "UAN";
            //contactos.Contacto = "Manuel Sandoval Rodríguez";
            //contactos.TipoContacto="DIGR";
            //catalogoColegios.lstContactos = new List<Contactos>();
            //catalogoColegios.lstContactos.Add(contactos);

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
        public void UpdatefromURLCatalogoColegiosSucessTest()
        {

            //Arrange
            var url = "http://localhost:21292/Api/UpdateCatalogoColegios";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(catalogoColegios);
            var response = proxy.PostAsJsonAsync(url, catalogoColegios).Result;
            var result = response.Content.ReadAsAsync<bool>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(false, result);
        }


        [TestMethod]
        public void UpdatefromControlerCatalogoColegiosSuccessTest()
        {   
            //Arrange

            //Act
            var resultado = _srvGestionColegioColegiosController.Post(catalogoColegios);

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
        public void UpdatefromControlerCatalogoColegiosListaContactosSuccessTest()
        {
            //Arrange
            #region Informacion de CatalogoColegios
            catalogoColegios = new CatalogoColegios();

            //catalogoColegios.ClaveColegio = "1000";
            catalogoColegios.Clave_Colegio = "998"; ;
            //catalogoColegios.NombreColegio = "I ANGLO MEXICANO (HERMOSILLO)";
            catalogoColegios.Nombre_Colegio = "Prueba2 Colegio";
            catalogoColegios.Calle = "Periférico Poniente";
            catalogoColegios.Numero = "98";
            catalogoColegios.Colonia = "";
            catalogoColegios.Municipio = "26001";
            catalogoColegios.Estado = "SON";
            catalogoColegios.Pais = "99";
            catalogoColegios.Codigo_Postal = "83250";
            catalogoColegios.Tipo_Colegio = "H";
            Contactos contactos = new Contactos();
            contactos.VPDI = "UAN";
            contactos.Contacto = "Manuel Sandoval Rodríguez";
            contactos.TipoContacto = "DIGR";
            //Contactos contactos2 = new Contactos();
            //contactos2.VPDI = "UAS";
            //contactos2.Contacto = "Manuel Sandoval Rodríguez";
            //contactos2.TipoContacto = "DIGR";
            //catalogoColegios.lstContactos = new List<Contactos>();
            //catalogoColegios.lstContactos.Add(contactos);
            //catalogoColegios.lstContactos.Add(contactos2);

            #endregion
            //Act
            var resultado = _srvGestionColegioColegiosController.Post(catalogoColegios);

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
        public void UpdatefromControlerCatalogoColegiosSinListaContactosSuccessTest()
        {
            //Arrange
            #region Informacion de CatalogoColegios
            catalogoColegios = new CatalogoColegios();

            //catalogoColegios.ClaveColegio = "1000";
            catalogoColegios.Clave_Colegio = "998"; ;
            //catalogoColegios.NombreColegio = "I ANGLO MEXICANO (HERMOSILLO)";
            catalogoColegios.Nombre_Colegio = "Prueba2 Colegio";
            catalogoColegios.Calle = "Periférico Poniente";
            catalogoColegios.Numero = "98";
            catalogoColegios.Colonia = "";
            catalogoColegios.Municipio = "26001";
            catalogoColegios.Estado = "SON";
            catalogoColegios.Pais = "99";
            catalogoColegios.Codigo_Postal = "83250";
            catalogoColegios.Tipo_Colegio = "H";
          
          

            #endregion
            //Act
            var resultado = _srvGestionColegioColegiosController.Post(catalogoColegios);

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
        public void UpdatefromControlerCatalogoColegiosVPDIyTipoContactoErroneosSuccessTest()
        {
            //Arrange
            #region Informacion de CatalogoColegios
            catalogoColegios = new CatalogoColegios();

            //catalogoColegios.ClaveColegio = "1000";
            catalogoColegios.Clave_Colegio = "998"; ;
            //catalogoColegios.NombreColegio = "I ANGLO MEXICANO (HERMOSILLO)";
            catalogoColegios.Nombre_Colegio = "Prueba2 Colegio";
            catalogoColegios.Calle = "Periférico Poniente";
            catalogoColegios.Numero = "98";
            catalogoColegios.Colonia = "";
            catalogoColegios.Municipio = "26001";
            catalogoColegios.Estado = "SON";
            catalogoColegios.Pais = "99";
            catalogoColegios.Codigo_Postal = "83250";
            catalogoColegios.Tipo_Colegio = "H";
            //Contactos contactos = new Contactos();
            //contactos.VPDI = "111";
            //contactos.Contacto = "Manuel Sandoval Rodríguez";
            //contactos.TipoContacto = "1111";          
            //catalogoColegios.lstContactos = new List<Contactos>();
            //catalogoColegios.lstContactos.Add(contactos);
      

            #endregion
            //Act
            var resultado = _srvGestionColegioColegiosController.Post(catalogoColegios);

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
        public void UpdatefromControlerCatalogoColegiosDireccionInexistenteSuccessTest()
        {
            //Arrange
            catalogoColegios.Municipio = "666";
            catalogoColegios.Estado = "666";
            //Act
            var resultado = _srvGestionColegioColegiosController.Post(catalogoColegios);

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
