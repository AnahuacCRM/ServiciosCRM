using System;
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
    public class UpdateFechaExamenAdmisionController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;
        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prospectProcessor"></param>
        public UpdateFechaExamenAdmisionController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        /// <summary>
        /// Metodo para asignar Fecha de Examen de Admision.
        /// </summary>
        /// <param name="FechaExamenAdmision">Objeto que define atributos para la asignacion de fechas y numero de solicitud.</param>
        /// <returns>Respuesta verdadera o falsa de la accion del metodo.</returns>
        [HttpPost]
        public HttpResponseMessage Post(FechaExamenAdmision FechaExamenAdmision)
        {
            try
            {
                if (FechaExamenAdmision == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto FechaExamenAdmision no puede ir vacio");
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.FechaExamenAdmision(FechaExamenAdmision));
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
