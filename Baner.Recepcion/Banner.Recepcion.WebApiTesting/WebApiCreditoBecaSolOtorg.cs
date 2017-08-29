using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using Baner.Recepcion.BusinessTypes;
using System.Collections.Generic;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCreditoBecaSolOtorg
    {
        srvAltaSolicitaBecaController solicutudBecaControlador;
        srvAltaOtorgaBecaController otorgaBecaController;
        srvGestionOtorgaCreditoController gestionOtorgaCreditoContorller;
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;



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
            solicutudBecaControlador = new srvAltaSolicitaBecaController(_logger, _prospectprocessor);

            solicutudBecaControlador.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            otorgaBecaController = new srvAltaOtorgaBecaController(_logger, _prospectprocessor);
            gestionOtorgaCreditoContorller = new srvGestionOtorgaCreditoController(_logger,_prospectprocessor);


        }
        [TestMethod]
        public void SolicitaBecaController()
        {
            var infb1 = new InformacionBeca
            {
                TipoBeca = "10221633",
                Periodo = "201760",
                CampusVPDI = "UAM",
                DescripcionBeca = "Beca BME 50%-50%",
                FechaSolicitudBeca = new CustomDate { Year = 2017, Month = 1, Day = 1 }
            };
            var listinfoB = new List<InformacionBeca>();
           // listinfoB.Add(infb1);

            var infb2 = new InformacionBeca
            {
                TipoBeca = "00000005",
                Periodo = "201760",
                CampusVPDI = "UAM",
                DescripcionBeca = "Beca de excelencia 25%",
                FechaSolicitudBeca = null
            };

            listinfoB.Add(infb2);

            var ObjetoRecibe = new SolicitaBeca
            {
                id_Cuenta = "b6c7418c-c940-e711-8117-e0071b669e71",
                SolicitudBecas = listinfoB
            };

           

            var resultado = solicutudBecaControlador.CreateSolicitaBeca(ObjetoRecibe);

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);

            }
        }

        [TestMethod]
        public void OtorgaBecaController()
        {
            otorgaBecaController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            var BecaOt = new OtorgamientoBeca
            {
                Id_Cuenta = "0d48fba0-972a-e711-8114-e0071b669e71"
            };

            var becaC = new BecaClass
            {
                TipoBeca = "1",
                Periodo = "201860",
                CampusVPDI = "UAN",
                DescripcionBeca = "Beca por promedio"
            };
            var becaol = new BecaOtorga
            {
                Beca = becaC,
                FechaOtorgaBeca = new CustomDate { Year = 2017, Month = 4, Day = 28 },
                FechaVencimientoBeca = new CustomDate { Year = 2018, Month = 1, Day = 2 }
            };

           
            BecaOt.lstBeca = new List<BecaOtorga>();
            BecaOt.lstBeca.Add(becaol);



            var resultado = otorgaBecaController.OtorgamientoBeca(BecaOt);

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);

            }
        }

        [TestMethod]
        public void OtorgaCreditoController()
        {

            gestionOtorgaCreditoContorller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };

            var CreditoO = new OtorgaCredito
            {
                id_Cuenta = "0d48fba0-972a-e711-8114-e0071b669e71"
            };
            var litC = new InformacionOtorgaCredito("Credito nuevo", "UAN", "201860", new CustomDate { Year = 2010, Month = 2, Day = 2 });
            CreditoO.InfoCreditos = new List<InformacionOtorgaCredito>();
            CreditoO.InfoCreditos.Add(litC);


            var resultado = gestionOtorgaCreditoContorller.GestionOtorgaCredito(CreditoO);

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);

            }


        }
    }
}
