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
    public class PropedeuticoProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Propedeutico propedeutico;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de Propedeutico
            propedeutico = new Propedeutico()
            {
                //IdCRM = new Guid(),
                IdBanner = "00",
                NumeroSolicitud = 12,
                PeriodoPL = "201645",
                SolicitudAdmisionPL = "Y",
                DecisionAdmision="AA",
                VPDI = "UAC",
                VPDIPL="UAS",
                CampusAdmisionPL = "UAC",
                ProgramaPL= "ACT",
                Periodo="111111"


            };
            #endregion
        }

        [TestMethod]
        public void PropedeuticoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdatePropedeutico(It.IsAny<Propedeutico>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdatePropedeutico(propedeutico);
            //Assert
            mockProspectRepository.Verify(r => r.UpdatePropedeutico(It.IsAny<Propedeutico>()));
        }

        /// <summary>
        /// Validar Longuitud máxima
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void PropedeuticoLongitudeIdBannerSucessTest()
        {
            //Arrange
            propedeutico.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdatePropedeutico(It.IsAny<Propedeutico>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdatePropedeutico(propedeutico);
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

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void PropedeuticoExcededLongitudePeriodoPLSucessTest()
        {
            //Arrange
            propedeutico.PeriodoPL = "123456789098765432123564";
            mockProspectRepository.Setup(r => r.UpdatePropedeutico(It.IsAny<Propedeutico>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdatePropedeutico(propedeutico);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del Periodo PL es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void PropedeuticoExcededLongitudeDecisionAdmisionSucessTest()
        {
            //Arrange
            propedeutico.DecisionAdmision = "123456";
            mockProspectRepository.Setup(r => r.UpdatePropedeutico(It.IsAny<Propedeutico>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdatePropedeutico(propedeutico);
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
