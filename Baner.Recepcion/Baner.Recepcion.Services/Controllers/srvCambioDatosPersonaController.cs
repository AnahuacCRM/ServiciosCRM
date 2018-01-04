using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
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
    /// <summary>
    /// Servicio 8 CRM.
    /// </summary>
    public class srvCambioDatosPersonaController : ApiController
    {

        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;

        public srvCambioDatosPersonaController(ILogger plogger,IProspectProcessor pprospectprocessor)
        {
            this._logger = plogger;
            this._prospectProcessor = pprospectprocessor;

        }
         [HttpPost]
        public HttpResponseMessage ActualizarDatosPersona(DatosPersona dp)
        {
            try
            {
                if (dp == null)
                 return  Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto DatosPersona no puede ir vacio");

                return Request.CreateResponse<bool>(HttpStatusCode.OK,_prospectProcessor.UpdateDatosPersona(dp));
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
