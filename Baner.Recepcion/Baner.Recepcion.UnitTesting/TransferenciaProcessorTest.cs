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
    public class TransferenciaProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Transferencia transferencia;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de ExaminadoPI
            transferencia = new Transferencia()
            {
                OportunidadIdCRM= Guid.Empty,
                IdBanner = "00",
                CampusOrigen ="UAC",
                CampusDestino = "UAN"
            };
            #endregion
        }

        [TestMethod]
        public void TransferenciaTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateTransferencia(It.IsAny<Transferencia>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateTransferencia(transferencia);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateTransferencia(It.IsAny<Transferencia>()));
        }

        /// <summary>
        /// Validar  requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void TransferenciaRequeridosSucessTest()
        {
            //Arrange
            transferencia.IdBanner = default(string);
            transferencia.CampusOrigen= default(string);
            transferencia.CampusDestino= default(string);
            mockProspectRepository.Setup(r => r.UpdateTransferencia(It.IsAny<Transferencia>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateTransferencia(transferencia);
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

                var expectedErrorMessage2 = "El Campus Origen es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El Campus Destino es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar longitud excedida
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void TransferenciaLonguitudExcedidaSucessTest()
        {
            //Arrange
            transferencia.IdBanner = "1234567989456";
            transferencia.CampusOrigen = "AAAAAAAAAAAAAAAAAAAAAA";
            transferencia.CampusDestino = "BBBBBBBBBBBBBBBBBBB";
            mockProspectRepository.Setup(r => r.UpdateTransferencia(It.IsAny<Transferencia>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateTransferencia(transferencia);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del id de banner es de 9 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Campus Origen es de 3 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de Campus Destino es de 3 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }
    }
}
