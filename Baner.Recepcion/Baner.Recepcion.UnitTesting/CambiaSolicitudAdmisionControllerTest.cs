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
    public class CambiaSolicitudAdmisionControllerTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CambiaSolicitudAdmision cambiasolicitudadmision;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Datos Persona

            cambiasolicitudadmision = new CambiaSolicitudAdmision()
            {
                OportunidadIdCRM = new Guid("5D806EF7-123E-E611-80F0-A45D36FCEECC"),
                IdBanner = "0213",
                Periodo = "111111",
                Programa = "ACT",
                Campus = "UAN",
                Escuela = "UA",
                VPDI="UAN",
                NumeroSolicitud=1

            };
            #endregion
        }

        [TestMethod]
        public void CambiaSolicitudAdmisionTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCambiaSolicitudAdmision(It.IsAny<CambiaSolicitudAdmision>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCambiaSolicitudAdmision(cambiasolicitudadmision);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCambiaSolicitudAdmision(It.IsAny<CambiaSolicitudAdmision>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaSolicitudAdmisionValidationLengthSucessTest()
        {
            //Arrange
            cambiasolicitudadmision.IdBanner = "002948115565656";
            cambiasolicitudadmision.Periodo = "111111343434";
            cambiasolicitudadmision.Programa = "ACTERERERERERERE";
            cambiasolicitudadmision.Campus = "UACDFDFDF";
            cambiasolicitudadmision.Escuela = "UADFDFF";

            mockProspectRepository.Setup(r => r.UpdateCambiaSolicitudAdmision(It.IsAny<CambiaSolicitudAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaSolicitudAdmision(cambiasolicitudadmision);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 5;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaSolicitudAdmisionValidationRequiredSucessTest()
        {
            //Arrange
            cambiasolicitudadmision.IdBanner = default(string);
            cambiasolicitudadmision.Periodo = default(string);
            cambiasolicitudadmision.Programa = default(string);
            cambiasolicitudadmision.Campus = default(string);
            cambiasolicitudadmision.Escuela = default(string);
            cambiasolicitudadmision.VPDI = default(string);
           

            mockProspectRepository.Setup(r => r.UpdateCambiaSolicitudAdmision(It.IsAny<CambiaSolicitudAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaSolicitudAdmision(cambiasolicitudadmision);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 6;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
