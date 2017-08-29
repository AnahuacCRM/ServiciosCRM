using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using Baner.Recepcion.BusinessTypes;
using System.Net;

namespace Banner.Recepcion.WebApiTesting
{
    [TestClass]
    public class WebApiCoincidencias
    {
        srvGestionCoincidenciasController _GestionCoincidencias;
        ILogger _ilogger;
        IBannerProcessor _bannerProcessor;
        IBannerRepository _ibannerRepository;
        IOpportunityRepository _iOpotunityRepository;
        Coincidencias ProspectoC;

        [TestInitialize]
        public void Initialize()
        {
            _ilogger = new DebugerLogger();
            _ibannerRepository = new BannerRepository();
            _iOpotunityRepository = new OpportunityRepository();
            _bannerProcessor = new BannerProcessor(_ilogger,_ibannerRepository, _iOpotunityRepository);
            _GestionCoincidencias = new srvGestionCoincidenciasController(_ilogger, _bannerProcessor);

            _GestionCoincidencias.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };
            //Gustavo1 Carreno Nuñez
            ProspectoC = new Coincidencias
            {
                //Nombre = "Gustavo1",
                //Segundo_Nombre = "",
                //Apellido_Paterno = "Carreno",
                //Apellido_Materno= "Nuñez",
                //Fecha_Nacimiento = null,
                //Correo_Electrónico = ":",
                //Sexo = ":",
                //Codigo_Area = ":",
                //Numero_Telefonico = ":",
                //VPD = "UAN"

                Nombre = "Juan",
                Segundo_Nombre = "",
                Apellido_Paterno = "Perez",
                Fecha_Nacimiento = null,
                Correo_Electrónico = ":",
                Sexo = ":",
                Codigo_Area = ":",
                Numero_Telefonico = ":",
                VPD = "UAN"
            };


        }
        [TestMethod]
        public void CoincidenciasControler()
        {
            var Res = _GestionCoincidencias.Consultar(ProspectoC);
            if (Res.StatusCode != HttpStatusCode.OK)
            {
                var msgresultado = Res.Content.ReadAsStringAsync().Result;
                _ilogger.Infomacion(msgresultado);
            }
           
            var resultado =
            Res.Content.ReadAsStringAsync().Result;
           

            Assert.AreEqual(HttpStatusCode.OK, Res.StatusCode);
            Assert.AreNotEqual(false, Res);
        }
    }
}
