using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.BusinessTypes.Extensions;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer.Cache;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
//using System.Web.Http.Cors;
//using Rhino.Crm365Connector;
using CRM365.Conector;
using Rhino.RetrieveBearerToken;
using Rhino.RetrieveBearerToken.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Baner.Recepcion.DataLayer
{
    public class BannerRepository : IBannerRepository
    {
        private readonly IOrganizationService _xrmServerConnection;
        private readonly MemoryCacher LocalCache;
        //public Rhino.Crm365Connector.Service service { get; set; }
        public CRM365.Conector.Service service { get; set; }
        public BannerRepository()
        {
            //if (ConfigurationManager.ConnectionStrings.Count == 0)
            //    throw new ConfigurationSettingsException("No se han configurado los atributos de conexion correspondientes [CRM]");

            //if (ConfigurationManager.ConnectionStrings["CRM"] == null || string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString))
            //    throw new ConfigurationSettingsException("No se ha configurado [CRM]");
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            // this.service = new Rhino.Crm365Connector.Service(org, "", user, pass);
            this.service = new CRM365.Conector.Service(org, "", user, pass);

            _xrmServerConnection = this.service.OrganizationService;
            LocalCache = new MemoryCacher();

        }

        public List<RespuestaCoincidencia> ConsultarCoincidencias(Coincidencias coincidencia)
        {
            //Token Banner
            var localtoken = "TokenCache";
            Token token = default(Token);
            var cacheToken = LocalCache.GetValue(localtoken);

            
            if (cacheToken == null)
            {
                #region Seguridad
                var uriToken = ObtenerVariableSistema("ua_variablesistema", "UrlToken");
               // var uriToken = @"https://rua-integ-dev.ec.lcred.net/wsBannerCRMP/o/Server";

                var aplicacion = ObtenerVariableSistema("ua_variablesistema", "AplicacionSeguridad");
                //var aplicacion = "Banner";

                var secret = ObtenerVariableSistema("ua_variablesistema", "SecretSeguridad");
                //var secret = "ygCz85Z7SBA5HhXCLEjp/Vfb9j1oRzesmBVPNal8u+2ggb0TQO662/xNfyPf2NCRvwA/ppY/OYVn38Eu6w9Sgg==";

                var usuario = ObtenerVariableSistema("ua_variablesistema", "UsuarioSeguridad");
                //var usuario = "Banner";

                SecurityToken st = new SecurityToken();
                token = st.RetrieveBearerToken(uriToken, aplicacion, secret, usuario);
                #endregion

                LocalCache.Add(localtoken, token, DateTimeOffset.UtcNow.AddHours(2));
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                token = (Token)cacheToken;
            }


            var localUrl = "UrlBannerCoincidencia";
            var url = default(string);
            var cacheurl = LocalCache.GetValue(localUrl);
            if (cacheurl == null)
            {// Obtenemos url del WS a consumir       
               // url = @"https://rua-integ-dev.ec.lcred.net/wsBannerCRMP/api/srvGestionCoincidencias";//api/srvGestionCoincidencias 
               url=   ObtenerVariableSistema("ua_variablesistema", "url ConsultaCoincidencia");//URL wsBanner
                LocalCache.Add(localUrl, url, DateTimeOffset.UtcNow.AddHours(2));
                
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                url = (string)cacheurl;
            }






            #region Consumir servicio de validacion

            HttpClient proxy = new HttpClient();
            // proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var responsePost = proxy.PostAsJsonAsync(url, coincidencia).Result;

            if (responsePost.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var mensajeerror = responsePost.Content.ReadAsStringAsync().Result;
                throw new BannerException("Ocurrio un error al validar en Banner: " + mensajeerror);
            }

            var resultado =
             responsePost.Content.ReadAsStringAsync().Result;


            var mensaje = JsonConvert.DeserializeObject<SinCoincidencias>(resultado,
                 new JsonSerializerSettings
                 {
                     Error = HandleDeserializationError
                 });


            var coincidencias = JsonConvert.DeserializeObject<List<RespuestaCoincidencia>>(resultado, new JsonSerializerSettings
            {
                Error = HandleDeserializationError
            });


            if (mensaje != null && !string.IsNullOrWhiteSpace(mensaje.Mensaje))
                throw new BannerException(mensaje.Mensaje);
            else
            {
                foreach (var c in coincidencias)
                {
                    if (c.fechaNacimiento != null)
                        c.fecha = c.fechaNacimiento.GetDate();
                }
                return coincidencias;
            }

            #endregion

        }

        public bool CreateAccountBanner(CrearCuentaBanner createaccountBanner)
        {
            var localtoken = "TokenCache";
            Token token = default(Token);
            var cacheToken = LocalCache.GetValue(localtoken);

            if (cacheToken == null)
            {
                #region Seguridad
                // var uriToken = ObtenerVariableSistema("rs_variablesistema", "UrlToken");
                var uriToken = @"https://rua-integ-dev.ec.lcred.net/wsBannerCRMP/o/Server";
                //var aplicacion = ObtenerVariableSistema("rs_variablesistema", "AplicacionSeguridad");
                var aplicacion = "Banner";
                //var secret = ObtenerVariableSistema("rs_variablesistema", "SecretSeguridad");
                var secret = "ygCz85Z7SBA5HhXCLEjp/Vfb9j1oRzesmBVPNal8u+2ggb0TQO662/xNfyPf2NCRvwA/ppY/OYVn38Eu6w9Sgg==";
                //var usuario = ObtenerVariableSistema("rs_variablesistema", "UsuarioSeguridad");
                var usuario = "Banner";

                SecurityToken st = new SecurityToken();
                token = st.RetrieveBearerToken(uriToken, aplicacion, secret, usuario);
                #endregion

                LocalCache.Add(localtoken, token, DateTimeOffset.UtcNow.AddHours(2));
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                token = (Token)cacheToken;
            }

            var localUrl = "UrlCreateCuentaBanner";
            var url = default(string);
            var cacheurl = LocalCache.GetValue(localUrl);
            if (cacheurl == null)
            {// Obtenemos url del WS a consumir       
                url = @"https://rua-integ-dev.ec.lcred.net/wsBannerCRMP/api/srvAltaProspectoBanner";// ObtenerVariableSistema("rs_variablesistema", "ConsultaCoincidencia");//URL wsBanner
                LocalCache.Add(localUrl, url, DateTimeOffset.UtcNow.AddHours(2));
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                url = (string)cacheurl;
            }


            #region Consumir servicio de validacion

            HttpClient proxy = new HttpClient();
            // proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var responsePost = proxy.PostAsJsonAsync(url, createaccountBanner).Result;

            if (responsePost.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var mensajeerror = responsePost.Content.ReadAsStringAsync().Result;
                throw new BannerException("Ocurrio un error al validar en Banner: " + mensajeerror);
            }

            var resultado =
             responsePost.Content.ReadAsStringAsync().Result;


            var mensaje = JsonConvert.DeserializeObject<cuentaBannerClass>(resultado,
                 new JsonSerializerSettings
                 {
                     Error = HandleDeserializationError
                 });
            var regisban = JsonConvert.DeserializeObject<cuentaBannerClass>(resultado);

            var coincidencias = JsonConvert.DeserializeObject<cuentaBannerClass>(resultado, new JsonSerializerSettings
            {
                Error = HandleDeserializationError
            });

            return true;
            //if (mensaje != null && !string.IsNullOrWhiteSpace(mensaje.Mensaje))
            //    throw new BannerException(mensaje.Mensaje);
            //else
            //{
            //    foreach (var c in coincidencias)
            //    {
            //        if (c.fechaNacimiento != null)
            //            c.fecha = c.fechaNacimiento.GetDate();
            //    }
            //    return coincidencias;
            //}

            #endregion
        }

        public bool CrearOportunidad(OportunidadBanner oportunidadBanner)
        {
            var localtoken = "TokenCache";
            Token token = default(Token);
            var cacheToken = LocalCache.GetValue(localtoken);

            if (cacheToken == null)
            {
                #region Seguridad
                // var uriToken = ObtenerVariableSistema("rs_variablesistema", "UrlToken");
                var uriToken = @"https://rua-integ-dev.ec.lcred.net/wsBannerCRMP/o/Server";
                //var aplicacion = ObtenerVariableSistema("rs_variablesistema", "AplicacionSeguridad");
                var aplicacion = "Banner";
                //var secret = ObtenerVariableSistema("rs_variablesistema", "SecretSeguridad");
                var secret = "ygCz85Z7SBA5HhXCLEjp/Vfb9j1oRzesmBVPNal8u+2ggb0TQO662/xNfyPf2NCRvwA/ppY/OYVn38Eu6w9Sgg==";
                //var usuario = ObtenerVariableSistema("rs_variablesistema", "UsuarioSeguridad");
                var usuario = "Banner";

                SecurityToken st = new SecurityToken();
                token = st.RetrieveBearerToken(uriToken, aplicacion, secret, usuario);
                #endregion

                LocalCache.Add(localtoken, token, DateTimeOffset.UtcNow.AddHours(2));
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                token = (Token)cacheToken;
            }


            var localUrl = "UrlCreateCuentaBanner";
            var url = default(string);
            var cacheurl = LocalCache.GetValue(localUrl);
            if (cacheurl == null)
            {// Obtenemos url del WS a consumir       
                url = @"https://rua-integ-dev.ec.lcred.net/wsBannerCRMP/api/srvRegistraOportunidad";// ObtenerVariableSistema("rs_variablesistema", "ConsultaCoincidencia");//URL wsBanner
                LocalCache.Add(localUrl, url, DateTimeOffset.UtcNow.AddHours(2));
            }
            else
            { // Si esta en cache el catalogo, regresarlo
                url = (string)cacheurl;
            }


            #region Consumir servicio de validacion

            HttpClient proxy = new HttpClient();
            // proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            proxy.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var responsePost = proxy.PostAsJsonAsync(url, oportunidadBanner).Result;

            if (responsePost.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var mensajeerror = responsePost.Content.ReadAsStringAsync().Result;
                throw new BannerException("Ocurrio un error  en Banner: " + mensajeerror);
            }

            var resultado =
             responsePost.Content.ReadAsStringAsync().Result;


            var mensaje = JsonConvert.DeserializeObject<cuentaBannerClass>(resultado,
                 new JsonSerializerSettings
                 {
                     Error = HandleDeserializationError
                 });
            var regisban = JsonConvert.DeserializeObject<cuentaBannerClass>(resultado);

            var coincidencias = JsonConvert.DeserializeObject<cuentaBannerClass>(resultado, new JsonSerializerSettings
            {
                Error = HandleDeserializationError
            });

           
           

            #endregion
            return false;
        }

        private void HandleDeserializationError(object sender, ErrorEventArgs args)
        {
            var currenterror = args.ErrorContext.Error.Message;
            args.ErrorContext.Handled = true;
        }


        private class SinCoincidencias
        {
            public string Mensaje { get; set; }
        }

        private class cuentaBannerClass
        {
            public string ID { get; set; }
            public string  Status { get; set; }
            public string Mensaje { get; set; }

        }
        private string ObtenerVariableSistema(string EntityLogicalName, string Variable)
        {
            string resultado = string.Empty;
            
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,
                EntityName = EntityLogicalName,
                ColumnSet = new ColumnSet("ua_valortexto"),
                Criteria =
                {
                    Conditions = {
                         new ConditionExpression("ua_name", ConditionOperator.Equal, Variable)
                    }
                },
            };

            EntityCollection ec = _xrmServerConnection.RetrieveMultiple(query);

            if (ec.Entities.Any())
                resultado = ec.Entities[0].GetAttributeValue<string>("ua_valortexto");

            return resultado;
        }



    }
}
