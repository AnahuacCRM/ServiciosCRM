using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.BusinessLayer;
using Moq;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;

namespace Baner.Recepcion.UnitTesting
{
    /// <summary>
    /// Summary description for PreUniversitario
    /// </summary>
    [TestClass]
    public class PreUniversitarioTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        PreUniversitario preUniversitario;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Create PreUniversitario
            preUniversitario = new PreUniversitario()
            {
                Nombre = "ELIZABETH",
                SegundoNombre = "",
                ApellidoPaterno = "RUIZ",
                ApellidoMaterno = "NAVARRO",
                TelefonoLada = "56",
                TelefonoNumero = "56565656",
                CorreoElectronico = "elizabeth@rhino.mx",
                Nivel = "LC",
                Codigo = "BIO",
                Descripcion = "Biotecnologia",
                Campus = "UAN",
                Estado = "16",
                Municipio = "12",
                OtroEstado = "4",
                Origen = "1",
                SubOrigen = "SubOrigen",
                VPD = "UAN"
            };
            #endregion
        }

        [TestMethod]
        public void PreUniversitario()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.CreatePreUniversitario(It.IsAny<PreUniversitario>())).Returns(Guid.NewGuid());
            //Act
            var resultado = prospectProcessor.CreatePreUniversitario(preUniversitario);
            //Assert
            mockProspectRepository.Verify(r => r.CreatePreUniversitario(It.IsAny<PreUniversitario>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void PreUniversitarioWithoutRequiredSucessTest()
        {
            //Arrange
            preUniversitario.Nombre = default(string);
            preUniversitario.ApellidoPaterno = default(string);
            preUniversitario.CorreoElectronico = default(string);
            preUniversitario.Nivel = default(string);
            preUniversitario.Campus = default(string);
            preUniversitario.Origen = default(string);
            preUniversitario.SubOrigen = default(string);
            preUniversitario.VPD = default(string);

            mockProspectRepository.Setup(r => r.CreatePreUniversitario(It.IsAny<PreUniversitario>())).Returns(Guid.NewGuid());
            //Act
            try
            {
                var resultado = prospectProcessor.CreatePreUniversitario(preUniversitario);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 8;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El atributo Nombre es requerido.";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "El atributo ApellidoPaterno es requerido.";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "El atributo CorreoElectronico es requerido.";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "El atributo Nivel es requerido.";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "El atributo Campus es requerido.";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "El atributo Origen es requerido.";
                var actualErrorMessage6 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);

                var expectedErrorMessage7 = "El atributo SubOrigen es requerido.";
                var actualErrorMessage7 = ex.Errors.ElementAt(6);
                Assert.AreEqual<string>(expectedErrorMessage7, actualErrorMessage7);

                var expectedErrorMessage8 = "El atributo VPD es requerido.";
                var actualErrorMessage8 = ex.Errors.ElementAt(7);
                Assert.AreEqual<string>(expectedErrorMessage8, actualErrorMessage8);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
