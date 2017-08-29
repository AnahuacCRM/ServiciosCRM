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
    public class DatosPersonaProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        DatosPersona datospersona;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de Datos Persona

            datospersona = new DatosPersona()
            {
                IdBanner = "00294838",
                Apellidos = "ORTIZ*GRANADOS",
                Nombre = "ERIK",
                SegundoNombre = "SANTIAGO",
                FechaNacimiento = new CustomDate() { Year = 1980, Month = 10, Day = 23 },
                Nacionalidad = "AU",
                EstadoCivil = "L",
                Religion = "AG",
                Sexo = "F"
            };
            #endregion
        }

        [TestMethod]
        public void DatosPersonaTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateDatosPersona(It.IsAny<DatosPersona>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateDatosPersona(datospersona);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateDatosPersona(It.IsAny<DatosPersona>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void DatosPersonaValidationLengthSucessTest()
        {
            //Arrange
            datospersona.IdBanner = "002948113434343434343";
            datospersona.Apellidos = "VEGAS*BETANfgdfgdfgdfgfdgdfgdfgdfgdfgdfgdfgdfgdfgdgdfgdfgfdgdfgdfg";
            datospersona.Nombre = "VEGAS*BETANfgdfgdfgdfgfdgdfgdfgdfgdfgdfgdfgdfgdfgdgdfgdfgfdgdfgdfg";
            datospersona.SegundoNombre = "VEGAS*BETANfgdfgdfgdfgfdgdfgdfgdfgdfgdfgdfgdfgdfgdgdfgdfgfdgdfgdfg";
            //datospersona.FechaNacimiento = new CustomDate() { Year = 2300, Month = 20, Day = 53 };
            datospersona.Nacionalidad = "ALERER";
            datospersona.EstadoCivil = "SERERE";
            datospersona.Religion = "CAERERE";
            datospersona.Sexo = "MERERER";

            mockProspectRepository.Setup(r => r.UpdateDatosPersona(It.IsAny<DatosPersona>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateDatosPersona(datospersona);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 8;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void DatosPersonaValidationRequiredSucessTest()
        {
            //Arrange
            datospersona.IdBanner = default(string);
            datospersona.Apellidos = default(string);
            datospersona.Nombre = default(string);
            //datospersona.FechaNacimiento = default(CustomDate);
            datospersona.Sexo = default(string);

            mockProspectRepository.Setup(r => r.UpdateDatosPersona(It.IsAny<DatosPersona>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateDatosPersona(datospersona);
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

    }
}
