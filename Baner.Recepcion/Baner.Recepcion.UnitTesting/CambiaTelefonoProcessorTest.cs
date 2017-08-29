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
    public class CambiaTelefonoProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CambiaTelefono cambiaTelefono;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de CambiaTelefono


            cambiaTelefono = new CambiaTelefono();

            cambiaTelefono.IdBanner = "00294811";
            var lst_CambiaTelefonos = new ListaCambiaTelefonos();
            lst_CambiaTelefonos.VPDI = "UAC";
            lst_CambiaTelefonos.TipoTelefono = "C";
            lst_CambiaTelefonos.SecuenciaTelefono = 1;
            lst_CambiaTelefonos.TipoOperacion = "I";
            lst_CambiaTelefonos.TelefonoArea = "099";
            lst_CambiaTelefonos.Telefono = "5512122365";
            lst_CambiaTelefonos.TelefonoPreferido = "Y";

            cambiaTelefono.lstInformacionTelefonos = new List<ListaCambiaTelefonos>();
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos);
                 
           
            #endregion
        }

        [TestMethod]
        public void CambiaTelefonoTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.CambiaTelefono(It.IsAny<CambiaTelefono>())).Returns(true);
            //Act
            var resultado = prospectProcessor.CambiaTelefono(cambiaTelefono);
            //Assert
            mockProspectRepository.Verify(r => r.CambiaTelefono(It.IsAny<CambiaTelefono>()));
        }

        /// <summary>
        /// Validar Informacion ListaCambiaTelefonos null
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaTelefonoSinInformacionSucessTest()
        {
            //Arrange
            #region Informacion CambiaTelefono

            cambiaTelefono = new CambiaTelefono();

            cambiaTelefono.IdBanner = "00";

            cambiaTelefono.lstInformacionTelefonos = null;
            #endregion
            mockProspectRepository.Setup(r => r.CambiaTelefono(It.IsAny<CambiaTelefono>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.CambiaTelefono(cambiaTelefono);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Lista de Información de telefonos es requerida";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar Requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaTelefonoRqueridosSucessTest()
        {
            //Arrange
            #region Informacion de CambiaTelefono


            cambiaTelefono = new CambiaTelefono();

            cambiaTelefono.IdBanner = "00294811";
            var lst_CambiaTelefonos = new ListaCambiaTelefonos();
            lst_CambiaTelefonos.VPDI = default(string);
            lst_CambiaTelefonos.TipoTelefono = default(string);
            lst_CambiaTelefonos.SecuenciaTelefono = null;
            lst_CambiaTelefonos.TipoOperacion = default(string);
            lst_CambiaTelefonos.TelefonoArea = default(string);
            lst_CambiaTelefonos.Telefono = default(string);
            lst_CambiaTelefonos.TelefonoPreferido = default(string);

            cambiaTelefono.lstInformacionTelefonos = new List<ListaCambiaTelefonos>();
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos);


            #endregion
            mockProspectRepository.Setup(r => r.CambiaTelefono(It.IsAny<CambiaTelefono>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.CambiaTelefono(cambiaTelefono);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 5;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "VPDI es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "Tipo Telefono es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "Secuencia Telefono es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "Tipo Operacion  es requerido";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "Telefono es requerido";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

               



                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar LongitudExtendida
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaTelefonoLongitudExtendidaSucessTest()
        {
            //Arrange
            #region Informacion de CambiaTelefono


            cambiaTelefono = new CambiaTelefono();

            cambiaTelefono.IdBanner = "00294811";
            var lst_CambiaTelefonos = new ListaCambiaTelefonos();
            lst_CambiaTelefonos.VPDI ="1231321321321332132";
            lst_CambiaTelefonos.TipoTelefono = "1231321321321332132";
            lst_CambiaTelefonos.SecuenciaTelefono = 1234567;
            lst_CambiaTelefonos.TipoOperacion = "1231321321321332132";
            lst_CambiaTelefonos.TelefonoArea = "1231321321321332132";
            lst_CambiaTelefonos.Telefono = "1231321321321332132";
            lst_CambiaTelefonos.TelefonoPreferido = "1231321321321332132";

            cambiaTelefono.lstInformacionTelefonos = new List<ListaCambiaTelefonos>();
            cambiaTelefono.lstInformacionTelefonos.Add(lst_CambiaTelefonos);


            #endregion
            mockProspectRepository.Setup(r => r.CambiaTelefono(It.IsAny<CambiaTelefono>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.CambiaTelefono(cambiaTelefono);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 7;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima de VPDI es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Tipo Telefono es de 4 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "Secuencia Telefono máxima  es  de 999999";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "La longitud máxima de Tipo Operacion  es de 1 caracter";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "La longitud máxima de TelefonoArea es de 6 caracteres";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "La longitud máxima de Telefono es de 12 caracteres";
                var actualErrorMessage6 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);

                var expectedErrorMessage7 = "La longitud máxima de Telefono Preferido es de 1 caracter";
                var actualErrorMessage7 = ex.Errors.ElementAt(6);
                Assert.AreEqual<string>(expectedErrorMessage7, actualErrorMessage7);



                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }


    }
}
