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
    public class EstatusAlumnoProcessorTest
    {

        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        EstatusAlumno estatusalumno;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de cambio de Estatus del alumno

            estatusalumno = new EstatusAlumno()
            {
                OportunidadIdCRM = new Guid("5D806EF7-123E-E611-80F0-A45D36FCEECC"),
                IdBanner = "0213",
                Estatus = "BI",
                Campus = "UAN",
                Periodo="111111"
            
            };
            #endregion
        }

        [TestMethod]
        public void EstatusAlumnoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateEstatusAlumno(It.IsAny<EstatusAlumno>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateEstatusAlumno(estatusalumno);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateEstatusAlumno(It.IsAny<EstatusAlumno>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void EstatusAlumnoValidationLengthSucessTest()
        {
            //Arrange
            estatusalumno.IdBanner = "002948113434343434343002948113434343434343002948113434343434343002948113434343434343002948113434343434343002948113434343434343";
            estatusalumno.Estatus = "122002948113434343434343002948113434343434343002948113434343434343002948113434343434343";

            mockProspectRepository.Setup(r => r.UpdateEstatusAlumno(It.IsAny<EstatusAlumno>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateEstatusAlumno(estatusalumno);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 2;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void EstatusAlumnoValidationRequiredSucessTest()
        {
            //Arrange
            estatusalumno.IdBanner = default(string);
            estatusalumno.Estatus = default(string);

            mockProspectRepository.Setup(r => r.UpdateEstatusAlumno(It.IsAny<EstatusAlumno>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateEstatusAlumno(estatusalumno);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 2;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
