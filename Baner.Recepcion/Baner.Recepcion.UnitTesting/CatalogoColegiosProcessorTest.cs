using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class CatalogoColegiosProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CatalogoColegios catalogoColegios;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de CatalogoColegios
            catalogoColegios = new CatalogoColegios();


            catalogoColegios.ClaveColegio = "1000";
            catalogoColegios.NombreColegio = "I ANGLO MEXICANO (HERMOSILLO)";
            catalogoColegios.Calle = "Periférico Poniente";
            catalogoColegios.Numero = "98";
            catalogoColegios.Colonia = " Palmar del Sol";
            catalogoColegios.Municipio = "12";
            catalogoColegios.Estado = "12";
            catalogoColegios.Pais = "56";
            catalogoColegios.CodigoPostal = "83250";
            catalogoColegios.TipoColegio = "A";
            Contactos contactos = new Contactos();
            contactos.VPDI = "UAN";
            contactos.Contacto = "Juan Ramirez Escutia";
            contactos.TipoContacto = "1";
            catalogoColegios.lstContactos = new List<Contactos>();
            catalogoColegios.lstContactos.Add(contactos);      
            
            #endregion
        }

        [TestMethod]
        public void CatalogoColegiosTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCatalogoColegios(catalogoColegios);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>()));
        }

        /// <summary>
        /// Validar con no requeridos
        /// </summary>
        [TestMethod]
        public void CatalogoColegiosNoRequeridosTest()
        {
            //Arrange
            #region Informacion de CatalogoColegios
            catalogoColegios = new CatalogoColegios()
            {
                ClaveColegio = "1000",
                NombreColegio = "I ANGLO MEXICANO (HERMOSILLO)",
                Calle = default(string),
                Numero = default(string),
                Colonia = default(string),
                Municipio = default(string),
                Estado = default(string),
                Pais = default(string),
                CodigoPostal = default(string),
               lstContactos= null,
                TipoColegio = "A"
            };
            #endregion
           
            mockProspectRepository.Setup(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCatalogoColegios(catalogoColegios);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>()));
        }


        /// <summary>
        /// Validar  requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CatalogoColegiosRequeridosSucessTest()
        {
            //Arrange
            catalogoColegios.ClaveColegio = default(string);
            catalogoColegios.NombreColegio = default(string);
            catalogoColegios.TipoColegio = default(string);

            mockProspectRepository.Setup(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCatalogoColegios(catalogoColegios);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Clave colegio es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "Nombre colegio es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "Tipo Colegio es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  LongitudExcedida
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CatalogoColegiosLongitudExcedidaSucessTest()
        {
            //Arrange
            catalogoColegios.ClaveColegio = "123456789";
            catalogoColegios.NombreColegio = "123456891234567891345689123456789132456798";
            catalogoColegios.Pais = "13245654";

            mockProspectRepository.Setup(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCatalogoColegios(catalogoColegios);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima de Clave colegio  es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Nombre colegio  es de 30 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de País es de 5 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  LongitudExcedida contactos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CatalogoColegiosLongitudExcedidaContactosSucessTest()
        {
            //Arrange
            Contactos contactos = new Contactos();
            contactos.VPDI = "UANsdasdasdasd";
            contactos.Contacto = "12345678911234567891123456789112345678911234567891123456789112345678911234567891123456789112345678911234567891123456789112345678911234567891123456789112345678911234567891123456789112345678911234567891123456789112345678911234567891123456789112345678911234567891";
            contactos.TipoContacto = "11543kjllkjl1";
            catalogoColegios.lstContactos = new List<Contactos>();
            catalogoColegios.lstContactos.Add(contactos);

            mockProspectRepository.Setup(r => r.UpdateCatalogoColegios(It.IsAny<CatalogoColegios>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCatalogoColegios(catalogoColegios);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima de VPDI es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Contacto es de 230 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de Tipo contacto es de 4 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }


    }
}
