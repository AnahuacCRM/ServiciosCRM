using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.BusinessTypes;

namespace Banner.Recepcion.DataLayerTest
{
    [TestClass]
    public class CatalogRepositoryTests
    {
        ILogger _logger;
        ICatalogRepository catalogrepository;
        //IServerConnection serverconnection;

        [TestInitialize]
        public void Init()
        {
            _logger = new DebugerLogger();
            //serverconnection = new ServerConnection();
            catalogrepository = new CatalogRepository();


        }

        [TestMethod]
        public void CampusSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaCampus();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void ColegioSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaColegio();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void EscuelaSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaEscuela();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void EstadoSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaEstado();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }


        [TestMethod]
        public void MunicipioSucessTest()
        {
            //Arrange
            var mun = new Municipio();
            mun.CodigoMunicipio ="09010";
            mun.Estado = "M09";
            
            //Act
            var resultado = catalogrepository.ListaMunicipio(mun);

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void NacionalidadSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaNacionalidad();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void PaisSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaPais();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void PeriodoSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaPeriodo();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void ProgramaSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaProgramas();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }



        [TestMethod]
        public void ReligionSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaReligion();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        //[TestMethod, ExpectedException(typeof(ArgumentException))]
        //public void CiudadesSucessTest()
        //{
        //    //Arrange

        //    //Act
        //    var resultado = catalogrepository.ListaCiudades();

        //    //Assert
        //    Assert.AreNotEqual(0, resultado.Count);

        //}

        [TestMethod]
        public void TipoDireccionesSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaTipoDireccion();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void CodigoPostalSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaCodigoPostal("01000");
            resultado = catalogrepository.ListaCodigoPostal("01000");
            resultado = catalogrepository.ListaCodigoPostal("45000");
            resultado = catalogrepository.ListaCodigoPostal("35000");
            resultado = catalogrepository.ListaCodigoPostal("25000");
            resultado = catalogrepository.ListaCodigoPostal("46000");

            //Assert
            Assert.AreNotEqual(0, resultado.Count);
            _logger.Infomacion(string.Format("Elementos recuperados {0}", resultado.Count));

        }

        [TestMethod]
        public void TipoTelefonoSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaTipoTelefono();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        [TestMethod]
        public void TipoCorreoSucessTest()
        {
            //Arrange

            //Act
            var resultado = catalogrepository.ListaTipoCorreo();

            //Assert
            Assert.AreNotEqual(0, resultado.Count);

        }

        //[TestMethod]
        //public void ColoniaSucessTest()
        //{
        //    //Arrange

        //    //Act
        //    var resultado = catalogrepository.ListaColonias();

        //    //Assert
        //    Assert.AreNotEqual(0, resultado.Count);

        //}

        //[TestMethod]
        //public void RetrieveColoniaSucessTest()
        //{
        //    //Arrange

        //    //Act
        //    var resultado = catalogrepository.RetrieveColonias("la purisima");

        //    resultado = catalogrepository.RetrieveColonias("tultepec");

        //    //Assert
        //    Assert.AreNotEqual(0, resultado.Count);

        //}

        //[TestMethod]
        //public void RetrieveCiudadesSucessTest()
        //{
        //    //Arrange

        //    //Act
        //    var resultado = catalogrepository.RetrieveCiudades("Francisco I. Madero");

        //    //Assert
        //    Assert.AreNotEqual(0, resultado.Count);

        //}

    }
}
