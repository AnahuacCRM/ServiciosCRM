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
    public class WebApiCreateProspectoPreuni
    {

        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
      
        IOpportunityRepository opportunityRepository;
        srvAltaPreUniversitarioWebController srvAltaPreuniversitarioweb;
        srvAltaPreUniversitarioBecarioController srvFormularioBecarios;
        Token token;
        #endregion
        PreUniversitario preuniversitario;
        Becario becarios;
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
            srvAltaPreuniversitarioweb = new srvAltaPreUniversitarioWebController(_logger, _prospectprocessor);
            srvFormularioBecarios = new srvAltaPreUniversitarioBecarioController(_logger, _prospectprocessor);
            srvAltaPreuniversitarioweb.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


           
            // ArrangeSecurity();

        }
        [TestMethod]
        public void GestionPreunivesitarioWEb()
      {

            #region Informacion de CatalogoPeriodos
            preuniversitario = new PreUniversitario()
            {
                //ua_delegacion_municipio_asesorId
                Nombre = "Karyna9",
                Apellido_Paterno = "Cabrera9",
                Apellido_Materno = "Tapia9",
                Segundo_Nombre = "",
                Telefono_Lada = "66",
                Telefono_Numero = "12345671",
                Correo_Electronico = "Kary9@hotmail.com",
                Nivel = "LC",
                Codigo = "48",//"48",
                Descripcion = "Actuaría",
                Campus = "UAM",
                Pais = "",
                Estado = "",
                Municipio = "",
                Origen = "2",
                SubOrigen = @"http://redanahuac.mx/apreu/test_form_apreu_crm.html",
                VPD = "UAM"



            };
            #endregion
            //Act
            var resultado = srvAltaPreuniversitarioweb.Post(preuniversitario);

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
        public void CrearProspectoBecarios()
        {
            srvFormularioBecarios.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            becarios = new Becario()
            {
                //ua_delegacion_municipio_asesorId
                Nombre = "Prueba2",
                Apellido_Paterno = "Ruiz",
                Apellido_Materno = "Garcia",
                Segundo_Nombre = "Carlos",
                Colegio = "270",//"11",
                Grado = "Concluida",
                Fecha_Nacimiento = new CustomDate { Year = 1999, Month = 08, Day = 03 },
                Sexo = "F",
                Telefono_Lada = "55",
                Telefono_Numero = "55555555",
                Correo_Electronico = "prueba1@g.com",
                Pais = "99",
                Estado = "M09",
                Municipio = "09012",
                Periodo = "201710",
                Nivel = "LC",
                Codigo = "2",//"8",//"48",
                Descripcion = "Medico Cirujano",
                Campus = "UAN",

                Origen = "W",// "5",
                SubOrigen = "http://localhost:58402/Formulario",
                VPD = "UAN"




            };

            //Act
            var resultado = srvFormularioBecarios.Post(becarios);

            //Assert

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
