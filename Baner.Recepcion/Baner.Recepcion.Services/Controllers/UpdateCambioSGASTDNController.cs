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
    public class UpdateCambioSGASTDNController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;
        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prospectProcessor"></param>
        public UpdateCambioSGASTDNController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        /// <summary>
        /// Metodo para actualizar CambioSGASTDN en CRM
        /// </summary>
        /// <param name="cambio">Clase que define la estructura de CambioSGASTDN</param>
        /// <returns>Respuesta en formato HTTPResponseMessage, devuelve un OK si es correcto</returns>
        [HttpPost]
        public HttpResponseMessage Post(CambioSGASTDN cambio)
        {

            try
            {
                if (cambio == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "cambio no puede ir vacio");
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.UpdateCambioSGASTDN(cambio));
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
