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
    public class CambiaEmailProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        CambiaEmail cambiaEmail;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);

            #region Informacion de CambiaEmail


            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";
            var infoCambiaEmail = new infoCambiaEmails();
            infoCambiaEmail.VPDI = "UAC";
            infoCambiaEmail.TipoCorreoElectronico = "CE";
            infoCambiaEmail.SecuenciaCorreo = "AIIUDIJEDKMD/SDS";
            infoCambiaEmail.TipoOperacion = "I";
            infoCambiaEmail.CorreoElectronico = "loco15@hotmail.com";           
            infoCambiaEmail.CorreoElectronicoIndPreferido = "Y";

            cambiaEmail.lstinfoCambiaEmails = new List<infoCambiaEmails>();
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail);


            #endregion
        }

        [TestMethod]
        public void CambiaEmailTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.UpdateCambiaEmail(It.IsAny<CambiaEmail>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateCambiaEmail(cambiaEmail);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateCambiaEmail(It.IsAny<CambiaEmail>()));
        }

        /// <summary>
        /// Validar Informacion infoCambiaEmails null
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void CambiaEmailSinInformacionSucessTest()
        {
            //Arrange
            #region Informacion CambiaEmail

            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";

            cambiaEmail.lstinfoCambiaEmails = null;
            #endregion
            mockProspectRepository.Setup(r => r.UpdateCambiaEmail(It.IsAny<CambiaEmail>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaEmail(cambiaEmail);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Lista de Información de EMail es requerida";
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
        public void CambiaEmailRqueridosSucessTest()
        {
            //Arrange
            #region Informacion de CambiaEmail


            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";
            var infoCambiaEmail = new infoCambiaEmails();
            infoCambiaEmail.VPDI = default(string);
            infoCambiaEmail.TipoCorreoElectronico = default(string);
            infoCambiaEmail.SecuenciaCorreo = default(string);
            infoCambiaEmail.TipoOperacion = default(string);
            infoCambiaEmail.CorreoElectronico = default(string);
            infoCambiaEmail.CorreoElectronicoIndPreferido = default(string);

            cambiaEmail.lstinfoCambiaEmails = new List<infoCambiaEmails>();
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail);


            #endregion
            mockProspectRepository.Setup(r => r.UpdateCambiaEmail(It.IsAny<CambiaEmail>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaEmail(cambiaEmail);
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

                var expectedErrorMessage2 = "Tipo Correo Electronico es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "Secuencia de Correo es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "Tipo Operacion  es requerido";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "Correo Electronico es requerido";
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
        public void CambiaEmailLongitudExtendidaSucessTest()
        {
            //Arrange
            #region Informacion de CambiaEmail


            cambiaEmail = new CambiaEmail();

            cambiaEmail.IdBanner = "00294811";
            var infoCambiaEmail = new infoCambiaEmails();
            infoCambiaEmail.VPDI = "3132132132132321321";
            infoCambiaEmail.TipoCorreoElectronico = "123456";
            infoCambiaEmail.SecuenciaCorreo = "12345678912345679812345679";
            infoCambiaEmail.TipoOperacion = "123546";
            infoCambiaEmail.CorreoElectronico = "123456789123456789112345678912345678911234567891234567891123456789123456789112345678912345678911234567891234567891123456789123456789123456789";
            infoCambiaEmail.CorreoElectronicoIndPreferido = "123546";

            cambiaEmail.lstinfoCambiaEmails = new List<infoCambiaEmails>();
            cambiaEmail.lstinfoCambiaEmails.Add(infoCambiaEmail);


            #endregion
            mockProspectRepository.Setup(r => r.UpdateCambiaEmail(It.IsAny<CambiaEmail>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateCambiaEmail(cambiaEmail);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 6;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "La longitud máxima de VPDI es de 6 caracteres";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage2 = "La longitud máxima de Tipo Correo Electronico  es de 4 caracteres";
                var actualErrorMessage2 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de Secuencia de Correo es de 18 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "La longitud máxima de Tipo Operacion  es de 1 caracter";
                var actualErrorMessage4 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "La longitud máxima de  Correo Electronico es de 128 caracteres";
                var actualErrorMessage5 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "La longitud máxima de Correo Electronico IndPreferido es de 1 caracter";
                var actualErrorMessage6 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);
                        



                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
}
