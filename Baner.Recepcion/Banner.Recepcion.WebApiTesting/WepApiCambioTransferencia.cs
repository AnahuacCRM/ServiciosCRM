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
    public class WepApiCambioTransferencia
    {

        srvCambioTransferenciaController srvCambioTransferencia;
        srvMarcarTransferidoController srvMarcarTransferencia;
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;

        Transferencia transferencia;
        MarcaTransferido marcartrasfer;
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
            srvCambioTransferencia = new srvCambioTransferenciaController(_logger, _prospectprocessor);

            srvCambioTransferencia.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            srvMarcarTransferencia = new srvMarcarTransferidoController(_logger, _prospectprocessor);
        }

        [TestMethod]
        public void Transferencia()
        {
            transferencia = new Transferencia
            {
                id_Oportunidad = "59071f43-844b-e711-80e0-3863bb2ec350",
                Campus_Destino = "UAN",
                Campus_Origen = "UAM",
                Periodo = "201760"
            };
            var res = srvCambioTransferencia.ActaulizaTransferencia(transferencia);

            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }


        [TestMethod]
        public void MarcarTrasnferido()
        {
            marcartrasfer = new MarcaTransferido
            {
                id_Oportunidad = "b12c476b-b752-e711-811a-e0071b6a9211",
                Campus_Origen = "UAQ",
               
            };

            srvMarcarTransferencia.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            var res = srvMarcarTransferencia.MarcarTransferido(marcartrasfer);

            if (res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
