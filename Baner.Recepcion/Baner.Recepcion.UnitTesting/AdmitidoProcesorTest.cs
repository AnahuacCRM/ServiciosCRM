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
    public class AdmitidoProcesorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Admitido admitido;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Update Admitido
            admitido = new Admitido()
            {
                OportunidadIdCRM = new Guid("EEC074FC-7036-E611-80EB-6C3BE5A84798"),
                IdBanner = "234567890",
                NumeroSolicitud = 99,
                DesicionAdmision = "AA",
                Programa = "Actuaria",
                Periodo = "196810",
                Campus = "UAS",
                VPDI = "UAN",
                Escuela = "IA",
                TipoAdmision = "AD",
                PuntualizacionSobresaliente = "Y"
            };
            #endregion
        }

        [TestMethod]
        public void AdmitidoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateAdmitido(It.IsAny<Admitido>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateAdmitido(admitido);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateAdmitido(It.IsAny<Admitido>()));
        }

        /// <summary>
        /// Valida solo caracteres validos.
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void AdmitidoPuntuacionSobresalienteCaracteresValidosSucessTest()
        {
            //Arrange
            admitido.PuntualizacionSobresaliente = "Z";
            mockProspectRepository.Setup(r => r.UpdateAdmitido(It.IsAny<Admitido>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateAdmitido(admitido);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El atributo PuntualizacionSobresaliente solo acepta los valores Y ó N.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Valida longitudes de los atributos.
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void AdmitidoValidationLengthSucessTest()
        {
            //Arrange
            admitido.IdBanner = "23456789034";
            admitido.NumeroSolicitud = 994;
            admitido.DesicionAdmision = "AA3434";
            admitido.Programa = "Actuaria34343434";
            admitido.Periodo = "1968103434";
            admitido.Campus = "UAS343434";
            admitido.VPDI = "UAN34343";
            admitido.Escuela = "IA343434";
            admitido.TipoAdmision = "AD343434";
            admitido.PuntualizacionSobresaliente = "Z";

            mockProspectRepository.Setup(r => r.UpdateAdmitido(It.IsAny<Admitido>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateAdmitido(admitido);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 10;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                //var expectedErrorMessage = "El atributo PuntualizacionSobresaliente solo acepta los valores Y ó N.";
                //var actualErrorMessage = ex.Errors.ElementAt(0);
                //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Valida requeridos.
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void AdmitidoValidationRequiredSucessTest()
        {
            //Arrange
            admitido.OportunidadIdCRM = null;
            admitido.IdBanner = "";
            admitido.NumeroSolicitud = default(int);
            admitido.DesicionAdmision = "";
            admitido.Programa = "";
            admitido.Periodo = "";
            admitido.Campus = "";
            admitido.VPDI = "";
            admitido.Escuela = "";
            admitido.TipoAdmision = "";
            admitido.PuntualizacionSobresaliente = "";

            mockProspectRepository.Setup(r => r.UpdateAdmitido(It.IsAny<Admitido>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateAdmitido(admitido);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 11;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                //var expectedErrorMessage = "El atributo PuntualizacionSobresaliente solo acepta los valores Y ó N.";
                //var actualErrorMessage = ex.Errors.ElementAt(0);
                //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
