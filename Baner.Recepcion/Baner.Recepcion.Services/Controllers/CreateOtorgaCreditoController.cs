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
    [Authorize]
    public class CreateOtorgaCreditoController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;
        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prospectProcessor"></param>
        public CreateOtorgaCreditoController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        /// <summary>
        /// Metodo para crear Otorga Credito en CRM
        /// </summary>
        /// <param name="otorgaCredito">Clase que define la estructura de OtorgaCredito</param>
        /// <returns>Respuesta en formato HTTPResponseMessage, Incluye el Guid que es el identificador de CRM</returns>
        [HttpPost]
        public HttpResponseMessage Create(OtorgaCredito otorgaCredito)
        {

            try
            {
                if (otorgaCredito == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "otorgaCredito no puede ir vacio");
                }

                return Request.CreateResponse<Guid>(HttpStatusCode.OK, _prospectProcessor.CreateOtorgaCredito(otorgaCredito));
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
            catch (Exception ex)
            {
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }
    }
}
