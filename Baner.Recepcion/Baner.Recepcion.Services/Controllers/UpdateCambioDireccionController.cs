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
    public class UpdateCambioDireccionController : ApiController
    {

        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prospectProcessor"></param>
        public UpdateCambioDireccionController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        /// <summary>
        /// Metodo para actualizar datos del domicilio.
        /// </summary>
        /// <param name="cambiadomicilio">Objeto que define atributos para el Objeto Direccion.</param>
        /// <returns>Respuesta verdadera o falsa de la accion del metodo.</returns>
        [HttpPost]
        public HttpResponseMessage Post(CambiaDomicilio cambiadomicilio)
        {
            try
            {
                if (cambiadomicilio == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto CambiaDomicilio no puede ir vacio");
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.UpdateCambiaDomicilio(cambiadomicilio));
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
