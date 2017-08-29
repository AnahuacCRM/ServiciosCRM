using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.DataInterfaces;
using Moq;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class DatosPrepaProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        DatosPrepa datosPrepa;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de DatosPrepa
            datosPrepa = new DatosPrepa()
            {
                IdBanner = "00",
                Preparatoria = "1001",
                PromedioPreparatoria = "9.66",
                VPDI="UAN"
            };
            #endregion
        }

        [TestMethod]
        public void DatosPrepaTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateDatosPrepa(It.IsAny<DatosPrepa>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateDatosPrepa(datosPrepa);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateDatosPrepa(It.IsAny<DatosPrepa>()));
        }

        /// <summary>
        /// Campos No Requeridos
        /// </summary>
        [TestMethod]
        public void DatosPrepaCamposNoRequeridosTest()
        {
            //Arrange
            datosPrepa.IdBanner = "00";
            datosPrepa.Preparatoria = "1001";
            datosPrepa.PromedioPreparatoria = default(string);
            datosPrepa.VPDI="UAN";
            mockProspectRepository.Setup(r => r.UpdateDatosPrepa(It.IsAny<DatosPrepa>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateDatosPrepa(datosPrepa);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateDatosPrepa(It.IsAny<DatosPrepa>()));
        }
        /// <summary>
        /// Validar  requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void DatosPrepaRequeridosSucessTest()
        {
            //Arrange
            datosPrepa.IdBanner = default(string);
            datosPrepa.Preparatoria = default(string);
            datosPrepa.VPDI = default(string);
            mockProspectRepository.Setup(r => r.UpdateDatosPrepa(It.IsAny<DatosPrepa>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateDatosPrepa(datosPrepa);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Identificador de banner es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "Preparatoria es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "VPDI es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);



                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  Logitud Excedida
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void DatosPrepaLongitudExcedidaSucessTest()
        {
            //Arrange
            datosPrepa.IdBanner = "12345798987654321";
            datosPrepa.Preparatoria = "10001213213050313540321";
            datosPrepa.PromedioPreparatoria = "9.321121321353131513513513554654654654654444444444444";
            datosPrepa.VPDI = "dasdasdasdasdasd";
            mockProspectRepository.Setup(r => r.UpdateDatosPrepa(It.IsAny<DatosPrepa>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateDatosPrepa(datosPrepa);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 4;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del id de banner es de 9 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Preparatoria es de 6 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de Promedio Preparatoria es de 24 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "La longitud máxima de VPDI es de 4 caracteres";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
