﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Baner.Recepcion.OperationalManagement.Extensions;

namespace Baner.Recepcion.Services.Controllers
{
    [Authorize]
    public class CreatePreUniversitarioController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _preUniversitarioProcessor;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="_preUniversitarioProcessor"></param>
        public CreatePreUniversitarioController(ILogger logger, IProspectProcessor preUniversitarioProcessor)
        {
            _logger = logger;
            _preUniversitarioProcessor = preUniversitarioProcessor;
        }

        /// <summary>
        /// Metodo para crear Prospecto.
        /// </summary>
        /// <param name="preUniversitario">Objeto que define atributos para la creacion de prospecto en la etapa de PreUniversitario.</param>
        /// <returns>Respuesta verdadera o falsa de la accion del metodo.</returns>
        [HttpPost]
        public HttpResponseMessage Post(PreUniversitario preUniversitario)
        {
            try
            {
                if (preUniversitario == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto PreUniversitario no puede ir vacio");
                }

                return Request.CreateResponse<Guid>(HttpStatusCode.OK, _preUniversitarioProcessor.CreatePreUniversitario(preUniversitario));
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }

    }
}
