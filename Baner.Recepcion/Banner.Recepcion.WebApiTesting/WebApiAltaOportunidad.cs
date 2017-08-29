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
    public class WebApiAltaOportunidad
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvAltaOportunidadController _AltaOportunidad;

        Oportunidad VOporunidad;


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
            _AltaOportunidad = new srvAltaOportunidadController(_logger, _prospectprocessor);

            //Necesario para inicialisar el ReQuest
            _AltaOportunidad.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
        }

        [TestMethod]
        public void AltaOportunidadControler()
        {
            VOporunidad = new Oportunidad
            {
                id_Cuenta = "0bd12d1e-c37b-e711-811b-e0071b669e71",
                //id_Cuenta= "292887b5-b366-e711-8104-5065f38a9b71",
                id_Oportunidad = "",
                Numero_Solicitud = 1,
                VPD = "UAN",
                Campus = "UAN",
                Estatus_Solicitud = "D",

                Periodo = "201760",
                Nivel = "LC",
                Programa = "LC-ACTU-16",
                Escuela = "IN",
                Codigo_Tipo_Alumno = "S",
                Codigo_Tipo_admision = "AA"// "AD"
            };
            var res = _AltaOportunidad.Create(VOporunidad);


            if (res.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

            Assert.AreEqual(HttpStatusCode.Created, res.StatusCode);
            Assert.AreNotEqual(null, res);

        }
    }
}
