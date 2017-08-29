using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.Services.Models;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessLayer;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCreateAccount
    {
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IOpportunityRepository opportunityRepository;
        CreateAccountController _createAccountController;
        // Token token;

        //NewProspect newprospect;
        Cuenta newprospect;

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
            _createAccountController = new CreateAccountController(_logger, _prospectprocessor);

            //Necesario para inicialisar el ReQuest
            _createAccountController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
        }
        [TestMethod]
        public void CrearCuentaTest()
        {


            #region Informacion de la cuenta
            newprospect = new Cuenta()
            {

                IdBanner = "01111111",// "01111111",//"00315908",
                Nombre = "Gussssssssss",
                Segundo_Nombre = " pruebaaaaaa",
                Apellidos = "Guillomen*Rivera",
                Fecha_de_nacimiento = new CustomDate() { Year = 2000, Month = 11, Day = 9 },


                Campus = "UAP",
                CampusVPD = "UAP",
                Numero_Solicitud = 1,
                Estatus_Solicitud = "D",
                Sexo = "M",
                Nacionalidad = "ME",
                Colegio_Procedencia = "8222",// "11",
                Promedio = "6.6",
                PeriodoId= "201710",
                Programa1= "LC-COMU-16", //"LC-ACTU-16",
                Escuela = "EN",
                Codigo_Tipo_Alumno="N",
                Codigo_Tipo_admision= "AD",
                ReligionId= "CA",
                EstadoCivil="S",
                Nivel="LC"
                
            };
            #endregion

            #region Telefono
            //telefono movil
           
            //Personal
            var t2 = new Telefono()
            {
                LadaTelefono = "272",
                Telefono1 = "1399567",
                TipoTelefono = "CE",
                PreferidoTelefono = "",
            };
           
           

            //Otro telefono


            newprospect.Telefonos = new List<Telefono>();
            newprospect.Telefonos.Add(t2);
            //newprospect.Telefonos.Add(t1);
           // newprospect.Telefonos.Add(t0);


            #endregion
            #region Direccion

            var d1 = new Direccion()
            {
                Calle = "Lago Superior #6",
                Numero = "El Espinal",
                CodigoPostal = "94330",
                Colonia = "Orizaba",
                DelegacionMunicipioId = "30118",//"07078",
                //"Ciudad": ".",
                Estado = "M30",//,"M05",
                PaisId = "99",//"99",
                TipoDireccionId = "PR"
            };
           


            newprospect.Direcciones = new List<Direccion>();
           // newprospect.Direcciones.Add(d2);
           newprospect.Direcciones.Add(d1);
           // newprospect.Direcciones.Add(d2);
            //newprospect.Direcciones.Add(d3);

            #endregion
            #region CorreoElectronico


            var co = new Correo()
            {
                TipoCorreoElectronicoId = "PERS",
                Correo_Electronico = "jguillaomen95@hotmail.com",
                IndPreferido = "N",
               

            };
            

            
            newprospect.Correos = new List<Correo>();
            newprospect.Correos.Add(co);
           // newprospect.Correos.Add(co2);
            // newprospect.Correos.Add(co2);
            //newprospect.Correos.Add(co3);





            #endregion
            #region Tutor


            var pP = new PadreoTutor()
            {
                Parentesco = "P",
                FirstName = "Jason",
                MiddleName = "",
                LastName = "Fallen*Up",
                //TipoDireccion = "PR",
                Vive = "Y",



            };
            var dP = new Direccion()
            {
                Calle = "Titanic 80",
                Numero = "",
                CodigoPostal = "94310",
                Colonia = "Cerritos",
                DelegacionMunicipioId = "30118",
                //"Ciudad": ".",
                Estado = "M30",
                PaisId = "99",
                TipoDireccionId = "PR"
            };

            pP.Direcciones = new Direccion();
            pP.Direcciones = dP;


            var pM = new PadreoTutor()
            {
                Parentesco = "M",
                FirstName = "Roxana",
                MiddleName = "",
                LastName = "Rivera Barrientos",
                //TipoDireccion = "PR",
                Vive = "Y",



            };
            var dM = new Direccion()
            {
                Calle = "Lago Superior #6",
                Numero = "El Espina",
                CodigoPostal = "94330",
                Colonia = "",
                DelegacionMunicipioId = "30118",
                //"Ciudad": ".",
                Estado = "M30",
                PaisId = "99",
                TipoDireccionId = "PR"
            };

            pM.Direcciones = new Direccion();
            pM.Direcciones = dM;

            newprospect.PadreoTutor = new List<PadreoTutor>();
           
            newprospect.PadreoTutor.Add(pP);
            //newprospect.PadreoTutor.Add(pM);


            #endregion

            var ResCreteacc = _createAccountController.Create(newprospect);

           if (ResCreteacc.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = ResCreteacc.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

            Assert.AreEqual(HttpStatusCode.Created, ResCreteacc.StatusCode);
            Assert.AreNotEqual(null, ResCreteacc);
        }
    }
}
