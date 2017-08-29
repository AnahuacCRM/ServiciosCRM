using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.BusinessTypes;
using System.Collections.Generic;
using Baner.Recepcion.DataLayer.CRM;

namespace Banner.Recepcion.DataLayerTest
{
    [TestClass]
    public class ProspectRepositoryTest
    {
        //ILogger _logger;
        ICatalogRepository catalogrepository;
        IPickListRepository picklistRepository;
        IProspectRepository prospectRepository;
        IOpportunityRepository opportunityRepository;
        //IServerConnection serverconnection;
        [TestInitialize]
        public void Init()
        {
            //_logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            catalogrepository = new CatalogRepository();
            picklistRepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            prospectRepository = new ProspectRepository( catalogrepository, picklistRepository, opportunityRepository);
        }

        //[TestMethod]
        //public void NewProspecctSucessTest()
        //{
        //    //Arrange


        //    #region Informacion de Prospecto
        //    var p = new NewProspect()
        //    {
        //        Nombre = "Argelis",
        //        Apellidos = "Torres *Cuellar",
        //        Campus = "UAC",
        //        CampusVPD = "UAC",
        //        CiudadNacimiento = "DF",
        //        ColegioProcedencia = "CW",
        //        Escuela = "CW",
        //        EstadoCivil = "S",
        //        EstadoNacimiento = "DF",
        //        EstatusSolicitud = "C",
        //        FechaNacimiento = new CustomDate() { Year = 1983, Month = 5, Day = 30 },
        //        Nacionalidad = "AL",
        //        PaisNacimiento = "001",
        //        PeriodoId = "201645",
        //        Programa1 = "ACT",
        //        Promedio = "9.0",
        //        ReligionId = "CA",
        //        SegundoNombre = "Noe",
        //        Sexo = "M",
        //        CodigoTipoadmision = "AA",
        //        CodigoTipoAlumno = "S"
        //    };
        //    #endregion
        //    #region Direccion
        //    var d = new Direccion()
        //    {
        //        Calle = "Aguascalientes",
        //        Ciudad = "DF",
        //        CodigoPostalId = "01270",
        //        Colonia = "Golondrinas",
        //        DelegacionMunicipioId = "09001",
        //        Estado = "DF",
        //        Numero = "1234",
        //        PaisId = "99",
        //        //Preferido="",
        //        SecuenciaDireccion = 1,
        //        TipoDireccionId = "PA"
        //    };
        //    p.Direcciones = new List<Direccion>();
        //    p.Direcciones.Add(d);
        //    #endregion
        //    #region Telefono
        //    var t = new Telefono()
        //    {
        //        LadaTelefono = "123",
        //        PreferidoTelefono = "Principal",
        //        SecuenciaTelefono = 1,
        //        Telefono1 = "2345678",
        //        TipoTelefono = "PR"
        //    };
        //    p.Telefonos = new List<Telefono>();
        //    p.Telefonos.Add(t);
        //    #endregion
        //    #region CorreoElectronico
        //    var co = new Correo()
        //    {
        //        CorreoElectronico1 = "falonso@hotmail.com",
        //        IndPreferido = "Preferido",
        //        TipoCorreoElectronicoId = "PERS",
        //        SecuenciaCorreo = 1
        //    };
        //    p.Correos = new List<Correo>();
        //    p.Correos.Add(co);
        //    #endregion
        //    #region Tutor
        //    var tut = new PadreoTutor()
        //    {
        //        FirstName = "Argelis",
        //        LastName = "Torres",
        //        MiddleName = "Noe",
        //        Parentesco = "P",
        //        Vive = "Si"
        //    };
        //    p.PadreoTutor = tut;
        //    #endregion
        //    //Act
        //    var IdProspecto = prospectRepository.Create(p);


        //    //Assert
        //}
    }
}
