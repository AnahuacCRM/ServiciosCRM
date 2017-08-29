using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.DataInterfaces;
using Moq;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class InscritoProcesorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Inscrito insc;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de Inscrito
            insc = new Inscrito()
            {
                OportunidadIdCRM = new Guid("EEC074FC-7036-E611-80EB-6C3BE5A84798"),
                IdBanner = "00",
                Periodo = "201645",
                FechaPagoInscripcion = new CustomDate() { Year = 2015, Month = 5, Day = 30 },
                VPDI = "UAS"
                
            };
            #endregion
        }

        [TestMethod]
        public void InscritoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateInscrito(It.IsAny<Inscrito>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateInscrito(insc);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateInscrito(It.IsAny<Inscrito>()));
        }

        /// <summary>
        /// Validar Longuitud máxima IdBanner
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            insc.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdateInscrito(It.IsAny<Inscrito>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateInscrito(insc);
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
        /// Validar Longuitud máxima Periodo
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExcededLongitudePeriodoSucessTest()
        {
            //Arrange
            insc.Periodo = "123456789098765432";
            mockProspectRepository.Setup(r => r.UpdateInscrito(It.IsAny<Inscrito>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateInscrito(insc);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del Periodo es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void InscritoRequeridosSucessTest()
        {
            //Arrange
            insc.OportunidadIdCRM = null;
            insc.IdBanner = default(string);
            insc.Periodo = default(string);
            insc.FechaPagoInscripcion = default(CustomDate);

            mockProspectRepository.Setup(r => r.UpdateInscrito(It.IsAny<Inscrito>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateInscrito(insc);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
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

               


                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  LongitudExtendida
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void InscritoLongitudExtendidaSucessTest()
        {
            //Arrange
            insc.IdBanner ="12345678912345798";
            insc.Periodo = "12345678912345798";
            //insc.FechaPagoInscripcion = new CustomDate()
            //{
            //    Day = 5,
            //    Month = 66,//Mes incorrecto
            //    Year = 2016
            //};


            mockProspectRepository.Setup(r => r.UpdateInscrito(It.IsAny<Inscrito>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateInscrito(insc);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 2;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del id de banner es de 9 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima del Periodo es de 6 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                //var expectedErrorMessage3 = "El mes proporcionado es incorrecto";
                //var actualErrorMessage3 = ex.Errors.ElementAt(2);
                //Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);



                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  Fecha incorrecta
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void InscritoFechaIncorrectaSucessTest()
        {
            //Arrange

            insc.FechaPagoInscripcion = new CustomDate()
            {
                Day = 55,
                Month = 66,//Mes incorrecto
                Year = 201612
            };


            mockProspectRepository.Setup(r => r.UpdateInscrito(It.IsAny<Inscrito>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateInscrito(insc);
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
