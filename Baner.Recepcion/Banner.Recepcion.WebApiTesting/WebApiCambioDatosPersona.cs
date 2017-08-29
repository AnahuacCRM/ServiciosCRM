using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.Services.Models;
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
    public class WebApiCambioDatosPersona
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        srvCambioDatosPersonaController _servCambiodatosPersona;
        srvCambioDatosPrepaController _srvDatosPrepaController;
        Token token;

        DatosPersona catalogoPeriodos;

        [TestInitialize]
        public void Inittializate()
        {

            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository(_catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _servCambiodatosPersona = new srvCambioDatosPersonaController(_logger, _prospectprocessor);
            _srvDatosPrepaController = new srvCambioDatosPrepaController(_logger, _prospectprocessor);

            _servCambiodatosPersona.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            catalogoPeriodos = new DatosPersona
            {
                id_Cuenta = "f143f3f9-8640-e711-811a-e0071b6a8131",
                Nombre = "Hugo",
                Segundo_Nombre = "Albelto",
                Apellidos = "Sanchez Maradona",
                Fecha_Nacimiento = new CustomDate { Year = 1998, Month = 1, Day = 1 },
                Sexo = "M",
                Nacionalidad = "ME",
                Religion = "CA",
                Estado_Civil = "S"

            };

        }

        [TestMethod]
        public void CambioDatosPersona()
        {
          var resultado =  _servCambiodatosPersona.ActualizarDatosPersona(catalogoPeriodos);

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

        }


        [TestMethod]
        public void CambioDatosPrepa()
        {

            _srvDatosPrepaController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            DatosPrepa dp = new DatosPrepa
            {
                id_Cuenta = "1711c9f3-df5c-e711-8104-c4346bdcf2f1",
                Preparatoria = "8102",
                PromedioPreparatoria = "9.0",
                VPDI = "UAM"
            };
            var resultado = _srvDatosPrepaController.ActualizaDatosPrepa(dp);

            if (resultado.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

        }
    }
}
