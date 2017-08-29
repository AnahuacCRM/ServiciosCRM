using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Baner.Recepcion.Services.Controllers
{
    public class AltaProspectoBannerController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IBannerProcessor _bannerProcessor;

        public AltaProspectoBannerController(ILogger logger, IBannerProcessor bannerProcessor)
        {
            _logger = logger;
            _bannerProcessor = bannerProcessor;
        }
        [HttpPost]
        public HttpResponseMessage AltaProespectoBanner(CrearCuentaBanner Prospecto)
        {

            try
            {
                if (Prospecto == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El prospecto no puede ir vacio");
                }
                // return Request.CreateResponse<RespuestaCoincidencia>(HttpStatusCode.OK, _bannerProcessor.ConsultarCoincidencias(Prospecto));
                return Request.CreateResponse<bool>(HttpStatusCode.OK, _bannerProcessor.CrearCuentaBanner(Prospecto));
            }

            catch (BannerException ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message);
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
            catch (Exception ex)
            {
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }
    }
}
