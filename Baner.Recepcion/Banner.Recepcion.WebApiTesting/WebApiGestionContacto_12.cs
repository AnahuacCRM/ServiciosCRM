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
using System.Collections.Generic;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiGestionContacto_12
    {

        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        srvGestionContactosController _srvgestioncontacto;

        Parentesco parentesco;

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
            _srvgestioncontacto = new srvGestionContactosController(_logger, _prospectprocessor);

            //Necesario para inicialisar el ReQuest
            _srvgestioncontacto.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


        }

        [TestMethod]
        public void TestoGestionParientes()
        {


            var pM = new PadreoTutor()

            {
                Parentesco = "M",
                FirstName = "Stephanie",
                MiddleName = "",
                LastName = "Handlos",
                //TipoDireccion = "PR",
                Vive = "",



            };
            var dM = new Direccion()
            {
                Calle = "Lot Des Abricots 20",
                Numero = "Remire Montjoly",
                CodigoPostal = "00000",
                Colonia = "Remire Montjoly",
                DelegacionMunicipioId = "00000",
                //"Ciudad": ".",
                Estado = "000",
                PaisId = "63",
                TipoDireccionId = "PR"
            };

            pM.Direcciones = new Direccion();
            pM.Direcciones = dM;
            

            var pt = new PadreoTutor()
            {
                Parentesco = "P",
                FirstName = "Carlos",
                MiddleName = "Alfonso",
                LastName = "Anaya * Valenzuela",
                //TipoDireccion = "PR",
                Vive = "",



            };
            var dt = new Direccion()
            {
                Calle = "Cda. de la Coruña",
                Numero = "10",
                CodigoPostal = "52786",
                Colonia = "Parques De La Herradura",
                DelegacionMunicipioId = "15037",
                //"Ciudad": ".",
                Estado = "M11",
                PaisId = "99",
                TipoDireccionId = "PR"
            };

            pt.Direcciones = new Direccion();
            pt.Direcciones = dt;

         
            parentesco = new Parentesco
            {
                id_Cuenta = "e969dd10-656d-e711-8113-e0071b6a82e1",
                
            };

            parentesco.PadreoTutor = new List<PadreoTutor>();

            parentesco.PadreoTutor.Add(pM);
            //parentesco.PadreoTutor.Add(pt);
           // parentesco.PadreoTutor.Add(pm);
            var res = _srvgestioncontacto.GestionContacto(parentesco);


            if (res.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = res.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }
        }
    }
}
