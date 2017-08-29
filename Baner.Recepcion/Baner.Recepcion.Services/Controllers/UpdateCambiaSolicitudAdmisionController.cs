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
    public class UpdateCambiaSolicitudAdmisionController : ApiController
    {

        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;
        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prospectProcessor"></param>
        public UpdateCambiaSolicitudAdmisionController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        /// <summary>
        /// Metodo para actualizar Cambia Solicitud Admision
        /// </summary>
        /// <param name="cambiasolicitudadmision">Clase que define la estructura de CambioSGASTDN</param>
        /// <returns>Respuesta en formato HTTPResponseMessage, devuelve un OK si es correcto</returns>
        [HttpPost]
        public HttpResponseMessage Post(CambiaSolicitudAdmision cambiasolicitudadmision)
        {

            try
            {
                if (cambiasolicitudadmision == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto CambiaSolicitudAdmision no puede ir vacio");
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.UpdateCambiaSolicitudAdmision(cambiasolicitudadmision));
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
