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
    public class CambioSGASTDNProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CambioSGASTDN cambio;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de CambioSGASTDN
            cambio = new CambioSGASTDN()
            {
                OportunidadIdCRM = new Guid("5D806EF7-123E-E611-80F0-A45D36FCEECC"),
                IdBanner = "0213",
                Periodo = "111111",
                Programa= "ACT",
                Campus= "UAC",
                Escuela= "UA",
                VPDI="UAN"
            };
            #endregion
        }

        [TestMethod]
        public void CambioSGASTDNTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCambioSGASTDN(It.IsAny<CambioSGASTDN>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCambioSGASTDN(cambio);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCambioSGASTDN(It.IsAny<CambioSGASTDN>()));
        }

        /// <summary>
        /// Validar  requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambioSGASTDNRequeridosSucessTest()
        {
            //Arrange
            cambio.OportunidadIdCRM = null;
            cambio.IdBanner = default(string);
            cambio.Periodo = default(string);
            cambio.Programa = default(string);
            cambio.Campus= default(string);
            cambio.Escuela= default(string);
            cambio.VPDI= default(string);
            mockProspectRepository.Setup(r => r.UpdateCambioSGASTDN(It.IsAny<CambioSGASTDN>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambioSGASTDN(cambio);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 7;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);
               
                var expectedErrorMessage = "El Identificador de Oportunidad es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "El Identificador de banner es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El Periodo es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "El Programa es requerido";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "El Campus es requerido";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "Escuela es requerido";
                var actualErrorMessage6 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);

                var expectedErrorMessage7 = "VPDI es requerido";
                var actualErrorMessage7 = ex.Errors.ElementAt(6);
                Assert.AreEqual<string>(expectedErrorMessage7, actualErrorMessage7);


                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  longitud excedida
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambioSGASTDNLongitudExcedidaSucessTest()
        {
            //Arrange
            cambio.IdBanner = "12345678987665443";
            cambio.Periodo = "AAAAAAAAAAAAAAAAAAAAAAAAA";
            cambio.Programa = "1111111111111111111111";
            cambio.Campus ="UACDFSDFS";
            cambio.Escuela ="UACASDASDASDASDA";
            cambio.VPDI = "asdasdasdasdasdadasd";
            mockProspectRepository.Setup(r => r.UpdateCambioSGASTDN(It.IsAny<CambioSGASTDN>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambioSGASTDN(cambio);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 6;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del id de banner es de 9 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima del Periodo es de 6 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima del Programa es de 12 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "La longitud máxima de Campus es de 3 caracteres";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "La longitud máxima de Escuela es de 2 caracteres";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "La longitud máxima de VPDI es de 4 caracteres";
                var actualErrorMessage6 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);


                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
