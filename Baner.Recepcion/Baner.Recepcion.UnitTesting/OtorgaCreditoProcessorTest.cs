using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using System.Collections.Generic;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class OtorgaCreditoProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        OtorgaCredito otorgaCredito;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);


            #region Informacion OtorgaCredito

            otorgaCredito = new OtorgaCredito();
            otorgaCredito.IdBanner = "00";
            #region InformacionBeca
            var infoCredito = new InformacionOtorgaCredito();


           
            infoCredito.DescripcionCredito = "credito academico";
            infoCredito.CampusVPDI = "UAC";
            infoCredito.Periodo = "111111";

            infoCredito.FechaOtorgaCredito = new CustomDate()
            {
                Day = 5,
                Month = 6,
                Year = 2016

            };
            #endregion
            otorgaCredito.InfoCreditos = new List<InformacionOtorgaCredito>();
            otorgaCredito.InfoCreditos.Add(infoCredito);
            #endregion
        }

        [TestMethod]
        public void OtorgaCreditoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
            //Assert
            mockProspectRepository.Verify(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>()));
        }

        /// <summary>
        /// Validar Informacion Otorga credito null
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void OtorgaCreditoSinInformacionSucessTest()
        {
            //Arrange
            #region Informacion SolicitaBeca

            otorgaCredito = new OtorgaCredito();
            otorgaCredito.IdBanner = "00";

            otorgaCredito.InfoCreditos = null;
            #endregion
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Debe tener registrada información de otorgamiento de crédito";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar IdBanner Longuitud máxima
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void OtorgaCreditoExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            otorgaCredito.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
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
        public void OtorgaCreditoRequeridoIdBannerSucessTest()
        {
            //Arrange
            otorgaCredito.IdBanner = "";
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
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
        ///  Validar CampusVPDI requerido
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void OtorgaCreditoRequeridoCampusVPDISucessTest()
        {
            //Arrange
            #region Informacion OtorgaCredito

            otorgaCredito = new OtorgaCredito();
            otorgaCredito.IdBanner = "00";
            #region InformacionBeca
            var infoCredito = new InformacionOtorgaCredito();


           
            infoCredito.DescripcionCredito = "credito academico";
            infoCredito.CampusVPDI = default(string);
            infoCredito.Periodo = "111111";

            infoCredito.FechaOtorgaCredito = new CustomDate()
            {
                Day = 5,
                Month = 6,
                Year = 2016

            };
            #endregion
            otorgaCredito.InfoCreditos = new List<InformacionOtorgaCredito>();
            otorgaCredito.InfoCreditos.Add(infoCredito);
            #endregion
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Campus VPDI es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        ///  Validar Fecha no requerida
        /// </summary>
        [TestMethod]
        public void OtorgaCreditoFechaNorequeridaTest()
        {
            //Arrange
            #region Informacion OtorgaCredito

            otorgaCredito = new OtorgaCredito();
            otorgaCredito.IdBanner = "00";
            #region InformacionBeca
            var infoCredito = new InformacionOtorgaCredito();


           
            infoCredito.DescripcionCredito = "credito academico";
            infoCredito.CampusVPDI = "UAC";
            infoCredito.Periodo = "111111";

            //infoCredito.FechaOtorgaCredito = new CustomDate()
            //{
            //    Day = 5,
            //    Month = 6,
            //    Year = 2016

            //};
            #endregion
            otorgaCredito.InfoCreditos = new List<InformacionOtorgaCredito>();
            otorgaCredito.InfoCreditos.Add(infoCredito);
            #endregion
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
            //Assert
            mockProspectRepository.Verify(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>()));
        }

        /// <summary>
        /// Validar Fecha con datos incorrectos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void OtorgaCreditoFechaIncorrectaSucessTest()
        {
            //Arrange
            #region Informacion OtorgaCredito

            otorgaCredito = new OtorgaCredito();
            otorgaCredito.IdBanner = "00";
            #region InformacionBeca
            var infoCredito = new InformacionOtorgaCredito();


          
            infoCredito.DescripcionCredito = "credito academico";
            infoCredito.CampusVPDI = "UAC";
            infoCredito.Periodo = "111111";

            infoCredito.FechaOtorgaCredito = new CustomDate()
            {
                Day = 5,
                Month = 66,//mes incorrecto
                Year = 2016

            };
            #endregion
            otorgaCredito.InfoCreditos = new List<InformacionOtorgaCredito>();
            otorgaCredito.InfoCreditos.Add(infoCredito);
            #endregion
            mockProspectRepository.Setup(r => r.CreateOtorgaCredito(It.IsAny<OtorgaCredito>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateOtorgaCredito(otorgaCredito);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El mes proporcionado es incorrecto";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }
    }
}
