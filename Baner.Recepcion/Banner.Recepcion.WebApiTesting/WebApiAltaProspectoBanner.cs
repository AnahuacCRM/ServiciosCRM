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
    public class WebApiAltaProspectoBanner
    {
        AltaProspectoBannerController AltaProspectoBanner;
        ILogger _ilogger;
        IBannerProcessor _bannerProcessor;
        IBannerRepository _ibannerRepository;
        IOpportunityRepository _iOpotunityRepository;
        CrearCuentaBanner ProspectoC;

        [TestInitialize]
        public void Initializate()
        {
            _ilogger = new DebugerLogger();
            _ibannerRepository = new BannerRepository();
            _iOpotunityRepository = new OpportunityRepository();
            _bannerProcessor = new BannerProcessor(_ilogger, _ibannerRepository, _iOpotunityRepository);
            AltaProspectoBanner = new AltaProspectoBannerController(_ilogger, _bannerProcessor);

            AltaProspectoBanner.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            ProspectoC = new CrearCuentaBanner
            {
                id_Cta = "IdCuentaSecundIdCuentaSecundIdCuentaSecund",
                id_Oportunidad = "IdOpoSecundIdOpoSecundIdOpoSecundIdOpoSecund",
                Nombre = "Cristian ",
                Segundo_Nombre = ":",
                ApellidoPaterno = "Barraza",
                Apellido_Materno = "Nevarez",
                Fecha_Nacimiento = new CustomDate { Year = 2001, Month = 2, Day = 3 },
                Correo = new Correo { Correo_Electronico = "cris@hotmial.com" },
                Campus = "UAN",
                Sexo = "M",
                Telefono = new Telefono { LadaTelefono = "66", Telefono1 = "201770" },
                Id_Banner_Vinculante = "",
                Periodo = "201760",
                VPD = "UAN",
                Programa = "LC-MEDI-16"


            };

        }



        [TestMethod]
        public void ProbarAltaProspectoBanner()
        {

            var Res = AltaProspectoBanner.AltaProespectoBanner(ProspectoC);

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
