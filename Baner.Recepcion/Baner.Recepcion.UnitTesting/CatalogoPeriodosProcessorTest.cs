using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class CatalogoPeriodosProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CatalogoPeriodos catalogoPeriodos;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de CatalogoPeriodos
            catalogoPeriodos = new CatalogoPeriodos()
            {
                ClavePeriodo = "111111",
                DescripcionPeriodo = "Periodo inicial",
                FechaInicioPeriodo = new CustomDate() {Day=1,Month=5,Year=2015 },
                FechaFinPeriodo = new CustomDate() { Day = 1, Month = 5, Year = 2015 },
                FechaInicioAlojamiento = new CustomDate() { Day = 1, Month = 5, Year = 2015 },
                FechaFinAlojamiento = new CustomDate() { Day = 1, Month = 5, Year = 2015 }

            };
            #endregion
        }

        [TestMethod]
        public void CatalogoPeriodosTest()
        {
     
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCatalogoPeriodos(It.IsAny<CatalogoPeriodos>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCatalogoPeriodos(catalogoPeriodos);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCatalogoPeriodos(It.IsAny<CatalogoPeriodos>()));
        }

        /// <summary>
        /// Validar  Excedidos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CatalogoPeriodosExcededLongitudeSucessTest()
        {
            //Arrange
            catalogoPeriodos.ClavePeriodo = "1213213213213212132132132121";
            catalogoPeriodos.DescripcionPeriodo = "0123456789123456789134567891324567891345678954654879531";
            mockProspectRepository.Setup(r => r.UpdateCatalogoPeriodos(It.IsAny<CatalogoPeriodos>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCatalogoPeriodos(catalogoPeriodos);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 2;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima de Clave Periodo  es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Descripcion periodo  es de 30 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        // <summary>
        /// Validar  Requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CatalogoPeriodosrequeridosSucessTest()
        {
            //Arrange
            catalogoPeriodos.ClavePeriodo = default(string);
            catalogoPeriodos.DescripcionPeriodo = default(string);
            catalogoPeriodos.FechaInicioPeriodo = default(CustomDate);
            catalogoPeriodos.FechaFinPeriodo = default(CustomDate);
            catalogoPeriodos.FechaInicioAlojamiento = default(CustomDate);
            catalogoPeriodos.FechaFinAlojamiento = default(CustomDate);
            mockProspectRepository.Setup(r => r.UpdateCatalogoPeriodos(It.IsAny<CatalogoPeriodos>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCatalogoPeriodos(catalogoPeriodos);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 6;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Clave Periodo es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "Descripcion periodo es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "Fecha inicio periodo es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "Fecha fin periodo es requerido";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "Fecha inicio alojamiento es requerido";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "Fecha fin alojamiento periodo es requerido";
                var actualErrorMessage6 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        // <summary>
        /// Validar  Fecha
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CatalogoPeriodosFechaValidaSucessTest()
        {
            //Arrange

            catalogoPeriodos.FechaInicioPeriodo =  new CustomDate() { Day = 1, Month = 15, Year = 2015 };
            //catalogoPeriodos.FechaFinPeriodo = new CustomDate() { Day = 1, Month = 150, Year = 2015 };
            //catalogoPeriodos.FechaInicioAlojamiento = new CustomDate() { Day = 1, Month = 5, Year = 201555 };
            //catalogoPeriodos.FechaFinAlojamiento = new CustomDate() { Day = 111, Month = 5, Year = 2015 };
            mockProspectRepository.Setup(r => r.UpdateCatalogoPeriodos(It.IsAny<CatalogoPeriodos>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCatalogoPeriodos(catalogoPeriodos);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El mes proporcionado es incorrecto";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                //var expectedErrorMessage2 = "El mes proporcionado es incorrecto";
                //var actualErrorMessage2 = ex.Errors.ElementAt(1);
                //Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                //var expectedErrorMessage3 = "El año proporcionado es incorrecto";
                //var actualErrorMessage3 = ex.Errors.ElementAt(2);
                //Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                //var expectedErrorMessage4 = "El día proporcionado es incorrecto";
                //var actualErrorMessage4 = ex.Errors.ElementAt(3);
                //Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

              

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }
    }
}
