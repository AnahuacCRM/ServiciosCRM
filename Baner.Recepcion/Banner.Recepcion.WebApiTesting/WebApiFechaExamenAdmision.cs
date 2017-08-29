using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.DataInterfaces;
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
    public class WebApiFechaExamenAdmision
    {
        srvCambioFechaExamenAdmisionController srvFechaExamenAdmision;
        srvCambioResultadoExamenController srvResultadoExamenController;
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        FechaExamenAdmision fechaExamenAdmision;
        [TestInitialize]
        public void Initializate()
        {

            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            srvFechaExamenAdmision = new srvCambioFechaExamenAdmisionController(_logger, _prospectprocessor);
            srvResultadoExamenController = new srvCambioResultadoExamenController(_logger, _prospectprocessor);

            srvFechaExamenAdmision.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
        }
        [TestMethod]
        public void FechaExamenAdmisionController()
        {
            fechaExamenAdmision = new FechaExamenAdmision
            {
                id_Oportunidad = "e987a773-163b-e711-810e-e0071b6a82a1",
                SessionExamen = "2"

            };

            Examenes ex2 = new Examenes
            {
                ClaveExamen = "EOV",
                FechaExamen = new CustomDate { Year = 2017, Month = 4, Day = 30 }
            };
            fechaExamenAdmision.lstExamenes = new System.Collections.Generic.List<Examenes>();
            // fechaExamenAdmision.lstExamenes.Add(ex);
            //fechaExamenAdmision.lstExamenes.Add(ex2);

            var res = srvFechaExamenAdmision.ActualizaFechaExamenAdmision(fechaExamenAdmision);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }



        public void ResultadoEsamenControler()
        {
            srvResultadoExamenController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            ResultadoExamen resEx = new ResultadoExamen
            {
                id_Cuenta = "e987a773-163b-e711-810e-e0071b6a82a1",
                ResultadosdeExamen = null,
                VPDI = ""
            };
            var res = srvResultadoExamenController.ActualizaResultadoExamen(resEx);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }

    }
}
