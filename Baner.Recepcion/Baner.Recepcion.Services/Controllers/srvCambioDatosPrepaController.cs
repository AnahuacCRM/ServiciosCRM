﻿using Baner.Recepcion.BusinessInterfaces;
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
    /// Servicio 9 CRM.
    /// </summary>
    public class srvCambioDatosPrepaController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;

        public srvCambioDatosPrepaController(ILogger pIlogger, IProspectProcessor pIprospectProcessor)
        {
            this._logger = pIlogger;
            this._prospectProcessor = pIprospectProcessor;

        }
        [HttpPost]
        public HttpResponseMessage ActualizaDatosPrepa(DatosPrepa datosPrepa)
        {
            try
            {
                if (datosPrepa == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto Datos Persona no peuede ir vacio");

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.UpdateDatosPrepa(datosPrepa));
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
