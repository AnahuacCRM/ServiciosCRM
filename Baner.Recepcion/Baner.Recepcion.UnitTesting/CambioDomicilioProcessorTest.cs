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
    public class CambioDomicilioProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CambiaDomicilio cambiodomicilio;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Domicilios

            List<Domicilio> lstDomicilio = new List<Domicilio>();
            Domicilio dom = new Domicilio()
            {
                Calle1 = "sauces",
                Calle2 = "13",
                //Ciudad = "MEX",
                Colonia = "izcalli del valle",
                CP = "56030",
                Estado = "MEX",
                Municipio = "15028",
                Pais = "99",
                SecuenciaDireccion = "1",
                TipoDireccion = "E2",
                TipoOperacion = "I",
                VPDI = "UAN"
            };
            lstDomicilio.Add(dom);
            Domicilio dom2 = new Domicilio()
            {
                Calle1 = "AGUASCALIENTES",
                Calle2 = "175",
                //Ciudad = "CIUDAD",
                Colonia = "",
                CP = "56030",
                Estado = "MEX",
                Municipio = "15028",
                Pais = "99",
                SecuenciaDireccion = "2",
                TipoDireccion = "E2",
                TipoOperacion = "U",
                VPDI = "UAS"
            };
            lstDomicilio.Add(dom2);

            cambiodomicilio = new CambiaDomicilio()
            {
                IdBanner = "00294811",
                lstDomicilio = lstDomicilio
            };
            #endregion
        }

        [TestMethod]
        public void CambiaDomicilioTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCambiaDomicilio(It.IsAny<CambiaDomicilio>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCambiaDomicilio(cambiodomicilio);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCambiaDomicilio(It.IsAny<CambiaDomicilio>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaDomicilioValidationLengthSucessTest()
        {
            //Arrange
            //cambiodomicilio.IdBanner = "002948113434343434343";
            cambiodomicilio.lstDomicilio[0] = new Domicilio()
            {
                Calle1 = "ERTERTERTERTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTRRTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTR",
                Calle2 = "ERTERTERTERTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTRRTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTR",
                //Ciudad = "ERTERTERTERTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTRRTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTR",
                Colonia = "ERTERTERTERTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTRRTERTERTERTERTERTERTETERTERTERTERTERTERTERTRETERTERTRETERTERTERTR",
                CP = "CPCPCPCPCPCCPCPPCPCPCPCPCPCPCPCPCPCPCCPCPCPPCPCPCPCPCPCPCPCPCPCPPCPCPPCPCPCPCP",
                Estado = "RTRYR",
                Municipio = "ERTERERTERT",
                Pais = "SDFSDFSDF",
                SecuenciaDireccion = "ERERER",
                TipoDireccion = "DFGDFG",
                TipoOperacion = "SDFDFGD",
                VPDI = "KJHKJHKJHKJ"
            };

            mockProspectRepository.Setup(r => r.UpdateCambiaDomicilio(It.IsAny<CambiaDomicilio>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaDomicilio(cambiodomicilio);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 12;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaDomicilioValidationRequiredSucessTest()
        {
            //Arrange
            //cambiodomicilio.IdBanner = default(string);
            cambiodomicilio.lstDomicilio[0] = new Domicilio()
            {
                //Ciudad = default(string),
                CP = default(string),
                Estado = default(string),
                SecuenciaDireccion = default(string),
                TipoDireccion = default(string),
                TipoOperacion = default(string),
                VPDI = default(string),
            };

            mockProspectRepository.Setup(r => r.UpdateCambiaDomicilio(It.IsAny<CambiaDomicilio>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaDomicilio(cambiodomicilio);
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

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaDomicilioValidationTipoOperacion()
        {
            //Arrange
            cambiodomicilio.lstDomicilio[0].TipoOperacion = "P";

            mockProspectRepository.Setup(r => r.UpdateCambiaDomicilio(It.IsAny<CambiaDomicilio>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaDomicilio(cambiodomicilio);
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
