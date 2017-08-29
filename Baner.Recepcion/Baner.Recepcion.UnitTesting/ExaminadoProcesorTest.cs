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
    public class ExaminadoProcesorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Examinado exam;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de Examinado
            exam = new Examinado()
            {
                OportunidadIdCRM = new Guid("3638c958-5a3d-e611-80ec-6c3be5a878bc"),
                IdBanner = "00",
                NumeroSolicitud = 12,
                Programa = "ACT",
                PromedioPreparatoria = "9.0",
                Periodo = "201645",
                TipoAlumno = "S",
                VPDI = "UAN",
                Campus="UAN"
            };
            #endregion
        }

        [TestMethod]
        public void ExaminadoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateExaminado(It.IsAny<Examinado>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateExaminado(exam);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateExaminado(It.IsAny<Examinado>()));
        }

        /// <summary>
        /// Validar Longuitud máxima
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExaminadoExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            exam.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdateExaminado(It.IsAny<Examinado>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateExaminado(exam);
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

        [TestMethod]
        public void ExaminadoWithoutNoRerquiredSucessTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateExaminado(It.IsAny<Examinado>())).Returns(true);
            #region Limpiar campos no requeridos
            //exam.IdCRM = default(Guid);
           
            exam.Periodo = default(string);
           

            #endregion

            //Act
            var resultado = prospectProcessor.UpdateExaminado(exam);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateExaminado(It.IsAny<Examinado>()));
        }


        /// <summary>
        /// Validar Longuitud máxima Programa
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExaminadoExcededLongitudeProgramarSucessTest()
        {
            //Arrange
            exam.Campus = "AAA";
            exam.Programa = "123456789098765432123564";
            mockProspectRepository.Setup(r => r.UpdateExaminado(It.IsAny<Examinado>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateExaminado(exam);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del programa es de 12 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExaminadoGuidNullSucessTest()
        {
            //Arrange
            #region Informacion de Examinado
            Examinado exami = new Examinado()
            {
                //OportunidadIdCRM = new Guid(),
                IdBanner = "00",
                NumeroSolicitud = 12,
                Programa = "ACT",
                PromedioPreparatoria = "9.0",
                Periodo = "201645",
                TipoAlumno = "S",
                VPDI = "UAN"
            };
            #endregion
            mockProspectRepository.Setup(r => r.UpdateExaminado(It.IsAny<Examinado>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateExaminado(exami);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 2;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Identificador de Oportunidad es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
} 
        

