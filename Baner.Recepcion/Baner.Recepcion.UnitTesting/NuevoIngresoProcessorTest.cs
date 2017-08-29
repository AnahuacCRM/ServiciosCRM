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
    public class NuevoIngresoProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        NuevoIngreso nuevoingreso;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Update Nuevo Ingreso
            nuevoingreso = new NuevoIngreso()
            {
                OportunidadIdCRM = new Guid("EEC074FC-7036-E611-80EB-6C3BE5A84798"),
                IdBanner = "00294811",
                Periodo = "196810",
                FechaSeleccionCursos = new CustomDate() { Year = 2016, Month = 10, Day = 4 },
                VPDI="UAS"
            };
            #endregion
        }

        [TestMethod]
        public void NuevoIngresoProcessor()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateNuevoIngreso(It.IsAny<NuevoIngreso>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateNuevoIngreso(nuevoingreso);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateNuevoIngreso(It.IsAny<NuevoIngreso>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void NuevoIngresoWithoutRequiredSucessTest()
        {
            //Arrange
            nuevoingreso.OportunidadIdCRM = null;
            nuevoingreso.IdBanner = default(string);
            nuevoingreso.Periodo = default(string);
            nuevoingreso.FechaSeleccionCursos = default(CustomDate);

            mockProspectRepository.Setup(r => r.UpdateNuevoIngreso(It.IsAny<NuevoIngreso>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateNuevoIngreso(nuevoingreso);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 4;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);
                
                var expectedErrorMessage = "El Identificador de Oportunidad es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "El atributo IdBanner es requerido.";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El atributo Periodo es requerido.";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "El atributo FechaSeleccionCursos es requerido.";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
