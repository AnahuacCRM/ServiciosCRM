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
    public class SolicitaBecaProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        SolicitaBeca solicitaBeca;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);


            #region Informacion SolicitaBeca
         
            solicitaBeca = new SolicitaBeca();
            solicitaBeca.IdBanner = "00";
            #region InformacionBeca
            var infbeca = new InformacionBeca();


            infbeca.TipoBeca = "A";
            infbeca.DescripcionBeca = "apoyo 50%";
            infbeca.CampusVPDI = "UAC";
            infbeca.Periodo = "111111";        
          
            infbeca.FechaSolicitudBeca = new CustomDate()
            {
                Day = 5,
                Month = 6,
                Year = 2016

            };
            #endregion
            solicitaBeca.SolicitudBecas= new List<InformacionBeca>();
            solicitaBeca.SolicitudBecas.Add(infbeca);
            #endregion
        }

        [TestMethod]
        public void SolicitaBecaTest()
        {

            //Arrange
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
            //Assert
            mockProspectRepository.Verify(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>()));
        }

        /// <summary>
        /// Validar InformacionBeca nulla
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void SolicitaBecaSinInformacionSucessTest()
        {
            //Arrange
            #region Informacion SolicitaBeca

            solicitaBeca = new SolicitaBeca();
            solicitaBeca.IdBanner = "00";

            solicitaBeca.SolicitudBecas = null; 
            #endregion
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Debe tener registrada información de solicitud de beca";
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
        public void SolicitaBecaExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            solicitaBeca.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
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
        public void SolicitaBecaRequeridoIdBannerSucessTest()
        {
            //Arrange
            solicitaBeca.IdBanner = "";
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Identificador de banner es requerido" ;
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }


        /// <summary>
        /// Validar Tipo beca requerido
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void SolicitaBecaRequeridoTipoBecaSucessTest()
        {
            //Arrange
            #region Informacion SolicitaBeca

            solicitaBeca = new SolicitaBeca();
            solicitaBeca.IdBanner = "00";
            #region InformacionBeca
            var infbeca = new InformacionBeca();


            infbeca.TipoBeca = "";
            infbeca.DescripcionBeca = "apoyo 50%";
            infbeca.CampusVPDI = "UAC";
            infbeca.Periodo = "111111";

            infbeca.FechaSolicitudBeca = new CustomDate()
            {
                Day = 5,
                Month = 6,
                Year = 2016

            };
            #endregion
            solicitaBeca.SolicitudBecas = new List<InformacionBeca>();
            solicitaBeca.SolicitudBecas.Add(infbeca);
            #endregion
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Tipo de beca es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar Fecha no requerida
        /// </summary>
        [TestMethod]
        public void SolicitaBecaNoRequeridoFechaSucessTest()
        {
            //Arrange
            #region Informacion SolicitaBeca

            solicitaBeca = new SolicitaBeca();
            solicitaBeca.IdBanner = "00";

            #region InformacionBeca
            var infbeca = new InformacionBeca();


            infbeca.TipoBeca = "A";
            infbeca.DescripcionBeca = "Apoyo 50%";
            infbeca.CampusVPDI = "UAC";
            infbeca.Periodo = "111111";

            //infbeca.FechaSolicitudBeca = new CustomDate()
            //{
            //    Day = 5,
            //    Month = 6,
            //    Year = 2016

            //};
            #endregion
            solicitaBeca.SolicitudBecas = new List<InformacionBeca>();
            solicitaBeca.SolicitudBecas.Add(infbeca);
            #endregion

            //Arrange
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
            //Assert
            mockProspectRepository.Verify(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>()));
        }

        /// <summary>
        /// Validar Fecha con datos incorrectos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void SolicitaBecaFechaIncorrectaSucessTest()
        {
            //Arrange
            #region Informacion SolicitaBeca

            solicitaBeca = new SolicitaBeca();
            solicitaBeca.IdBanner = "00";
            #region InformacionBeca
            var infbeca = new InformacionBeca();


            infbeca.TipoBeca = "A";
            infbeca.DescripcionBeca = "Apoyo 50%";
            infbeca.CampusVPDI = "UAC";
            infbeca.Periodo = "111111";

            infbeca.FechaSolicitudBeca = new CustomDate()
            {
                Day = 5,
                Month = 66, //mes incorrecto
                Year = 2016

            };
            #endregion
            solicitaBeca.SolicitudBecas = new List<InformacionBeca>();
            solicitaBeca.SolicitudBecas.Add(infbeca);
            #endregion
            mockProspectRepository.Setup(r => r.CreateSolicitaBeca(It.IsAny<SolicitaBeca>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreateSolicitaBeca(solicitaBeca);
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
