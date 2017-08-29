using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
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
    public class WebApiInscritoNuevoIngreso
    {
        srvCambioNuevoIngresoController srvnuevoingresocontroller;
        srvCambioInscritoController srvinscritpcontroller;
        srvCambioFaseController srvCambiofasesolicitante;
       
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        NuevoIngreso nuevoingreso;
        Inscrito inscrito;

        [TestInitialize]
        public void Inictializate()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            srvnuevoingresocontroller = new srvCambioNuevoIngresoController(_logger, _prospectprocessor);
            srvinscritpcontroller = new srvCambioInscritoController(_logger, _prospectprocessor);
            srvCambiofasesolicitante = new srvCambioFaseController(_logger, _prospectprocessor);




        }

        [TestMethod]
        public void ActualizaInscrito()
        {
            srvinscritpcontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            inscrito = new Inscrito
            {
                id_Oportunidad = "e2ee9332-b568-e711-811c-e0071b6a8131",
                FechaPagoInscripcion = new CustomDate { Year = 2017, Month = 07, Day = 18 }
            };

            var res = srvinscritpcontroller.ActualizaInscrito(inscrito);


            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }

        [TestMethod]
        public void ActualizaNuevoIngreso()
        {

            srvnuevoingresocontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            nuevoingreso = new NuevoIngreso
            {
                Id_Oportunidad = "e2ee9332-b568-e711-811c-e0071b6a8131",
                FechaSeleccionCursos = new CustomDate { Year = 2017, Month = 07, Day = 19 }

            };

            var res = srvnuevoingresocontroller.ActualizaNuevoIngreso(nuevoingreso);

            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

        }

        [TestMethod]
        public void CambiaFaeSolicitante()
        {

            srvCambiofasesolicitante.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            CambioFaseSolicitante fase = new CambioFaseSolicitante
            {
                id_oportunidad = "e2ee9332-b568-e711-811c-e0071b6a8131",
                Fase=4
            };

          
            var res = srvCambiofasesolicitante.MoveFaseSolicitante(fase);

            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

        }








    }
}
