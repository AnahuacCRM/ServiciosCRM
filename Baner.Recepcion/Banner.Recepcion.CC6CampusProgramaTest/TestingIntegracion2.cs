
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.Net.Http;
using System.Net;

using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.Services.Controllers;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Baner.Recepcion.DataLayer.CRM;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;
using Baner.Recepcion.Services.Models;
using System.Configuration;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Linq;
using XRM;
using System.Threading;
using Rhino.Crm2016Connector;

namespace Banner.Recepcion.CC6CampusProgramaTest
{
    [TestClass]
    public class TestingIntegracion2
    {

        #region Propiedades
        ILogger _logger;
        IProspectRepository _prospectRepository;
        IProspectProcessor _prospectprocessor;
        ICatalogRepository _catalogrepository;
        IPickListRepository _picklistrepository;
        IServerConnection serverconnection;
        IOpportunityRepository opportunityRepository;
        CreateProspectController _createprospectcontroller;
        MarcaTransferidoController _MarcaTransferidoController;


        //Token token;
        NewProspect newprospect;
        Guid IdCRM = default(Guid);
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            _logger = new DebugerLogger();
            serverconnection = XRMServerConnection.GetInstance as IServerConnection;
            _catalogrepository = new CatalogRepository();
            _picklistrepository = new PickListRepository();
            opportunityRepository = new OpportunityRepository();
            _prospectRepository = new ProspectRepository( _catalogrepository, _picklistrepository, opportunityRepository);
            _prospectprocessor = new ProspectProcessor(_logger, _prospectRepository);
            _createprospectcontroller = new CreateProspectController(_logger, _prospectprocessor);
            _MarcaTransferidoController = new MarcaTransferidoController(_logger, _prospectprocessor);

            _createprospectcontroller.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };

            _MarcaTransferidoController.Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            };


            #region Informacion de Prospecto
            newprospect = new NewProspect()
            {
                //IdCRM = new Guid("3638c958-5a3d-e611-80ec-6c3be5a878bc"),
                IdBanner = "0000006",
                Nombre = "Argelis",
                Apellidos = "Torres*Cuellar",
                Campus = "UAM",
                CampusVPD = "UAC",
                CiudadNacimiento = "M09",
                ColegioProcedencia = "1000",
                Escuela = "AC",
                EstadoCivil = "S",
                EstadoNacimiento = "M09",
                EstatusSolicitud = "C",
                FechaNacimiento = new CustomDate() { Year = 1993, Month = 12, Day = 28 },
                Nacionalidad = "AL",
                PeriodoId = "201560",
                Programa1 = "LC-DERE-16",
                Promedio = "9.0",
                ReligionId = "CA",
                SegundoNombre = "Noe",
                Sexo = "M",
                CodigoTipoadmision = "AA",
                CodigoTipoAlumno = "S",
                Nivel = "LC",
                NumeroSolicitud = 1,
            };
            #endregion
            #region Direccion
            var d = new Direccion()
            {
                Calle = "PARQUE DE VALENCIA NO. 36 - 1",
                //Ciudad = "Ciudad de México",
                CodigoPostal = "01000",
                Colonia = "San Ángel",
                DelegacionMunicipioId = "09010",
                Estado = "M09",
                //Estado = "D1",//estado mal
                Numero = "PARQUES DE LA HERRADURA",
                PaisId = "99",
                //Preferido="",
                SecuenciaDireccion = 1,
                TipoDireccionId = "PA"
            };
            //var d = new Direccion()
            //{
            //    Calle = "Calle Rodin #45 Villas del Arte",
            //    //Ciudad = "Ciudad de México",
            //    CodigoPostal = "77500",
            //    Colonia = "Villas Tropicales",
            //    DelegacionMunicipioId = "23005",
            //    //Estado = "DF",
            //    Estado = "M23",
            //    Numero = "mZA. 61 Lte. 8",
            //    PaisId = "99",
            //    //Preferido="",
            //    SecuenciaDireccion = 1,
            //    TipoDireccionId = "PR"
            //};
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

            var co2 = new Correo()
            {
                CorreoElectronico1 = "otro@hotmail.com",
                IndPreferido = "Y",
                TipoCorreoElectronicoId = "CASA",
                SecuenciaCorreo = "1"
            };
            newprospect.Correos = new List<Correo>();
            newprospect.Correos.Add(co);
            #endregion
            #region Tutor
            var tut = new PadreoTutor()
            {
                ROWID = "XYZ1",
                FirstName = "Argelis Noe",
                LastName = "Torres *Cuellar",
                Parentesco = "P",
                Vive = "N"
            };
            newprospect.PadreoTutor = new List<PadreoTutor>();
            newprospect.PadreoTutor.Add(tut);
            #endregion
            //ArrangeSecurity();

        }


        //[TestCleanup]
        public void Clear()
        {

            //serverconnection.Service.Delete()


            #region Recuperar Registros a Borrar
            var tutores = RetrieveInfo(Lead.EntityLogicalName, "rs_solicitante", IdCRM.ToString(), "leadid");
            var Telefonos = RetrieveInfo(rs_telefono.EntityLogicalName, "rs_solicitanteid", IdCRM.ToString(), "rs_telefonoid");
            var Direcciones = RetrieveInfo(rs_direccion.EntityLogicalName, "rs_solicitante", IdCRM.ToString(), "rs_direccionid");
            var Correos = RetrieveInfo(rs_correoelectronico.EntityLogicalName, "rs_solicitanteid", IdCRM.ToString(), "rs_correoelectronicoid");

            var contactos = RetrieveInfo(Contact.EntityLogicalName, "rs_idbanner", newprospect.IdBanner, "contactid");


            var opsortunities = new List<Guid>();
            for (int i = 0; i < 100; i++)
            {
                opsortunities = RetrieveInfo("opportunity", "rs_idbanner", newprospect.IdBanner, "opportunityid");

                if (opsortunities != null && opsortunities.Any())
                    break;
                else
                {
                    Thread.Sleep(100);
                }
            }
            #endregion

            #region Editar Registros para poder borrarlos
            var tasksedit = new List<System.Threading.Tasks.Task>();
            foreach (var item in Telefonos)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    rs_telefono t = new rs_telefono(); t.rs_telefonoId = item;
                    t.rs_banderaintegraciones = true; serverconnection.Service.Update(t);
                });
                tasksedit.Add(task);
            }


            foreach (var item in Direcciones)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    rs_direccion d = new rs_direccion(); d.rs_direccionId = item;
                    d.rs_banderaintegraciones = true; serverconnection.Service.Update(d);
                });
                tasksedit.Add(task);
            }

            foreach (var item in Correos)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    rs_correoelectronico em = new rs_correoelectronico(); em.rs_correoelectronicoId = item;
                    em.rs_banderaIntegracion = true; serverconnection.Service.Update(em);
                });
                tasksedit.Add(task);
            }

            System.Threading.Tasks.Task.WaitAll(tasksedit.ToArray());
            #endregion

            #region Eliminacion de registros de prueba
            var tasksdelete = new List<System.Threading.Tasks.Task>();

            foreach (var item in tutores)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    serverconnection.Service.Delete(Lead.EntityLogicalName, item);
                });
                tasksdelete.Add(task);
            }

            foreach (var item in Telefonos)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    serverconnection.Service.Delete(rs_telefono.EntityLogicalName, item);
                });
                tasksdelete.Add(task);
            }

            foreach (var item in Direcciones)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    serverconnection.Service.Delete(rs_direccion.EntityLogicalName, item);
                });
                tasksdelete.Add(task);
            }

            foreach (var item in Correos)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    serverconnection.Service.Delete(rs_correoelectronico.EntityLogicalName, item);
                });
                tasksdelete.Add(task);
            }

            foreach (var item in opsortunities)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    serverconnection.Service.Delete(Opportunity.EntityLogicalName, item);
                });
                tasksdelete.Add(task);
            }

            foreach (var item in contactos)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    serverconnection.Service.Delete(Contact.EntityLogicalName, item);
                });
                tasksdelete.Add(task);
            }


            //tasksdelete.Add(taskpreuniversitario);

            System.Threading.Tasks.Task.WaitAll(tasksdelete.ToArray());

            var taskpreuniversitario = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                serverconnection.Service.Delete(rs_prospecto.EntityLogicalName, IdCRM);
            });

            taskpreuniversitario.Wait();
            #endregion


        }

        private List<Guid> RetrieveInfo(string EntityLogicalName, string campoFiltro, string valueFiltro, string camporetrieve)
        {

            EntityCollection ec = default(EntityCollection);
            List<Guid> resultado = new List<Guid>();
            QueryExpression Query = new QueryExpression(EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { camporetrieve }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression(campoFiltro, ConditionOperator.Equal, valueFiltro)
                            }
                        }
                #endregion
            };
            ec = serverconnection.Service.RetrieveMultiple(Query);
            if (ec.Entities != null && ec.Entities.Any())
            {
                foreach (var item in ec.Entities)
                {
                    resultado.Add(new Guid(item[camporetrieve].ToString()));
                }
            }
            return resultado;
        }




        [TestMethod]
        public void TestIntegracion2()
        {
            //arrange


            //act

            var resultadoCreacion = _createprospectcontroller.Create(newprospect);



            //assert
            if (resultadoCreacion.StatusCode != HttpStatusCode.Created)
            {
                var msgresultado = resultadoCreacion.Content.ReadAsStringAsync().Result;
                var objResultado = JsonConvert.DeserializeObject<ResponseNewProspect>(msgresultado);
                _logger.Infomacion(msgresultado);

                Assert.AreEqual(HttpStatusCode.OK, resultadoCreacion.StatusCode);
                Assert.AreNotEqual(null, resultadoCreacion);
                IdCRM = objResultado.IdCRM;
            }



            var opsortunities = default(List<Guid>);
            for (int i = 0; i < 100; i++)
            {
                opsortunities = RetrieveInfo("opportunity", "rs_idbanner", newprospect.IdBanner, "opportunityid");

                if (opsortunities != null && opsortunities.Any())
                    break;
                else
                {
                    Thread.Sleep(1000);
                }
            }


            //if (opsortunities != null)
            //{
            //    var resultadoMarca = _MarcaTransferidoController.Post(opsortunities);
            //    if (resultadoMarca.StatusCode != HttpStatusCode.Created)
            //    {
            //        var msgresultado = resultadoMarca.Content.ReadAsStringAsync().Result;
            //        var objResultado = JsonConvert.DeserializeObject<bool>(msgresultado);
            //        _logger.Infomacion(msgresultado);
            //        Assert.AreEqual(HttpStatusCode.OK, resultadoCreacion.StatusCode);
            //    }

            //}


        }



        [TestMethod]
        public void CleanTest()
        {
            var idpreuniversitario = RetrieveInfo(rs_prospecto.EntityLogicalName, "rs_idbanner", newprospect.IdBanner, "rs_prospectoid");

            if (idpreuniversitario.Any())
            {
                IdCRM = idpreuniversitario.FirstOrDefault();
                //
                Clear();
            }
           
        }




    }
}
