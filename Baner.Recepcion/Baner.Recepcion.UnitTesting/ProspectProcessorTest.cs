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
using Baner.Recepcion.BusinessTypes.RespuestasServicio;

namespace Baner.Recepcion.UnitTesting
{
    [TestClass]
    public class ProspectProcessorTest
    {
        #region Mocks
        Mock<IProspectRepository> mockProspectRepository;
        Mock<ILogger> mockLogger;
        Mock<IPickListRepository> mockpicklistRepository;
        Mock<ICatalogRepository> mockCatalogRepository;
        #endregion

        ProspectProcessor prospectProcessor;

        NewProspect newprospect;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger>();
            mockProspectRepository = new Mock<IProspectRepository>();
            mockpicklistRepository = new Mock<IPickListRepository>();
            mockCatalogRepository = new Mock<ICatalogRepository>();

            prospectProcessor = new ProspectProcessor(mockLogger.Object, mockProspectRepository.Object);
            //newprospect = new NewProspect();

            #region Informacion de Prospecto
            newprospect = new NewProspect()
            {
                IdBanner = "00",
                Nombre = "Argelis",
                Apellidos = "Torres *Cuellar",
                Campus = "UAC",
                CampusVPD = "UAC",
                CiudadNacimiento = "DF",
                ColegioProcedencia = "1000",
                Escuela = "AC",
                EstadoCivil = "S",
                EstadoNacimiento = "DF",
                EstatusSolicitud = "C",
                FechaNacimiento = new CustomDate() { Year = 1983, Month = 5, Day = 30 },
                Nacionalidad = "AL",
                PeriodoId = "201645",
                Programa1 = "ACT",
                Promedio = "9.0",
                ReligionId = "CA",
                SegundoNombre = "Noe",
                Sexo = "M",
                CodigoTipoadmision = "AA",
                CodigoTipoAlumno = "S",
                Nivel = "LC"

            };
            #endregion
            #region Direccion
            var d = new Direccion()
            {
                Calle = "Aguascalientes",
                //Ciudad = "DF",
                CodigoPostal = "01270",
                Colonia = "Golondrinas",
                DelegacionMunicipioId = "09001",
                Estado = "DF",
                Numero = "1234",
                PaisId = "99",
                //Preferido="",
                SecuenciaDireccion = 1,
                TipoDireccionId = "PA"
            };
            newprospect.Direcciones = new List<Direccion>();
            newprospect.Direcciones.Add(d);
            #endregion
            #region Telefono
            var t = new Telefono()
            {
                LadaTelefono = "123",
                PreferidoTelefono = "Y",
                SecuenciaTelefono = 1,
                Telefono1 = "2345678",
                TipoTelefono = "PR"
            };
            newprospect.Telefonos = new List<Telefono>();
            newprospect.Telefonos.Add(t);
            #endregion
            #region CorreoElectronico
            var co = new Correo()
            {
                CorreoElectronico1 = "falonso@hotmail.com",
                IndPreferido = "Y",
                TipoCorreoElectronicoId = "PERS",
                SecuenciaCorreo = "1"
            };
            newprospect.Correos = new List<Correo>();
            newprospect.Correos.Add(co);
            #endregion
            #region Tutor
            var tut = new PadreoTutor()
            {
                ROWID="1",
                FirstName = "Argelis Noe",
                LastName = "Torres *Cuellar",
                //MiddleName = "Noe",
                Parentesco = "P",
                Vive = "N"
            };
            newprospect.PadreoTutor = new List<PadreoTutor>();
            newprospect.PadreoTutor.Add(tut);
            #endregion
        }


        [TestMethod]
        public void NewProspectSucessTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.Create(It.IsAny<NewProspect>())).Returns(It.IsAny<ResponseNewProspect>());
            //Act
            var resultado = prospectProcessor.Create(newprospect);
            //Assert
            mockProspectRepository.Verify(r => r.Create(It.IsAny<NewProspect>()));
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void EmptyIdBannerSucessTest()
        {
            //Arrange
            newprospect.IdBanner = "";
            mockProspectRepository.Setup(r => r.Create(It.IsAny<NewProspect>())).Returns(It.IsAny<ResponseNewProspect>());
            //Act
            try
            {
                var resultado = prospectProcessor.Create(newprospect);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                var expectedErrorMessage = "El Identificador de banner es requerido";
                var actualErrorMessage = ex.Errors.ElementAt(0);
                Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void ExcededLongitudeIdBannerSucessTest()
        {
            //Arrange
            newprospect.IdBanner = "123456789098765432";
            mockProspectRepository.Setup(r => r.Create(It.IsAny<NewProspect>())).Returns(It.IsAny<ResponseNewProspect>());
            //Act
            try
            {
                var resultado = prospectProcessor.Create(newprospect);
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


        //[TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        //public void WithoutAddressSucessTest()
        //{
        //    //Arrange
        //    newprospect.Direcciones = null;
        //    mockProspectRepository.Setup(r => r.Create(It.IsAny<NewProspect>())).Returns(Guid.NewGuid());
        //    //Act
        //    try
        //    {
        //        var resultado = prospectProcessor.Create(newprospect);
        //    }
        //    catch (BusinessLayerValidationException ex)
        //    {
        //        //Assert
        //        var expectedErrorCount = 1;
        //        var actualErrorCount = ex.Errors.Count();
        //        Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

        //        var expectedErrorMessage = "Se debe proporcionar la direccion predeterminada";
        //        var actualErrorMessage = ex.Errors.ElementAt(0);
        //        Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

        //        mockLogger.Verify(l => l.Error(It.IsAny<string>()));
        //        throw;
        //    }
        //}


        [TestMethod, ExpectedException(typeof(BusinessLayerValidationException))]
        public void AddressErrorSucessTest()
        {
            //Arrange
            newprospect.Direcciones = new List<Direccion>();
            var d = new Direccion()
            {
                //Calle = "Aguascalientes",
                //CiudadId = "DF",
                //CodigoPostalId = "01270",
                //Colonia = "Golondrinas",
                //DelegacionMunicipioId = "09001",
                //Estado = "DF",
                //Numero = "1234",
                //PaisId = "99",
                //SecuenciaDireccion = 1,
                //TipoDireccionId = "PA"
            };
            newprospect.Direcciones.Add(d);
            mockProspectRepository.Setup(r => r.Create(It.IsAny<NewProspect>())).Returns(It.IsAny<ResponseNewProspect>());
            //Act
            try
            {
                var resultado = prospectProcessor.Create(newprospect);
            }
            catch (BusinessLayerValidationException ex)
            {
                //Assert
                var expectedErrorCount = 1;
                var actualErrorCount = ex.Errors.Count();
                Assert.AreEqual<int>(expectedErrorCount, actualErrorCount);

                //var expectedErrorMessage = "Se debe proporcionar la direccion predeterminada";
                //var actualErrorMessage = ex.Errors.ElementAt(0);
                //Assert.AreEqual<string>(expectedErrorMessage, actualErrorMessage);

                mockLogger.Verify(l => l.Error(It.IsAny<string>()));
                throw;
            }
        }

        [TestMethod]
        public void NewProspectWithoutNoRerquiredSucessTest()
        {
            //Arrange
            mockProspectRepository.Setup(r => r.Create(It.IsAny<NewProspect>())).Returns(It.IsAny<ResponseNewProspect>());
            #region Limpiar campos no requeridos
            newprospect.NumeroSolicitud = default(int?);
            newprospect.SegundoNombre = default(string);
            newprospect.Apellidos = "Torres";
            newprospect.FechaNacimiento = default(CustomDate);
            //newprospect.Campus = default(string);
            // newprospect.EstatusSolicitud = default(string);//Esta requerido previamente
            newprospect.Sexo = default(string);
            newprospect.Nacionalidad = default(string);
            newprospect.ReligionId = default(string);
            newprospect.EstadoCivil = default(string);
            newprospect.EstadoNacimiento = default(string);
            newprospect.CiudadNacimiento = default(string);
            //newprospect.ColegioProcedencia= default(string);//Esta requerido previamente
            newprospect.Promedio = default(string);
            // newprospect.PeriodoId= default(string);//Esta requerido previamente
            //campos no requeridos en direccion
            foreach (var d in newprospect.Direcciones)
            {
                d.SecuenciaDireccion = default(int);
            }

            newprospect.PadreoTutor = null;
            //newprospect.PadreoTutor.MiddleName= default(string);
            //newprospect.PadreoTutor.Parentesco= default(string);
            //newprospect.PadreoTutor.LastName = "Torres";
            //newprospect.PadreoTutor.Vive= default(string);



            #endregion



            //Act
            var resultado = prospectProcessor.Create(newprospect);
            //Assert
            mockProspectRepository.Verify(r => r.Create(It.IsAny<NewProspect>()));
        }
    }
}
