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
    public class srvCambioResultadoExamenController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;

        public srvCambioResultadoExamenController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        [HttpPost]
        public HttpResponseMessage ActualizaResultadoExamen(ResultadoExamen resultadoExamen)
        {
            try
            {
                if (resultadoExamen == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El Resultado examen no puede ir vacio");
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.UpdateResultadoExamen(resultadoExamen));
            }
            catch (BusinessLayerValidationException ex)
            {
                string errores = "";
                foreach (var e in ex.Errors)
                {
                    errores += string.Format("{0}\n ", e);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, errores, ex);
            }
            catch (PickListException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (LookupException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (CRMExceptionB ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Errors);
            }

            catch (Exception ex)
            {
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }
    }
}
