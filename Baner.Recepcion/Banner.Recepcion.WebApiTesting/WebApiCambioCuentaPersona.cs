using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCambioCuentaPersona
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;

        IOpportunityRepository opportunityRepository;
        srvCambioCuentaPersonaController srvCambioCuentaPersona;

        DatosPersona _datosPersona;

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
            srvCambioCuentaPersona = new srvCambioCuentaPersonaController(_logger, _prospectprocessor);

            srvCambioCuentaPersona.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de CatalogoPeriodos
            _datosPersona = new DatosPersona
            {
               // IdBanner="1",
                Nombre = "Gustavo",
                Segundo_Nombre="",
                Apellidos = "Carreno N",
                Sexo = "M",
                Fecha_Nacimiento= new CustomDate() { Year=200,Month=1,Day=2},
                Nacionalidad="Mex",
                Religion="Indu",
                Estado_Civil="S",
             };
               
            #endregion
            // ArrangeSecurity();

        }


        [TestMethod]
        public void CambioCuentaPersona()
        {
            var resultado = srvCambioCuentaPersona.Post(_datosPersona);
            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
            Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
            Assert.AreNotEqual(false, resultado);

        }
    }
}
