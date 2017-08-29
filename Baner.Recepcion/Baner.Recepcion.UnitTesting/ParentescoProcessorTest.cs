using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Moq;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.BusinessTypes;
using System.Collections.Generic;
using Baner.Recepcion.BusinessTypes.Exceptions;
using System.Linq;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class ParentescoProcessorTest
{
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        Parentesco parentesco;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de Parentesco
            parentesco = new Parentesco();
            parentesco.IdBanner = "00";
             #region listParentesco
            var listParentesco = new InfoParentesto();
            listParentesco.ROWID = "123";
            listParentesco.TipoOperacion = "I";
            listParentesco.ParentescoTipoRelacion = "P";
            listParentesco.ParentescoNombre = "Edgar";
            listParentesco.ParentescoApellidos = "Urquizu Franco";
            listParentesco.ParentescoDireccion = "1";
            listParentesco.ParentescoVive = "Y";
            listParentesco.VPDI = "UAN";
            #endregion
            parentesco.ListInfoParentesco = new List<InfoParentesto>();
            parentesco.ListInfoParentesco.Add(listParentesco);
            #endregion
        }

        [TestMethod]
        public void ParentescoTest()
        {
            //Arrange
           
            mockProspectRepository.Setup(r => r.UpdateParentesco(It.IsAny<Parentesco>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateParentesco(parentesco);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateParentesco(It.IsAny<Parentesco>()));
        }

        /// <summary>
        /// validar sin campos no requeridos
        /// </summary>
        [TestMethod]
        public void ParentescoCamposNoRequeridosTest()
        {
            //Arrange
            #region Informacion de Parentesco
            parentesco = new Parentesco();
            parentesco.IdBanner = "00";
            #region listParentesco
            var listParentesco = new InfoParentesto();
            listParentesco.ROWID = "1";
            listParentesco.TipoOperacion = "I";
            listParentesco.ParentescoTipoRelacion = "P";
            listParentesco.ParentescoNombre = "Edgar";
            listParentesco.ParentescoApellidos = "Urquizu Franco";
            listParentesco.ParentescoDireccion = default(string);
            listParentesco.ParentescoVive = default(string);
            listParentesco.VPDI = "UAS";

            #endregion
            parentesco.ListInfoParentesco = new List<InfoParentesto>();
            parentesco.ListInfoParentesco.Add(listParentesco);
            #endregion
            mockProspectRepository.Setup(r => r.UpdateParentesco(It.IsAny<Parentesco>())).Returns(true);
            //Act
            var resultado = prospectProcessor.UpdateParentesco(parentesco);
            //Assert
            mockProspectRepository.Verify(r => r.UpdateParentesco(It.IsAny<Parentesco>()));
        }
        /// <summary>
        /// Validar Informacion Parentesco null
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ParentescoSinInformacionSucessTest()
        {
            //Arrange
            #region Informacion Parentesco

            parentesco = new Parentesco();
            parentesco.IdBanner = "00";

            parentesco.ListInfoParentesco = null;
            #endregion
            mockProspectRepository.Setup(r => r.UpdateParentesco(It.IsAny<Parentesco>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateParentesco(parentesco);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "Debe tener registrada información de parentesco";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  longitud excedida 
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ParentescolongitudexcedidaSucessTest()
        {
            //Arrange
            #region Informacion de Parentesco
            parentesco = new Parentesco();
            parentesco.IdBanner = "00";
            #region listParentesco
            var listParentesco = new InfoParentesto();
            listParentesco.ROWID = "123456";
            listParentesco.TipoOperacion = "I";
            listParentesco.ParentescoTipoRelacion = "121321132323P";
            listParentesco.ParentescoNombre = "EdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgarEdgar";
            listParentesco.ParentescoApellidos = "Urquizu FrancoUrquizu FrancoUrquizu FrancoUrquizu FrancoUrquizu FrancoUrquizu Franco";
            listParentesco.ParentescoDireccion = "1321654651231";
            listParentesco.ParentescoVive = "Yes";
            listParentesco.VPDI = "dasdasdasd";

            #endregion
            parentesco.ListInfoParentesco = new List<InfoParentesto>();
            parentesco.ListInfoParentesco.Add(listParentesco);
            #endregion
            mockProspectRepository.Setup(r => r.UpdateParentesco(It.IsAny<Parentesco>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateParentesco(parentesco);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 6;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);               

                var expectedErrorMessage2 = "La longitud máxima de Parentesco Tipo relación es de 1 caracter";
                var actualErrorMessage2 = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "La longitud máxima de Parentesco Nombre es de 60 caracteres";
                var actualErrorMessage3 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "La longitud máxima de Parentesco Apellidos es de 60 caracteres";
                var actualErrorMessage4 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "La longitud máxima de Parentesco Dirección es de 2 caracteres";
                var actualErrorMessage5 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);

                var expectedErrorMessage6 = "La longitud máxima de Parentesco Vive es de 1 caracter";
                var actualErrorMessage6 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage6, actualErrorMessage6);

                var expectedErrorMessage7 = "La longitud máxima de VPDI es de 4 caracteres";
                var actualErrorMessage7 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage7, actualErrorMessage7);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        /// <summary>
        /// Validar  Campos Requeridos
        /// </summary>
        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ParentescoCamposRequeridosSucessTest()
        {
            //Arrange
            #region Informacion de Parentesco
            parentesco = new Parentesco();
            parentesco.IdBanner = "00";
            #region listParentesco
            var listParentesco = new InfoParentesto();
            listParentesco.ParentescoTipoRelacion = default(string);
            listParentesco.ParentescoNombre = default(string);
            listParentesco.ParentescoApellidos = default(string);
            listParentesco.ParentescoDireccion = default(string);
            listParentesco.ParentescoVive = default(string);
            listParentesco.VPDI = default(string);

            #endregion
            parentesco.ListInfoParentesco = new List<InfoParentesto>();
            parentesco.ListInfoParentesco.Add(listParentesco);
            #endregion
            mockProspectRepository.Setup(r => r.UpdateParentesco(It.IsAny<Parentesco>())).Returns(true);
            //Act
            try
            {
                var resultado = prospectProcessor.UpdateParentesco(parentesco);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 6;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El atributo ROWID es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                var expectedErrorMessage1 = "El atributo TipoOperacion es requerido";
                var actualErrorMessage1 = ex.Errors.ElementAt(1);
                Assert.AreEqual<string>(expectedErrorMessage1, actualErrorMessage1);

                var expectedErrorMessage2 = "Parentesco Tipo relación es requerido";
                var actualErrorMessage2 = ex.Errors.ElementAt(2);
                Assert.AreEqual<string>(expectedErrorMessage2, actualErrorMessage2);

                var expectedErrorMessage3 = "Parentesco Nombre es requerido";
                var actualErrorMessage3 = ex.Errors.ElementAt(3);
                Assert.AreEqual<string>(expectedErrorMessage3, actualErrorMessage3);

                var expectedErrorMessage4 = "Parentesco Apellidos es requerido";
                var actualErrorMessage4 = ex.Errors.ElementAt(4);
                Assert.AreEqual<string>(expectedErrorMessage4, actualErrorMessage4);

                var expectedErrorMessage5 = "VPDI es requerido";
                var actualErrorMessage5 = ex.Errors.ElementAt(5);
                Assert.AreEqual<string>(expectedErrorMessage5, actualErrorMessage5);


                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

    }
    }
