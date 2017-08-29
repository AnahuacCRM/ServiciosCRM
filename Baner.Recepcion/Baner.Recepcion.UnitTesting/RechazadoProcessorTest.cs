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
    public class RechazadoProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Rechazado rechazado;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de Rechazado
            rechazado = new Rechazado()
            {      
                OportunidadIdCRM= Guid.Empty,
                IdBanner = "00",
                NumeroSolicitud = 1,
                DecisionAdmision = "RE",
                VPDI="UAS",
                Periodo="111111"
            };
            #endregion
        }

        [TestMethod]
        public void RechazadoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateRechazado(It.IsAny<Rechazado>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateRechazado(rechazado);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateRechazado(It.IsAny<Rechazado>()));
        }

        /// <summary>
        /// Validar IdBanner requerido
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void RechazadoExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            rechazado.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdateRechazado(It.IsAny<Rechazado>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateRechazado(rechazado);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del id de banner es de 9 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar IdBanner requerido
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void RechazadoRequeridoIdBannerSucessTest()
        {
            //Arrange
            rechazado.IdBanner = "";
            mockProspectRepository.Setup(r => r.UpdateRechazado(It.IsAny<Rechazado>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateRechazado(rechazado);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Identificador de banner es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar Longuitud máxima DecisionAdmision
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void RechazadoExcededLongitudeDecisionAdmisionSucessTest()
        {
            //Arrange
            rechazado.OportunidadIdCRM = It.IsAny<Guid>();
            rechazado.DecisionAdmision = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdateRechazado(It.IsAny<Rechazado>())).Returns(true);

            //Act
            try
            {
                var resultado = prospectProcessor.UpdateRechazado(rechazado);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima de Decision de Admisión es de 2 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }
    }
}
