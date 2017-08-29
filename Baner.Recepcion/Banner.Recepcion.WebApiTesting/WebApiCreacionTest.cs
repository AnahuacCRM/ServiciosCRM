using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;

using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;
using Baner.Recepcion.Services.Models;
using System.Configuration;
using System.Text;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCreacionTest
    {
        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        //IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        CreateProspectController _createprospectcontroller;
        Token token;
        #endregion

        NewProspect newprospect;

        [TestInitialize]
        public void Initialize()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository( _catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _createprospectcontroller = new CreateProspectController(_logger, _prospectprocessor);

            _createprospectcontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Prospecto
            newprospect = new NewProspect()
            {
                //IdCRM = new Guid("3638c958-5a3d-e611-80ec-6c3be5a878bc"),
                //IdBanner = "00261233",
                //Nombre = "Paola",
                //Apellidos = "Gomez*Arzamendi",
                // IdBanner = "00271079",
                CuentaName = "Anahuac CRM Dynamics",
                IdBanner = "00319230",
                Nombre = "Gustavo",
                Apellidos = "Carreño*Nevarez",
                NameOportunidad="Oportunidad Gustavo",
                Campus = "UAM",
                CampusVPD = "UAM",
                CiudadNacimiento = "",
                ColegioProcedencia = "6980",
                Escuela = "ME",
                EstadoCivil = "S",
                EstadoNacimiento = "",
                EstatusSolicitud = "D",
                FechaNacimiento = new CustomDate() { Year = 1995, Month = 3, Day = 27 },
                Nacionalidad = "ME",
                PeriodoId = "201560",
                Programa1 = "LC-MEDI-16",
                Promedio = "8.3",
                ReligionId = "CA",
                SegundoNombre = "Guadalupe",
                Sexo = "F",
                CodigoTipoadmision = "AD",
                CodigoTipoAlumno = "N",
                Nivel = "LC",
                NumeroSolicitud=2
            };
            #endregion
            #region Direccion
            var d = new Direccion()
            {
                Calle = "CALLE 7 #567",
                //Ciudad = "Ciudad de México",
                CodigoPostal = "97217",
                Colonia = "Mérida",
                DelegacionMunicipioId = "31050",
                Estado = "M31",
                //Estado = "D1",//estado mal
                Numero = "por 66 y 68",
                PaisId = "99",
                //Preferido="",
                SecuenciaDireccion = 1,
                TipoDireccionId = "PR"
            };
            //var d = new Direccion()
            //{
            //    Calle = "Calle Rodin #45 Villas del Arte",
            //    //Ciudad = "Ciudad de México",
            //    CodigoPostal = "77500",
            //    Colonia = "Villas Tropicales",
            //    DelegacionMunicipioId = "23005",
            //    //Estado = "DF",
            //    Estado = "M23",
            //    Numero = "mZA. 61 Lte. 8",
            //    PaisId = "99",
            //    //Preferido="",
            //    SecuenciaDireccion = 1,
            //    TipoDireccionId = "PR"
            //};
            newprospect.Direcciones = new List<Direccion>();
            newprospect.Direcciones.Add(d);
            #endregion
            #region Telefono
            var t = new Telefono()
            {
                LadaTelefono = "999",
                PreferidoTelefono = "Y",
                SecuenciaTelefono = 1,
                Telefono1 = "6681150266",
                TipoTelefono = "CE"
            };
            var t2 = new Telefono()
            {
                LadaTelefono = "999",
                PreferidoTelefono = "",
                SecuenciaTelefono = 2,
                Telefono1 = "1234567890",
                TipoTelefono = "PR"
            };
            newprospect.Telefonos = new List<Telefono>();
            newprospect.Telefonos.Add(t);
            newprospect.Telefonos.Add(t2);
            #endregion
            #region CorreoElectronico
            var co = new Correo()
            {
                CorreoElectronico1 = "guscane@outlokk.com",
                IndPreferido = "Y",
                TipoCorreoElectronicoId = "PERS",
                SecuenciaCorreo = "AAAVJ0AAOAAIPO6ABa"
            };
            newprospect.Correos = new List<Correo>();
            newprospect.Correos.Add(co);
            #endregion
            #region Tutor
            var tut = new PadreoTutor()
            {
                FirstName = "Reyna",
                LastName = "Arzamendi Castillo",
                Parentesco = "M",
                Vive = "",
                TipoDireccion = "PR",
                ROWID= "AAAVXGAAOAAIQOVAAl"

            };
            var tut2 = new PadreoTutor()
            {
                FirstName = "Jorge",
                LastName = "Gómez Cauich",
                Parentesco = "P",
                Vive = "",
                TipoDireccion = "PR",
                ROWID= "AAAVXGAAOAAIQOVAAj"
            };
            var tut3 = new PadreoTutor()
            {
                FirstName = "Jorge",
                LastName = "Gómez Cauich",
                Parentesco = "P",
                Vive = "",
                TipoDireccion = "PR",
                ROWID= "AAAVXGAAOAAIQOVAAk"
            };
            newprospect.PadreoTutor = new List<PadreoTutor>();
            newprospect.PadreoTutor.Add(tut);
            newprospect.PadreoTutor.Add(tut2);
            newprospect.PadreoTutor.Add(tut3);
            #endregion
            //#region Informacion de Prospecto
            //newprospect = new NewProspect()
            //{
            //    //IdCRM = new Guid("3638c958-5a3d-e611-80ec-6c3be5a878bc"),
            //    IdBanner = "00019990",
            //    Nombre = "Apolo",
            //    Apellidos = "García*Perez",
            //    Campus = "UAC",
            //    CampusVPD = "UAC",
            //    CiudadNacimiento = "DF",
            //    ColegioProcedencia = "1000",
            //    Escuela = "AC",
            //    EstadoCivil = "S",
            //    EstadoNacimiento = "DF",
            //    EstatusSolicitud = "C",
            //    FechaNacimiento = new CustomDate() { Year = 1993, Month = 12, Day = 28 },
            //    Nacionalidad = "AL",
            //    PeriodoId = "201560",
            //    Programa1 = "LC-INMT-90",
            //    Promedio = "9.0",
            //    ReligionId = "CA",
            //    SegundoNombre = "christian",
            //    Sexo = "M",
            //    CodigoTipoadmision = "AA",
            //    CodigoTipoAlumno = "S",
            //    Nivel = "LC"
            //};
            //#endregion
            //#region Direccion
            //var d = new Direccion()
            //{
            //    Calle = "PARQUE DE VALENCIA NO. 36 - 1",
            //    //Ciudad = "Ciudad de México",
            //    CodigoPostal = "01000",
            //    Colonia = "San Ángel",
            //    DelegacionMunicipioId = "09010",
            //    Estado = "M09",
            //    //Estado = "D1",//estado mal
            //    Numero = "PARQUES DE LA HERRADURA",
            //    PaisId = "99",
            //    //Preferido="",
            //    SecuenciaDireccion = 1,
            //    TipoDireccionId = "PA"
            //};
            ////var d = new Direccion()
            ////{
            ////    Calle = "Calle Rodin #45 Villas del Arte",
            ////    //Ciudad = "Ciudad de México",
            ////    CodigoPostal = "77500",
            ////    Colonia = "Villas Tropicales",
            ////    DelegacionMunicipioId = "23005",
            ////    //Estado = "DF",
            ////    Estado = "M23",
            ////    Numero = "mZA. 61 Lte. 8",
            ////    PaisId = "99",
            ////    //Preferido="",
            ////    SecuenciaDireccion = 1,
            ////    TipoDireccionId = "PR"
            ////};
            //newprospect.Direcciones = new List<Direccion>();
            //newprospect.Direcciones.Add(d);
            //#endregion
            //#region Telefono
            //var t = new Telefono()
            //{
            //    LadaTelefono = "123",
            //    PreferidoTelefono = "Y",
            //    SecuenciaTelefono = 1,
            //    Telefono1 = "2345678",
            //    TipoTelefono = "PR"
            //};
            //newprospect.Telefonos = new List<Telefono>();
            //newprospect.Telefonos.Add(t);
            //#endregion
            //#region CorreoElectronico
            //var co = new Correo()
            //{
            //    CorreoElectronico1 = "falonso@hotmail.com",
            //    IndPreferido = "Y",
            //    TipoCorreoElectronicoId = "PERS",
            //    SecuenciaCorreo = "1"
            //};
            //newprospect.Correos = new List<Correo>();
            //newprospect.Correos.Add(co);
            //#endregion
            //#region Tutor
            //var tut = new PadreoTutor()
            //{
            //    FirstName = "Tutor Sandra",
            //    LastName = "Santillan *Cuellar",
            //    Parentesco = "P",
            //    Vive = "N"
            //};
            //newprospect.PadreoTutor = new List<PadreoTutor>();
            //newprospect.PadreoTutor.Add(tut);
            //#endregion
           // ArrangeSecurity();

        }
        private void ArrangeSecurity()
        {
            var Uri = "http://localhost:21292/";
            var client = new HttpClient();
            var secretrhino = ConfigurationManager.AppSettings["secret"];

            //Generacion del Token
            var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Rhino:{0}", secretrhino)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);

            var usr = ConfigurationManager.AppSettings["oauthusr"];

            var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", usr},
               };
            //Recuperacion del Token
            var tokenResponse = client.PostAsync(Uri + "o/Server", new FormUrlEncodedContent(form)).Result;

            var resultado = tokenResponse.Content.ReadAsStringAsync().Result;
            token = JsonConvert.DeserializeObject<Token>(resultado);
        }



        //[TestMethod]
        //public void HelloWorldSucessTest()
        //{
        //    //Arrange
        //    var url = "http://localhost:21292/Api/CreateProspect";

        //    //Act
        //    HttpClient proxy = new HttpClient();
        //    var response = proxy.GetAsync(url).Result;
        //    var result = response.Content.ReadAsStringAsync().Result;




        //    //Assert
        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    Assert.AreNotEqual("Hello world", result);
        //}


        #region Pruebas de creacion
        [TestMethod]
        public void CreatefromURLProspectSucessTest()
        {
            //Arrange
            var url = "http://localhost:21292/Api/CreateProspect";
            HttpClient proxy = new HttpClient();
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Act
            string json = JsonConvert.SerializeObject(newprospect);
            var response = proxy.PostAsJsonAsync(url, newprospect).Result;
            var result = response.Content.ReadAsAsync<ResponseNewProspect>().Result;
            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void CreatefromControlerProspectSuccessTest()
        {
            //Arrange
            //Recuperar la oportunidad que este en etapa de prospecto
            var idoportunidad=_prospectRepository.RetrieveOpportunityId(newprospect.IdBanner);
            newprospect.IdOpportunity = idoportunidad; //new Guid("6036665B-1C3F-E611-80EE-6C3BE5A8C0D0");


            //Act
            var resultado = _createprospectcontroller.Create(newprospect);

            //Assert
            
            if (resultado.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            Assert.AreNotEqual(null, resultado);

        }

        [TestMethod]
        public void CreateMultiplefromControlerProspectSuccessTest()
        {
            List<NewProspect> lstNewProspectos = new List<NewProspect>();

            string[] nombre ={ "Maria","Felipa","Fernanda","Sofia", "Gerarda", "Mariel", "Romina", "Sandra", "Laura", "Victoria", "Gloria",
                "Elena","Erika","Yaretzi","Monserrat","Tania","Hilda","Mary","Mitzy","Karina","Itzel",
            "Gloria","Lupe","Edna","Fanny","Estefany","Casandra","Yolanda","Rocio","Rosario","Beatriz","Patricia","Mariana","Ricarda",
            "Constanza","Esther","Jimena","Carolina",
            "Jennifer","Daniela","Alejandra","Alma","Petronila","Ivonne","Guadalupe","Wendy","Susana","Diana","Maxine","Michel","Sharon",
            "Geraldine","Angela","Joseline","Gregoria"};
            string[] Lastname = { "Ramirez", "jimenez", "Garcia", "Flores", "Barrera", "Vazquez", "Moreno", "Dominguez", "Rubio", "Cabrera",
                "Badillo", "Olivera", "Santos", "Martinez", "Rueda", "Torres", "Reyes", "Patiño", "Jaimes", "Noguera","Gutierrez","Savino",
                "Urban","Zapata","Madero","Ferrer","Lopez","Urvina","Cerda","Montes","Ibañez",
            "Santillan","Borrego","Castillo","Huerta","Rojas","Blanco","Montiel","Peña","Loza","Benitez",
            "Murillo","Sandoval","Zedillo","Alvares","Rivera","Portillo","Cerrano","Riquelme","Zamorano","Terrazas",
                "Fernandes","Villa","Cardoso","Peralta","Arellano","Mendoza","Zarate","Arriaga","Reyna","Jimenez"
            ,"Murano","Centella"};
            string[] vpdArr = {"UAN","UAS" };
            int[] milisegundos = {1000,2000,3000,4000 };
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {


                //Arrange
                #region newP1
                //#region Informacion de Prospecto
                //var newprospectBulk = new NewProspect()
                //{
                //    //IdCRM = new Guid("3638c958-5a3d-e611-80ec-6c3be5a878bc"),
                //    //IdBanner = "00261233",
                //    //Nombre = "Paola",
                //    //Apellidos = "Gomez*Arzamendi",
                //    IdBanner = "00702" + i.ToString("0000"),
                //    //IdBanner = "005390003",
                //    Nombre = nombre[r.Next(0, 50)],
                //    Apellidos = Lastname[r.Next(0, 60)] + "*" + Lastname[r.Next(0, 60)],
                //    Campus = "UAM",
                //    CampusVPD = "UAM",
                //    CiudadNacimiento = "",
                //    ColegioProcedencia = "6980",
                //    Escuela = "ME",
                //    EstadoCivil = "S",
                //    EstadoNacimiento = "",
                //    EstatusSolicitud = "D",
                //    FechaNacimiento = new CustomDate() { Year = 1996, Month = 3, Day = 27 },
                //    Nacionalidad = "ME",
                //    PeriodoId = "201560",
                //    Programa1 = "LC-MEDI-16",
                //    Promedio = "8.3",
                //    ReligionId = "CA",
                //    SegundoNombre = nombre[r.Next(0, 50)],
                //    Sexo = "F",
                //    CodigoTipoadmision = "AD",
                //    CodigoTipoAlumno = "N",
                //    Nivel = "LC",
                //    NumeroSolicitud = 2
                //};
                //#endregion
                //#region Direccion
                //var d = new Direccion()
                //{
                //    Calle = "CALLE 7 #567",
                //    //Ciudad = "Ciudad de México",
                //    CodigoPostal = "97217",
                //    Colonia = "Mérida",
                //    DelegacionMunicipioId = "31050",
                //    Estado = "M31",
                //    //Estado = "D1",//estado mal
                //    Numero = "por 66 y 68",
                //    PaisId = "99",
                //    //Preferido="",
                //    SecuenciaDireccion = 1,
                //    TipoDireccionId = "PR"
                //};
                ////var d = new Direccion()
                ////{
                ////    Calle = "Calle Rodin #45 Villas del Arte",
                ////    //Ciudad = "Ciudad de México",
                ////    CodigoPostal = "77500",
                ////    Colonia = "Villas Tropicales",
                ////    DelegacionMunicipioId = "23005",
                ////    //Estado = "DF",
                ////    Estado = "M23",
                ////    Numero = "mZA. 61 Lte. 8",
                ////    PaisId = "99",
                ////    //Preferido="",
                ////    SecuenciaDireccion = 1,
                ////    TipoDireccionId = "PR"
                ////};
                //newprospectBulk.Direcciones = new List<Direccion>();
                //newprospectBulk.Direcciones.Add(d);
                //#endregion
                //#region Telefono
                //var t = new Telefono()
                //{
                //    LadaTelefono = "999",
                //    PreferidoTelefono = "Y",
                //    SecuenciaTelefono = 1,
                //    Telefono1 = "55665522",
                //    TipoTelefono = "CE"
                //};
                //var t2 = new Telefono()
                //{
                //    LadaTelefono = "999",
                //    PreferidoTelefono = "",
                //    SecuenciaTelefono = 2,
                //    Telefono1 = "2855577",
                //    TipoTelefono = "PR"
                //};
                //newprospectBulk.Telefonos = new List<Telefono>();
                //newprospectBulk.Telefonos.Add(t);
                //newprospectBulk.Telefonos.Add(t2);
                //#endregion
                //#region CorreoElectronico
                //var co = new Correo()
                //{
                //    //CorreoElectronico1 = nombre[r.Next(0, 50)]+ Lastname[r.Next(0, 60)] + i.ToString("000")+"@yahoo.com",
                //    CorreoElectronico1 = newprospectBulk.Nombre + newprospectBulk.IdBanner + "@gmail.com",
                //    IndPreferido = "Y",
                //    TipoCorreoElectronicoId = "PERS",
                //    SecuenciaCorreo = "AAAVJ0AAOAAIPO6ABa"
                //};
                //newprospectBulk.Correos = new List<Correo>();
                //newprospectBulk.Correos.Add(co);
                //#endregion
                //#region Tutor
                //var tut = new PadreoTutor()
                //{
                //    FirstName = nombre[r.Next(0, 50)],
                //    LastName = Lastname[r.Next(0, 60)] + "*" + Lastname[r.Next(0, 60)],
                //    Parentesco = "M",
                //    Vive = "",
                //    TipoDireccion = "PR",
                //    ROWID = "AAAVXGAAOAAIQOVAAl"

                //};
                ////var tut2 = new PadreoTutor()
                ////{
                ////    FirstName = "Jorge",
                ////    LastName = "pepe Cauich",
                ////    Parentesco = "P",
                ////    Vive = "",
                ////    TipoDireccion = "PR",
                ////    ROWID = "AAAVXGAAOAAIQOVAAj"
                ////};
                ////var tut3 = new PadreoTutor()
                ////{
                ////    FirstName = "Jorge",
                ////    LastName = "pepe Cauich",
                ////    Parentesco = "P",
                ////    Vive = "",
                ////    TipoDireccion = "PR",
                ////    ROWID = "AAAVXGAAOAAIQOVAAk"
                ////};
                //newprospectBulk.PadreoTutor = new List<PadreoTutor>();
                //newprospectBulk.PadreoTutor.Add(tut);
                ////newprospect.PadreoTutor.Add(tut2);
                ////newprospect.PadreoTutor.Add(tut3);
                //#endregion
                #endregion
                #region newP1
                #region Informacion de Prospecto
                var newprospectBulk = new NewProspect()
                {
                    //IdCRM = new Guid("dd89fc64-e7c2-e611-80fa-6c3be5a84798"),
                    //IdBanner = "00261233",
                    //Nombre = "Paola",
                    //Apellidos = "Gomez*Arzamendi",
                    // IdBanner = "00701" + i.ToString("0000"),
                    IdBanner = "00315077",
                    Nombre = "Martina",
                    Apellidos = "Flores",
                    Campus = "UAM",
                    //CampusVPD = "UAM",
                    CampusVPD = vpdArr[i],
                    CiudadNacimiento = "",
                    ColegioProcedencia = "11",
                    Escuela = "EN",
                    EstadoCivil = "",
                    EstadoNacimiento = "",
                    EstatusSolicitud = "D",
                    FechaNacimiento = new CustomDate() { Year = 1980, Month = 1, Day = 1 },
                    Nacionalidad = "",
                    PeriodoId = "201710",
                    Programa1 = "LC-ADMI-16",
                    ////PeriodoId = "201560",
                    ////Programa1 = "LC-MEDI-16",
                    Promedio = "8.8",
                    ReligionId = "",
                    SegundoNombre = "",
                    Sexo = "F",
                    CodigoTipoadmision = "AA",
                    CodigoTipoAlumno = "N",
                    Nivel = "LC",
                    NumeroSolicitud = 2
                };
                #endregion
                #region Direccion
                //var d = new Direccion()
                //{
                //    //Calle = "CALLE 7 #567",
                //    ////Ciudad = "Ciudad de México",
                //    //CodigoPostal = "97217",
                //    //Colonia = "Mérida",
                //    //DelegacionMunicipioId = "31050",
                //    //Estado = "M31",
                //    ////Estado = "D1",//estado mal
                //    //Numero = "por 66 y 68",
                //    //PaisId = "99",
                //    ////Preferido="",
                //    //SecuenciaDireccion = 1,
                //    //TipoDireccionId = "PR"
                //};
                ////var d = new Direccion()
                ////{
                ////    Calle = "Calle Rodin #45 Villas del Arte",
                ////    //Ciudad = "Ciudad de México",
                ////    CodigoPostal = "77500",
                ////    Colonia = "Villas Tropicales",
                ////    DelegacionMunicipioId = "23005",
                ////    //Estado = "DF",
                ////    Estado = "M23",
                ////    Numero = "mZA. 61 Lte. 8",
                ////    PaisId = "99",
                ////    //Preferido="",
                ////    SecuenciaDireccion = 1,
                ////    TipoDireccionId = "PR"
                ////};
                //newprospectBulk.Direcciones = new List<Direccion>();
                //newprospectBulk.Direcciones.Add(d);
                #endregion
                #region Telefono
                var t = new Telefono()
                {
                    LadaTelefono = "55",
                    PreferidoTelefono = "Y",
                    SecuenciaTelefono = 1,
                    Telefono1 = "5555555555",
                    TipoTelefono = "PR"
                };

                newprospectBulk.Telefonos = new List<Telefono>();
                newprospectBulk.Telefonos.Add(t);
                #endregion
                #region CorreoElectronico
                var co = new Correo()
                {
                    //CorreoElectronico1 = nombre[r.Next(0, 50)]+ Lastname[r.Next(0, 60)] + i.ToString("000")+"@yahoo.com",
                    CorreoElectronico1 = "marged@ahoo.com.m",
                    IndPreferido = "N",
                    TipoCorreoElectronicoId = "PERS",
                    SecuenciaCorreo = "AAAVJ0AAXAACPZXAA3"
                };
                newprospectBulk.Correos = new List<Correo>();
                newprospectBulk.Correos.Add(co);
                #endregion
                #region Tutor
                //var tut = new PadreoTutor()
                //{
                //    FirstName = nombre[r.Next(0, 50)],
                //    LastName = Lastname[r.Next(0, 60)] + "*" + Lastname[r.Next(0, 60)],
                //    Parentesco = "M",
                //    Vive = "",
                //    TipoDireccion = "PR",
                //    ROWID = "AAAVXGAAOAAIQOVAAl"

                //};
                //var tut2 = new PadreoTutor()
                //{
                //    FirstName = "Jorge",
                //    LastName = "pepe Cauich",
                //    Parentesco = "P",
                //    Vive = "",
                //    TipoDireccion = "PR",
                //    ROWID = "AAAVXGAAOAAIQOVAAj"
                //};
                //var tut3 = new PadreoTutor()
                //{
                //    FirstName = "Jorge",
                //    LastName = "pepe Cauich",
                //    Parentesco = "P",
                //    Vive = "",
                //    TipoDireccion = "PR",
                //    ROWID = "AAAVXGAAOAAIQOVAAk"
                //};
                //newprospectBulk.PadreoTutor = new List<PadreoTutor>();
                //newprospectBulk.PadreoTutor.Add(tut);
                //newprospect.PadreoTutor.Add(tut2);
                //newprospect.PadreoTutor.Add(tut3);
                #endregion
                #endregion
                lstNewProspectos.Add(newprospectBulk);
            }


            //
            //List<Task> tareasNewProspect = new List<Task>();


            foreach (var newprospectBulk in lstNewProspectos)
            {
                //var tarea = Task.Factory.StartNew(() =>
                //{

                    //var idoportunidad = _prospectRepository.RetrieveOpportunityId(newprospectBulk.IdBanner);
                    //newprospectBulk.IdOpportunity = idoportunidad; //new Guid("6036665B-1C3F-E611-80EE-6C3BE5A8C0D0");


                    //Act
                    var resultado = _createprospectcontroller.Create(newprospectBulk);


                    //Assert

                    if (resultado.StatusCode != HttpStatusCode.OK)
                    {
                        var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                        _logger.Infomacion(msgresultado);
                    }

                    Assert.AreEqual(HttpStatusCode.OK, resultado.StatusCode);
                    Assert.AreNotEqual(null, resultado);
                //});
                //tareasNewProspect.Add(tarea);
                //System.Threading.Thread.Sleep(5000);

                //System.Threading.Thread.Sleep(milisegundos[r.Next(0,3)]);
            }

            //Task.WaitAll(tareasNewProspect.ToArray());



            //Debug.WriteLine("IdBanner:"+ newprospect.IdBanner);
            //    Debug.WriteLine("Nombre:" + newprospect.Nombre+" "+newprospect.Apellidos);

            //Recuperar la oportunidad que este en etapa de prospecto


        }

        [TestMethod]
        public void CreateWithoutNoRequiredSucessTest()
        {

            //Arrange
            #region Limpiar campos no requeridos
            newprospect.Nombre = "Noe";
            newprospect.NumeroSolicitud = default(int?);
            newprospect.SegundoNombre = default(string);
            newprospect.Apellidos = "Torres";
            newprospect.FechaNacimiento = default(CustomDate);
            //newprospect.Campus = default(string);
            // newprospect.EstatusSolicitud = default(string);//Esta requerido previamente
            newprospect.Sexo = default(string);
            newprospect.Nacionalidad = default(string);
            newprospect.ReligionId = default(string);
            newprospect.EstadoCivil = default(string);
            newprospect.EstadoNacimiento = default(string);
            newprospect.CiudadNacimiento = default(string);
            //newprospect.ColegioProcedencia= default(string);//Esta requerido previamente
            newprospect.Promedio = default(string);
            // newprospect.PeriodoId= default(string);//Esta requerido previamente
            //campos no requeridos en direccion
            foreach (var d in newprospect.Direcciones)
            {
                d.SecuenciaDireccion = default(int);
            }

            newprospect.PadreoTutor = null;
            //newprospect.PadreoTutor.MiddleName = default(string);
            //newprospect.PadreoTutor.Parentesco = default(string);
            //newprospect.PadreoTutor.LastName = "Torres";
            //newprospect.PadreoTutor.Vive = default(string);
            #endregion


            //Act
            var resultado = _createprospectcontroller.Create(newprospect);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            Assert.AreNotEqual(null, resultado);

        }

        #endregion

        #region Pruebas Generales

        [TestMethod]
        public void InvalidModelErrorTest()
        {
            var resultado = _createprospectcontroller.Create(new NewProspect());

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);


        }

        [TestMethod]
        public void SexoPicklistResolverErrorTest()
        {
            //Arrange
            newprospect.Sexo = "X";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el picklist de Sexo";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;


            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

        }

        [TestMethod]
        public void CampusVPDEntityreferenceResolverErrorTest()
        {

            //Arrange
            newprospect.CampusVPD = "CINVLD";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de CampusVPD";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;



            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }


        [TestMethod]
        public void CampusEntityreferenceResolverErrorTest()
        {

            //Arrange
            newprospect.Campus = "CINVLD";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Campus: CINVLD";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;



            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        [TestMethod]
        public void ColegioEntityreferenceResolverErrorTest()
        {

            //Arrange
            newprospect.ColegioProcedencia = "CINVLD";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Colegio: CINVLD";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;



            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        [TestMethod]
        public void EscuelaEntityreferenceResolverErrorTest()
        {

            //Arrange
            newprospect.Escuela = "!=";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Escuela: !=";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;



            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }


        [TestMethod]
        public void EstadoCivilOptionSetValueResolverErrorTest()
        {

            //Arrange
            newprospect.EstadoCivil = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el picklist de Estado Civil: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;



            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }


        [TestMethod]
        public void EstadoNacimientoEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.EstadoNacimiento = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Estado Nacimiento: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;



            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        [TestMethod]
        public void NacionalidadEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.Nacionalidad = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Nacionalidad: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        [TestMethod]
        public void PeriodoEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.PeriodoId = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Periodo: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        [TestMethod]
        public void ProgramaEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.Programa1 = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Programa: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        [TestMethod]
        public void ReligionEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.ReligionId = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el Lookup de Religion: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }


        [TestMethod]
        public void TipoAdmisionEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.CodigoTipoadmision = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el picklist de Tipo Admision: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }


        [TestMethod]
        public void TipoAlumnoEntityReferenceValueResolverErrorTest()
        {

            //Arrange
            newprospect.CodigoTipoAlumno = "!";

            //Act
            var resultado = _createprospectcontroller.Create(newprospect);
            var expectedErrorMessage = "No se pudo resolver el picklist Tipo Alumno: !";
            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
        }

        #endregion




        #region Pruebas para direccion


        //[TestMethod]
        //public void UnsolvedZipCodeTest()
        //{
        //    newprospect.Direcciones[0].CodigoPostal = "0";
        //    var resultado = _createprospectcontroller.Create(newprospect);

        //    var expectedErrorMessage = "No se pudo resolver el Lookup de Codigo Postal: 0";
        //    var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
        //    var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
        //    var actualErrorMessage = httperrormessage.Message;

        //    //Assert
        //    Assert.AreEqual(false, resultado.IsSuccessStatusCode);
        //    Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
        //    Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        //}

        [TestMethod]
        public void UnsolvedStateTest()
        {
            newprospect.Direcciones[0].Estado = "ZZZ";
            var resultado = _createprospectcontroller.Create(newprospect);

           // var expectedErrorMessage = "No se pudo resolver el Lookup de Estado de la direccion ZZZ";


            //var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            //var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            //var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(true, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void UnsolvedMunicipioTest()
        {
            newprospect.Direcciones[0].DelegacionMunicipioId = "!";
            var resultado = _createprospectcontroller.Create(newprospect);

            //var expectedErrorMessage = "No se pudo resolver el Lookup de Delegacion/Municipio: !";


            //var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            //var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            //var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(true, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void UnsolvedCountryTest()
        {
            newprospect.Direcciones[0].PaisId = "ZZZZZ";
            var resultado = _createprospectcontroller.Create(newprospect);

            var expectedErrorMessage = "No se pudo resolver el Lookup de Pais de la direccion: ZZZZZ";


            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void UnsolvedTipoDireccionTest()
        {
            newprospect.Direcciones[0].TipoDireccionId = "99";
            var resultado = _createprospectcontroller.Create(newprospect);

            var expectedErrorMessage = "No se pudo resolver el Lookup de TipoDireccion: 99";


            var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            var actualErrorMessage = httperrormessage.Message;

            //Assert
            Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void ForeingCountryTest()
        {
            newprospect.Direcciones[0].PaisId = "20";
            var resultado = _createprospectcontroller.Create(newprospect);
            //var expectedErrorMessage = "No se pudo resolver el Lookup de Estado de la direccion ZZZ";
            //var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            //var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            //var actualErrorMessage = httperrormessage.Message;

            //Assert
            //Assert.AreEqual(false, resultado.IsSuccessStatusCode);
            //Assert.AreEqual(HttpStatusCode.BadRequest, resultado.StatusCode);
            //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            Assert.AreNotEqual(null, resultado);

        }


        [TestMethod]
        public void DireccionUpdateEstadoIncorrectoTest()
        {
            newprospect.Direcciones[0].Estado = "666";
            var resultado = _createprospectcontroller.Create(newprospect);

            //var expectedErrorMessage = "El Estado no esta en el catalogo de CRM";


            //var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            //var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            //var actualErrorMessage = httperrormessage.Values;

            //Assert
            Assert.AreEqual(true, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void DireccionUpdateEstadoYmunicipioErroneosTest()
        {
            #region Direccion
            var d = new Direccion()
            {
                Calle = "Aguascalientes",
                //Ciudad = "Ciudad de México",
                CodigoPostal = "0000",
                Colonia = "SAN FRANCISCO",
                DelegacionMunicipioId = "00000",
                Estado = "xx",
                //Estado = "D1",//estado mal
                Numero = "1234",
                PaisId = "99",
                //Preferido="",
                SecuenciaDireccion = 1,
                TipoDireccionId = "PA"
            };


            newprospect.Direcciones = new List<Direccion>();
            newprospect.Direcciones.Add(d);
            #endregion
            var resultado = _createprospectcontroller.Create(newprospect);

            //var expectedErrorMessage = "El Estado no esta en el catalogo de CRM";


            //var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            //var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            //var actualErrorMessage = httperrormessage.Values;

            //Assert
            Assert.AreEqual(true, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void TelefonoUpdateTest()
        {
            #region Telefono
            var t = new Telefono()
            {
                LadaTelefono = "123",
                PreferidoTelefono = "Y",
                SecuenciaTelefono = 1,
                Telefono1 = "2345678",
                TipoTelefono = "PR"
            };
            newprospect.Telefonos = new List<Telefono>();
            newprospect.Telefonos.Add(t);
            #endregion
            var resultado = _createprospectcontroller.Create(newprospect);

            //var expectedErrorMessage = "El Estado no esta en el catalogo de CRM";


            //var httpresponsemessage = resultado.Content.ReadAsStringAsync().Result;
            //var httperrormessage = JsonConvert.DeserializeObject<HttpError>(httpresponsemessage);
            //var actualErrorMessage = httperrormessage.Values;

            //Assert
            Assert.AreEqual(true, resultado.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);


        }

        [TestMethod]
        public void CreacionfromControlerProspectSuccessTest()
        {
            //Arrange
            #region Informacion de Prospecto
            newprospect = new NewProspect()
            {
                //IdCRM = new Guid("3638c958-5a3d-e611-80ec-6c3be5a878bc"),
                IdBanner = "00294917",
                Nombre = "Aida",
                Apellidos = "Jimenez*Garcia",
                Campus = "UAC",
                CampusVPD = "UAS",
                CiudadNacimiento = "DF",
                ColegioProcedencia = "1001",
                Escuela = "AC",
                EstadoCivil = "S",
                EstadoNacimiento = "DF",
                EstatusSolicitud = "C",
                FechaNacimiento = new CustomDate() {Year = 1993, Month = 12, Day = 28 },
                Nacionalidad = "AL",
                PeriodoId = "201560",
                Programa1 = "LC-INAM-13",
                Promedio = "9.0",
                ReligionId = "CA",
                SegundoNombre = "Christian",
                Sexo = "M",
                CodigoTipoadmision = "AA",
                CodigoTipoAlumno = "S",
                Nivel="LC",
                NumeroSolicitud=1
            };
            #endregion
            #region Direccion
            var d = new Direccion()
            {
                Calle = "Aguascalientes",
                //Ciudad = "Ciudad de México",
                CodigoPostal = "01809",
                Colonia = "SAN FRANCISCO",
                DelegacionMunicipioId = "09001",
                Estado = "DF",
                //Estado = "D1",//estado mal
                Numero = "1234",
                PaisId = "99",
                //Preferido="",
                SecuenciaDireccion = 1,
                TipoDireccionId = "PA"
            };
            //var d = new Direccion()
            //{
            //    Calle = "Calle Rodin #45 Villas del Arte",
            //    //Ciudad = "Ciudad de México",
            //    CodigoPostal = "77500",
            //    Colonia = "Villas Tropicales",
            //    DelegacionMunicipioId = "23005",
            //    //Estado = "DF",
            //    Estado = "M23",
            //    Numero = "mZA. 61 Lte. 8",
            //    PaisId = "99",
            //    //Preferido="",
            //    SecuenciaDireccion = 1,
            //    TipoDireccionId = "PR"
            //};
            newprospect.Direcciones = new List<Direccion>();
            newprospect.Direcciones.Add(d);
            #endregion
            #region Telefono
            var t = new Telefono()
            {
                LadaTelefono = "123",
                PreferidoTelefono = "Y",
                SecuenciaTelefono = 1,
                Telefono1 = "2345678",
                TipoTelefono = "PR"
            };
            newprospect.Telefonos = new List<Telefono>();
            newprospect.Telefonos.Add(t);
            #endregion
            #region CorreoElectronico
            var co = new Correo()
            {
                CorreoElectronico1 = "falonso@hotmail.com",
                IndPreferido = "Y",
                TipoCorreoElectronicoId = "PERS",
                SecuenciaCorreo = "1"
            };
            newprospect.Correos = new List<Correo>();
            newprospect.Correos.Add(co);
            #endregion
            #region Tutor
            var tut = new PadreoTutor()
            {
                FirstName = "Tutor",
                LastName = "Perez *Gomez",
                Parentesco = "P",
                Vive = "Y"
            };
            newprospect.PadreoTutor = new List<PadreoTutor>();
            newprospect.PadreoTutor.Add(tut);
            #endregion
            //Recuperar la oportunidad que este en etapa de prospecto
            //var idoportunidad = _prospectRepository.RetrieveOpportunityId(newprospect.IdBanner);
            //newprospect.IdOpportunity = idoportunidad; //new Guid("6036665B-1C3F-E611-80EE-6C3BE5A8C0D0");


            //Act
            var resultado = _createprospectcontroller.Create(newprospect);

            //Assert

            if (resultado.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = resultado.Content.ReadAsStringAsync().Result;
                _logger.Infomacion(msgresultado);
            }

            Assert.AreEqual(HttpStatusCode.Created, resultado.StatusCode);
            Assert.AreNotEqual(null, resultado);

        }
        #endregion




    }
}

////ACT
//var task = proxy.GetAsync(url)
//   .ContinueWith((taskwithresponse) =>
//   {
//       var response = taskwithresponse.Result;
//       var readtask = response.Content.ReadAsAsync<List<State>>();
//       readtask.Wait();
//       resultado = readtask.Result;
//   });
//task.Wait();
