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
    public class FechaExamenAdmisionTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        FechaExamenAdmision fechaExamen;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Fecha de Examen de Admision

            #region Lista de Examenes
            List<Examenes> lstExam = new List<Examenes>();
            Examenes exam = new Examenes()
            {
                ClaveExamen = "PAA",
                FechaExamen = new CustomDate() { Day = 02, Month = 01, Year = 1980 }
            };
            lstExam.Add(exam);
            Examenes exam2 = new Examenes()
            {
                ClaveExamen = "EOV",
                FechaExamen = new CustomDate() { Day = 03, Month = 02, Year = 1981 }
            };

            #endregion

            fechaExamen = new FechaExamenAdmision()
            {
                OportunidadIdCRM = new Guid("5D806EF7-123E-E611-80F0-A45D36FCEECC"),
                IdBanner = "00294811",
                NumeroSolicitud = 23,
                lstExamenes = lstExam,
                SessionExamen = "1",
                VPDI="UAN",
                Periodo="111111"
            };
            #endregion
        }

        [TestMethod]
        public void FechaExamenTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            var resultado = prospectProcessor.FechaExamenAdmision(fechaExamen);
            //Assert
            mockProspectRepository.Verify(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>()));
        }

        /// <summary>
        /// Validar Longuitud máxima
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void FechaExamenExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            fechaExamen.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.FechaExamenAdmision(fechaExamen);
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
        /// Validar Longuitud máxima de Numero de Solicitud
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void FechaExamnExcededLongitudSucessTest()
        {
            //Arrange
            fechaExamen.NumeroSolicitud = 1244;
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.FechaExamenAdmision(fechaExamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El atributo NumeroSolicitud esta fuera de rango.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar Longuitud máxima de Session de Examen
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void FechaExamenExcededSessionExamenSucessTest()
        {
            //Arrange
            fechaExamen.SessionExamen = "2323";
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.FechaExamenAdmision(fechaExamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud maxima del atributo SessionExamen es de 1 caracter.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Valida que fecha de examen venga nulo.
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void FechaExamenRangeFechaExamenSucessTest()
        {
            //Arrange
            fechaExamen.lstExamenes[0].FechaExamen = new CustomDate()
            {
                Year = 1880,
                Month = 18,
                Day = 42
            };
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.FechaExamenAdmision(fechaExamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El año proporcionado es incorrecto";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "El mes proporcionado es incorrecto";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El día proporcionado es incorrecto";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Valida longitud de clave de examen.
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void FechaExamenLengthClaveExamenSucessTest()
        {
            //Arrange
            fechaExamen.lstExamenes[0].FechaExamen = new CustomDate()
            {
                Year = 1880,
                Month = 18,
                Day = 42
            };
            mockProspectRepository.Setup(r => r.FechaExamenAdmision(It.IsAny<FechaExamenAdmision>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.FechaExamenAdmision(fechaExamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El año proporcionado es incorrecto";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "El mes proporcionado es incorrecto";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El día proporcionado es incorrecto";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
