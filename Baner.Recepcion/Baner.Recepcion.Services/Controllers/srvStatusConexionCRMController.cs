
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Baner.Recepcion.OperationalManagement.Extensions;
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
        private readonly ILogger _logger;
        private readonly IMonitoreoConexion _MonitoreoConexion;
        public srvStatusConexionCRMController(ILogger logger, IMonitoreoConexion monitoreoCrm)
        {
            this._logger = logger;
            this._MonitoreoConexion = monitoreoCrm;
        }

        [HttpPost]
        public HttpResponseMessage StatusCrmConexion()
        {
            try
            {
                bool res = _MonitoreoConexion.NotificaConexionCRM();

                if (res)
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, _MonitoreoConexion.NotificaConexionCRM());
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ocurrió un problema al intentar establecer conexión con Dynamics CRM");

            }
            catch (BusinessLayerValidationException ex)
            {
                _logger.Error("BusinessLayerValidationException :" + ex.Message);
                string errores = "";
                foreach (var e in ex.Errors)
                {
                    errores += string.Format("{0}\n ", e);
                }
                _logger.Error("BusinessLayerValidationException Errores : " + errores);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, errores, ex);
            }
            catch (PickListException ex)
            {
                _logger.Error("PickListException  " + ex.Message);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (LookupException ex)
            {
                _logger.Error("LookupException  " + ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception   " + ex.Message);
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }
    }
}
