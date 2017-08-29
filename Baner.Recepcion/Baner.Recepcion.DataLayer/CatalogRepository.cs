using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer.Cache;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRM;

namespace Baner.Recepcion.DataLayer
{
    public class CatalogRepository : ICatalogRepository
    {

        //private readonly ILogger _logger;
        private readonly IOrganizationService _xrmServerConnection;

        private readonly MemoryCacher LocalCache;

        public CRM365.Conector.Service service { get; set; }

        public CatalogRepository()
        {
            //if (ConfigurationManager.ConnectionStrings.Count == 0)
            //    throw new ConfigurationSettingsException("No se han configurado los atributos de conexion correspondientes [CRM]");

            //if (ConfigurationManager.ConnectionStrings["CRM"] == null || string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString))
            //    throw new ConfigurationSettingsException("No se ha configurado [CRM]");

            //_logger = logger;
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new CRM365.Conector.Service(org, "", user, pass);
            try
            {
                _xrmServerConnection = service.OrganizationService;
            }
            catch(Exception ex)
            {
                var es = ex.Message;
            }

            LocalCache = new MemoryCacher();
        }

        public List<Colonia> ListaColonias(Colonia colonia)
        {
            List<Colonia> resultado = new List<Colonia>();


            QueryExpression Query = new QueryExpression(ua_colonia.EntityLogicalName)
            {
                #region Consulta
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_colonia", "ua_coloniaid", "ua_codigo_postal" }),
                Criteria = {
                            Conditions = {
                                new ConditionExpression("ua_colonia", ConditionOperator.NotNull),
                                new ConditionExpression("ua_colonia", ConditionOperator.Equal,colonia.Nombre),
                            }
                            ,FilterOperator = LogicalOperator.And
                        },
                #region Joins
                LinkEntities = {
                    new LinkEntity
                    {
                             LinkFromEntityName = ua_colonia.EntityLogicalName,
                             LinkToEntityName= ua_estados.EntityLogicalName,
                             LinkFromAttributeName="ua_desc_estado",
                             LinkToAttributeName= "ua_estadosid",
                             JoinOperator= JoinOperator.Inner,
                             Columns= new ColumnSet("ua_codigo_estado"),
                             EntityAlias="Estado",
                             LinkCriteria=
                             {
								//Filtar la colonia por estado
								Conditions=
                                {
                                    new ConditionExpression("ua_codigo_estado",ConditionOperator.Equal,colonia.Estado)
                                }
                             }
                    },
                    new LinkEntity
                    {
                             LinkFromEntityName = ua_colonia.EntityLogicalName,
                             LinkToEntityName= ua_pais.EntityLogicalName,
                             LinkFromAttributeName="ua_desc_pais",
                             LinkToAttributeName= "ua_paisid",
                             JoinOperator= JoinOperator.Inner,
                             Columns= new ColumnSet("ua_codigo_pais"),
                             EntityAlias="Pais",
                             LinkCriteria=
                             {
								//Filtar la colonia por estado
								Conditions=
                                {
                                    new ConditionExpression("ua_codigo_pais",ConditionOperator.Equal,colonia.Pais)
                                }
                             }
                    },
        //            new LinkEntity
        //            {
        //                     LinkFromEntityName = ua_colonia.EntityLogicalName,
        //                     LinkToEntityName= rs_codigopostal.EntityLogicalName,
        //                     LinkFromAttributeName="rs_codigopostalid",
        //                     LinkToAttributeName= "rs_codigopostalid",
        //                     JoinOperator= JoinOperator.Inner,
        //                     Columns= new ColumnSet("rs_name"),
        //                     EntityAlias="CP",
        //                     LinkCriteria=
        //                     {
								////Filtar la colonia por estado
								//Conditions=
        //                        {
        //                            new ConditionExpression("rs_name",ConditionOperator.Equal,colonia.CP)
        //                        }
        //                     }
        //            },
                    new LinkEntity
                    {
                             LinkFromEntityName = ua_colonia.EntityLogicalName,
                             LinkToEntityName= ua_delegacion_municipio.EntityLogicalName,
                             LinkFromAttributeName="ua_desc_municipio",
                             LinkToAttributeName= "ua_delegacion_municipioid",
                             JoinOperator= JoinOperator.Inner,
                             Columns= new ColumnSet("ua_codigo_municipio"),
                             EntityAlias="Municipio",
                             LinkCriteria=
                             {
								//Filtar la colonia por estado
								Conditions=
                                {
                                new ConditionExpression("ua_codigo_municipio",ConditionOperator.NotNull),
                                    new ConditionExpression("ua_codigo_municipio",ConditionOperator.Equal,colonia.DelegacionMunicipio)
                                }
                             }
                    }  
				#endregion
				}
                #endregion
            };

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                foreach (var col in ec.Entities)
                {
                    var col1 = new Colonia()
                    {
                        CP = col["ua_codigo_postal"].ToString(),
                        IdCRM = new Guid(col["ua_coloniaid"].ToString()),
                        DelegacionMunicipio = ((AliasedValue)col["Municipio.ua_codigo_municipio"]).Value.ToString(),
                        Estado = ((AliasedValue)col["Estado.ua_codigo_estado"]).Value.ToString(),
                        Pais = ((AliasedValue)col["Pais.ua_codigo_pais"]).Value.ToString(),
                        Nombre = col["ua_colonia"].ToString()
                    };
                    resultado.Add(col1);
                }
            }


            return resultado;
        }

        public List<Municipio> ListaMunicipio(Municipio municipio)
        {
            List<Municipio> resultado = new List<Municipio>();
            #region Consulta



            QueryExpression Query = new QueryExpression(ua_delegacion_municipio.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "ua_codigo_municipio", "ua_delegacion_municipioid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_codigo_municipio", ConditionOperator.NotNull),
                        new ConditionExpression("ua_codigo_municipio", ConditionOperator.Equal, municipio.CodigoMunicipio)
                    }
                    ,FilterOperator = LogicalOperator.And
                },
                #region JOIN


                LinkEntities = {
                    new LinkEntity {
                        LinkFromEntityName = ua_delegacion_municipio.EntityLogicalName,
                             LinkToEntityName= ua_estados.EntityLogicalName,
                             LinkFromAttributeName="ua_desc_estado",
                             LinkToAttributeName= "ua_estadosid",
                             JoinOperator= JoinOperator.Inner,
                             Columns= new ColumnSet("ua_codigo_estado"),
                             EntityAlias="Estado",
                             LinkCriteria=
                             {
								//Filtar la Municipio por estado
								Conditions=
                                {
                                    new ConditionExpression("ua_codigo_estado",ConditionOperator.Equal,municipio.Estado)

                                }
                             }
                    }
                }
                #endregion
            };
            #endregion
            #region Recuperacion del municipio

            var ec = _xrmServerConnection.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                foreach (var mun in ec.Entities)
                {
                    var mun1 = new Municipio()
                    {
                        IdCRM = new Guid(mun["ua_delegacion_municipioid"].ToString()),
                        CodigoMunicipio = mun["ua_codigo_municipio"].ToString(),
                        Estado = ((AliasedValue)mun["Estado.ua_codigo_estado"]).Value.ToString(),
                    };
                    resultado.Add(mun1);
                }
            }


            return resultado;
            #endregion

            //throw new NotImplementedException("Lista Municipios no implementada");
            //return ListaCatalogo("municipio", rs_delegacionmunicipio.EntityLogicalName, "rs_codigomunicipio", "rs_delegacionmunicipioid");
        }

        public Dictionary<string, string> GetIdMunicipio(string sCalveMunicipio)
        {
            return ListaCatalogo("MunicipioId", ua_delegacion_municipio.EntityLogicalName, "ua_codigo_municipio", "ua_delegacion_municipioid");
            //Guid IdMun = default(Guid);

            //QueryExpression Query = new QueryExpression(ua_delegacion_municipio.EntityLogicalName)
            //{
            //    NoLock = true,
            //    ColumnSet = new ColumnSet(new string[] { "ua_codigo_municipio", "ua_delegacion_municipioid" }),
            //    Criteria = {
            //        Conditions = {
            //            new ConditionExpression("ua_codigo_municipio", ConditionOperator.NotNull),
            //            new ConditionExpression("ua_codigo_municipio", ConditionOperator.Equal, sCalveMunicipio)
            //        }

            //    }
            //};

            //return IdMun;

        }

        public Dictionary<string, string> GetMunicipioAsesor(string sCalveMunicipio)
        {
            ua_delegacion_municipio_asesor munas = new ua_delegacion_municipio_asesor();
            
            return ListaCatalogo("MunicipioAsesor", ua_delegacion_municipio.EntityLogicalName, "ua_codigo_municipio", "ua_delegacion_municipio_asesorid");
            

        }

        public Dictionary<string, string> ListaEstado()
        {

            return ListaCatalogo("estado", ua_estados.EntityLogicalName, "ua_codigo_estado", "ua_estadosid");
        }

        public Dictionary<string, string> ListaCodigoEstado()
        {
            //return ListaCatalogo("estadocodigo", ua_estados.EntityLogicalName, "rs_sinonimocodigo", "rs_codigoestado");



            return ListaCatalogo("estadocodigo", ua_estados.EntityLogicalName, "ua_codigo_estado", "ua_desc_estado");
        }

        public Dictionary<string, string> ListaPeriodo()
        {
            //ua_periodo p = new ua_periodo();
            //p.ua_periodo1
            return ListaCatalogo("Periodos", ua_periodo.EntityLogicalName, "ua_periodo", "ua_periodoid");
        }

        public Dictionary<string, string> ListaPais()
        {
            // ua_pais pais = new ua_pais();
            return ListaCatalogo("pais", ua_pais.EntityLogicalName, "ua_codigo_pais", "ua_paisid");
        }

      

        public Dictionary<string, string> ListaCarreraWebCodigo()
        {

            return ListaCatalogo("CarreraWebCodigo", ua_carrerauniversal.EntityLogicalName, "ua_carrera_universal", "ua_carrerauniversalid");
        }
        public Dictionary<string, string> ListaCampus()
        {

            return ListaCatalogo("campus", BusinessUnit.EntityLogicalName, "name", "businessunitid");
        }

        public Dictionary<string, string> ListaNiveles()
        {

            return ListaCatalogo("NivelesEd", ua_niveles.EntityLogicalName, "ua_codigo_nivel", "ua_nivelesid");

        }

        public Dictionary<string, string> ListaNacionalidad()
        {
            ua_desc_nacionalidad na = new ua_desc_nacionalidad();

            return ListaCatalogo("nacionalidad", ua_desc_nacionalidad.EntityLogicalName, "ua_codigo_nacionalidad", "ua_desc_nacionalidadid");

        }

        public Dictionary<string, string> ListaReligion()
        {

            return ListaCatalogo("religion", ua_religion.EntityLogicalName, "ua_codigo_religion", "ua_religionid");
        }

        public Dictionary<string, string> ListaColegio()
        {

            //return ListaCatalogo("colegio", Account.EntityLogicalName, "accountnumber", "accountid");
            return ListaCatalogo("colegio", ua_colegios.EntityLogicalName, "ua_codigo_colegio", "ua_colegiosid");
        }

       public Dictionary<string, string> ListaPrograma()
        {
          
            return ListaCatalogo("ProgramaN", ua_programaV2.EntityLogicalName, "ua_codigo_del_programa", "ua_programav2id");
        }

        public Dictionary<string, string> ListaEscuela()
        {
            return ListaCatalogo("Escuela", ua_escuela.EntityLogicalName, "ua_codigo_escuela", "ua_escuelaid");
            //return ListaCatalogo("Escuela", ua_escuela.EntityLogicalName, "ua_codigo_colegio", "ua_codigo_escuela");

        }
        public Dictionary<string, string> ConjutoOpciones(string sEntityLoginame, string sCampoDeConjunto)
        {
            return ListaConjuntoOpcioneesPorCampo(sCampoDeConjunto, sEntityLoginame, sCampoDeConjunto);
        }

        private Dictionary<string, string> ListaCatalogo(string cachename, string EntityLogicalName, string campocodigo, string campoid)
        {
            var resultado = default(Dictionary<string, string>);
            EntityCollection ec = default(EntityCollection);
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            var localtoken = cachename;
            //Recuperar de cache los codigos de accion
            var cache = LocalCache.GetValue(localtoken);
            if (cache == null)
            {//Si no estan en cache recuperarlo de la BD

                QueryExpression Query = new QueryExpression(EntityLogicalName)
                {
                    #region Consulta
                    NoLock = true,
                    ColumnSet = new ColumnSet(new string[] { campocodigo, campoid }),
                    Criteria = {
                            Conditions = {
                                new ConditionExpression(campocodigo, ConditionOperator.NotNull)
                            }
                        }
                    #endregion
                };
                ec = _xrmServerConnection.RetrieveMultiple(Query);

                if (ec != null)
                {
                    resultado = new Dictionary<string, string>();
                    //Lo convertimos a mayusclas para que si mandan minusculas al compararlas sean iguales
                    foreach (var item in ec.Entities)
                    {

                        var codigo = item[campocodigo].ToString().ToUpper();
                        var id = item[campoid].ToString();
                        if (resultado.ContainsKey(codigo))
                        {
                            var error = string.Format("Código {0} duplicado en la entidad:{1}", codigo, EntityLogicalName);
                            throw new LookupException(error);
                        }

                        resultado.Add(codigo, id);
                    }



                    LocalCache.Add(localtoken, resultado, DateTimeOffset.UtcNow.AddHours(1));
                }
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                resultado = (Dictionary<string, string>)cache;
            }

            //sw.Stop();
            //Debug.WriteLine("{0} Tiempo recuperacion: {1} ms", cachename, sw.ElapsedMilliseconds);

            return resultado;
        }


        private Dictionary<string, string> ListaConjuntoOpcioneesPorCampo(string cachename, string EntityLogicaname, string CampodeConjunto)
        {
            Dictionary<string, string> dicn = new Dictionary<string, string>();
            var localtoken = cachename;
            //Recuperar de cache los codigos de accion
            var cache = LocalCache.GetValue(localtoken);
            if (cache == null)
            {


                //EntityLogicalName = ua_periodo.EntityLogicalName,
                //    LogicalName = "ua_tipo_periodo",
                //    RetrieveAsIfPublished = true
                RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
                {
                    EntityLogicalName = EntityLogicaname,
                    LogicalName = CampodeConjunto,
                    RetrieveAsIfPublished = true
                };

                // Execute the request
                RetrieveAttributeResponse attributeResponse =
                    (RetrieveAttributeResponse)_xrmServerConnection.Execute(attributeRequest);

                if (attributeResponse.Results.Count > 0)
                {
                    string sNameOpt = "";
                    int valorOpt = -1;
                    int conOp = 0;

                    foreach (PicklistAttributeMetadata ColecionValues in attributeResponse.Results.Values)
                    {
                        foreach (var item in ColecionValues.OptionSet.Options)
                        {
                            sNameOpt = item.Label.LocalizedLabels[0].Label;
                            valorOpt = int.Parse(item.Value.ToString());
                            dicn.Add(sNameOpt, valorOpt.ToString());
                            conOp++;
                        }

                    }
                    LocalCache.Add(localtoken, dicn, DateTimeOffset.UtcNow.AddHours(1));

                }
            }
            else// si lo encontro en cache retorna la cache
                dicn = (Dictionary<string, string>)cache;
            return dicn;
        }
    }
}
