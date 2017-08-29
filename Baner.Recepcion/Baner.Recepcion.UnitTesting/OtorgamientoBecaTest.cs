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
    public class OtorgamientoBecaTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        OtorgamientoBeca otorgamientoBeca;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Otorgamiento de Beca


            otorgamientoBeca = new OtorgamientoBeca()
            {
                IdBanner = "00294811",
                lstBeca = new List<BecaOtorga>()
                 {
                     new BecaOtorga()
                     {
                         Beca = new BecaClass() { TipoBeca = "A", DescripcionBeca="DescripcionBeca",Periodo="196810",CampusVDI="UAN" },
                         FechaOtorgaBeca = new CustomDate(){ Year = 1980, Month =1, Day=2 },
                         FechaVencimientoBeca = new CustomDate(){ Year = 1980, Month =3, Day=2 }
                     },
                     new BecaOtorga()
                     {
                         Beca = new BecaClass() { TipoBeca = "A", DescripcionBeca="DescripcionBeca2",Periodo="196810",CampusVDI="UAS" },
                         FechaOtorgaBeca = new CustomDate(){ Year = 2016, Month =6, Day=6 },
                         FechaVencimientoBeca = new CustomDate(){ Year = 2016, Month =8, Day=6 }
                     }
                 }
            };
            #endregion
        }

        [TestMethod]
        public void OtorgaBecaTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.OtorgamientoaBeca(It.IsAny<OtorgamientoBeca>())).Returns(true);
            //Act
            var resultado = prospectProcessor.OtorgamientoaBeca(otorgamientoBeca);
            //Assert
            mockProspectRepository.Verify(r => r.OtorgamientoaBeca(It.IsAny<OtorgamientoBeca>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void OtorgaBecaExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            otorgamientoBeca.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.OtorgamientoaBeca(It.IsAny<OtorgamientoBeca>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.OtorgamientoaBeca(otorgamientoBeca);
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

        //[TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        //public void OtorgaBecaValidationRequiredSucessTest()
        //{
        //    //Arrange
        //    //otorgamientoBeca.lstBeca = default(List<BecaOtorga>);
        //    otorgamientoBeca.lstBeca = new List<BecaOtorga>()
        //         {
        //             new BecaOtorga()
        //             {
        //                 Beca = default(BecaClass),
        //                 FechaOtorgaBeca = default(CustomDate),
        //                 FechaVencimientoBeca = default(CustomDate)
        //             },
        //             new BecaOtorga()
        //             {
        //                 Beca = default(BecaClass),
        //                 FechaOtorgaBeca = default(CustomDate),
        //                 FechaVencimientoBeca = default(CustomDate)
        //             }
        //         };

        //    mockProspectRepository.Setup(r => r.OtorgamientoaBeca(It.IsAny<OtorgamientoBeca>())).Returns(true);
        //    //Act
        //    try
        //    {
        //        var resultado = prospectProcessor.OtorgamientoaBeca(otorgamientoBeca);
        //    }
        //    catch (BusinessLayerValidationException ex)
        //    {
        //        //Assert
        //        var expectedErrorCount = 10;
        //        var actualErrorCount = ex.Errors.Count();
        //        Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

        //        //var expectedErrorMessage = "El atributo PuntualizacionSobresaliente solo acepta los valores Y ó N.";
        //        //var actualErrorMessage = ex.Errors.ElementAt(0);
        //        //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

        //        mockLogger.Verify(l => l.Error(It.IsAny<string>()));
        //        throw;
        //    }
        //}

    }
}
