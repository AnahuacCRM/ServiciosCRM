
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Baner.Recepcion.OperationalManagement.Extensions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Baner.Recepcion.Services.Controllers
{
    public class srvStatusConexionCRMController : ApiController
    {
        

        [HttpGet]
        public HttpResponseMessage StatusCrmConexion()
        {
            try
            {
                string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
                string user = System.Configuration.ConfigurationManager.AppSettings["username"];
                string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

                CRM365.Conector.Service service = new CRM365.Conector.Service(org, "", user, pass);
                IOrganizationService _xrmServerConnection2;

                _xrmServerConnection2 = service.OrganizationService;
                return Request.CreateResponse<bool>(HttpStatusCode.OK, true);


                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "surgio este erro al intentar conectarce al crm  " + ex.Message);


            }


            catch (Exception ex)
            {
                // _logger.Error("Exception   " + ex.Message);
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ocurrio un error al intentar conectarce con crm: " + ex.Message);
            }
        }
    }
}
