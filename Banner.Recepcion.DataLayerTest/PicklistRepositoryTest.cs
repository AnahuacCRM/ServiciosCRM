using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.DataLayer.CRM;

namespace Banner.Recepcion.DataLayerTest
{
    [TestClass]
    public class PicklistRepositoryTest
    {
        ILogger _logger;
        IPickListRepository picklistRepository;
        //IServerConnection serverconnection;

        [TestInitialize]
        public void Init()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            picklistRepository = new PickListRepository();
        }

        [TestMethod]
        public void CorreopreferidoSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaCorreoPreferido();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void EstadoCivilSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaEstadoCivil();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void EstatusSolicitudSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaEstatusSolicitud();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ParentescoSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaParentesco();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void SexoSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaSexo();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void TelefonoPreferidoSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaTelefonoPreferido();

            //Assert


            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void TipoAdmisionSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaTipoAdmision();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void TipoAlumnoSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaTipoAlumno();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ViveSucessTest()
        {
            //Arrange

            //Act
            var resultado = picklistRepository.ListaVive();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
        }

        //[TestMethod]
        //public void DireccionPreferidoSucessTest()
        //{
        //    //Arrange

        //    //Act
        //    var resultado = picklistRepository.ListaDIreccionPreferido();

        //    //Assert
        //    Assert.AreNotEqual(0, resultado.Count);
        //}
    }
}
