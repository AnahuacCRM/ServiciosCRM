using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class CambioTipoAdmisionProcessorTest
    {

        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CambiosTipoAdmision cambiotipoadmision;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Cambio Tipo de Admision

            cambiotipoadmision = new CambiosTipoAdmision()
            {
                OportunidadIdCRM = new Guid("5D806EF7-123E-E611-80F0-A45D36FCEECC"),             
                IdBanner = "00169536",
                TipoAlumno = "N",
                TipoAdmision = "AA",
                VPDI = "UAC",
                NumeroSolicitud = 1,
                Periodo= "196810"

            };
            #endregion
        }

        [TestMethod]
        public void CambioTipoAdmisionTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCambiosTipoAdmision(It.IsAny<CambiosTipoAdmision>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCambiosTipoAdmision(cambiotipoadmision);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCambiosTipoAdmision(It.IsAny<CambiosTipoAdmision>()));
        }

        /// <summary>
        /// Validar Longuitud máxima
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambioTipoAdmisionExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            cambiotipoadmision.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdateCambiosTipoAdmision(It.IsAny<CambiosTipoAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiosTipoAdmision(cambiotipoadmision);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud maxima del atributo IdBanner es de 9 caracteres.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar Longuitud máxima de los campos de Cambio Tipo admision
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambioTipoAdmisionExcededSucessTest()
        {
            //Arrange
            cambiotipoadmision.TipoAlumno = "ASD";
            cambiotipoadmision.TipoAdmision = "SDSDD";
            cambiotipoadmision.VPDI = "asdasdasd";
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiosTipoAdmision(cambiotipoadmision);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud maxima del atributo TipoAlumno es de 1 caracteres.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud maxima del atributo TipoAdmision es de 2 caracteres.";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de VPDI es de 4 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambioTipoAdmisionSinRequeridosSucessTest()
        {
            //Arrange
            cambiotipoadmision.IdBanner = default(string);
            cambiotipoadmision.TipoAlumno = default(string);
            cambiotipoadmision.TipoAdmision = default(string);
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiosTipoAdmision(cambiotipoadmision);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El atributo IdBanner es requerido.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "El atributo TipoAlumno es requerido.";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El atributo TipoAdmision es requerido.";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
