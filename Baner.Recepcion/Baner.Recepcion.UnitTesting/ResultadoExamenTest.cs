using System;
using System.Text;
using System.Collections.Generic;
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
    /// <summary>
    /// Summary description for ResultadoExamenTest
    /// </summary>
    [TestClass]
    public class ResultadoExamenTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion
        ProspectProcessor prospectProcessor;
        ResultadoExamen resultadoexamen;
        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            //Arrange
            resultadoexamen = new ResultadoExamen();
            resultadoexamen.IdBanner = "00";
            resultadoexamen.VPDI = "UAN";
            var r = new InformacionResultado();
            r.BanderaScore = "Y";
            r.CodigoExamen = "XX";
           
            r.FechaResultado = new CustomDate()
            {
                Year = 2016,
                Month = 06,
                Day = 1
            };
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);
        }


        [TestMethod]
        public void RecibirResultadoExamenSuccessTest()
        {
            mockProspectRepository.Setup(t =>
                t.UpdateResultadoExamen(It.IsAny<ResultadoExamen>())).Returns(true);
            //Arrange
            var expected = true;
            //Act
            var actual = prospectProcessor.UpdateResultadoExamen(resultadoexamen);

            //Assert
            Assert.AreEqual(expected, actual);
            mockProspectRepository.Verify(t => t.UpdateResultadoExamen(It.IsAny<ResultadoExamen>()));
        }


        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void RecibirResultadoExamenInvalidTest()
        {
            //Arrange
            mockProspectRepository.Setup(t =>
                t.UpdateResultadoExamen(It.IsAny<ResultadoExamen>())).Returns(true);
            try
            {
                //ACT
                var resultado = prospectProcessor.UpdateResultadoExamen(new ResultadoExamen());
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);
                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }



        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExcededLongitudeIdBannerResultadoExamenTest()
        {
            //Arrange
            resultadoexamen.IdBanner = "123456789098765432";

            try
            {//Act
                var resultado = prospectProcessor.UpdateResultadoExamen(resultadoexamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del atributo IdBanner es de 9 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExcededLongitudeCodigoExamenResultadoExamenTest()
        {
            //Arrange

            var r = new InformacionResultado();
            r.BanderaScore = "Y";
            r.CodigoExamen = "123456789098765432";
            r.FechaResultado = new CustomDate()
            {
                Year = 2016,
                Month = 06,
                Day = 1
            };
           
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);


            try
            {//Act
                var resultado = prospectProcessor.UpdateResultadoExamen(resultadoexamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del atributo CodigoExamen es de 4 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExcededLongitudeBanderaScoreResultadoExamenTest()
        {
            //Arrange
            var r = new InformacionResultado();
            r.BanderaScore = "123456789098765432";
            r.CodigoExamen = "XX";
            
            r.FechaResultado = new CustomDate()
            {
                Year = 2016,
                Month = 06,
                Day = 1
            };
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);

            try
            {//Act
                var resultado = prospectProcessor.UpdateResultadoExamen(resultadoexamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima del atributo BanderaScore es de 1 caracter";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void InvalidFechaResultadoResultadoExamenTest()
        {
            //Arrange
            var r = new InformacionResultado();
            r.BanderaScore = "Y";
            r.CodigoExamen = "XX";
            r.FechaResultado = new CustomDate()
            {
                Year = 0,
                Month = 0,
                Day = 0
            };
           
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);


            try
            {//Act
                var resultado = prospectProcessor.UpdateResultadoExamen(resultadoexamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }


        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void EmptyFechaResultadoResultadoExamenTest()
        {
            //Arrange
            var r = new InformacionResultado();
            r.BanderaScore = "Y";
            r.CodigoExamen = "XX";
          
            r.FechaResultado = new CustomDate();
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);


            try
            {//Act
                var resultado = prospectProcessor.UpdateResultadoExamen(resultadoexamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 3;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void NullFechaResultadoResultadoExamenTest()
        {
            //Arrange
            var r = new InformacionResultado();
            r.BanderaScore = "Y";
            r.CodigoExamen = "XX";
            r.FechaResultado = null;
           
            resultadoexamen.ResultadosdeExamen = new List<InformacionResultado>();
            resultadoexamen.ResultadosdeExamen.Add(r);


            try
            {//Act
                var resultado = prospectProcessor.UpdateResultadoExamen(resultadoexamen);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }


    }
}
