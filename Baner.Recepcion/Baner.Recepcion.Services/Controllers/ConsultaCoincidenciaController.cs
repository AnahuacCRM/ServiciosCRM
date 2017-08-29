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
using System.Web.Http.Cors;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;

namespace Baner.Recepcion.Services.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsultaCoincidenciaController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IBannerProcessor _bannerProcessor;
        public ConsultaCoincidenciaController(ILogger logger, IBannerProcessor bannerProcessor)
        {
            _logger = logger;
            _bannerProcessor = bannerProcessor;
        }

        [HttpGet]
        public HttpResponseMessage ObtenerPreOportunidad(string LeadId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LeadId))
                    throw new BusinessLayerException("Es requerida la pre oportunidad para validar el registro");

                return Request.CreateResponse<Coincidencias>(HttpStatusCode.OK, _bannerProcessor.ObtenerPreOportunidad(new Guid(LeadId)));
            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }

        [HttpPost]
        public HttpResponseMessage Consultar(Coincidencias prospecto)
        {
            try
            {
                if (prospecto == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El prospecto no puede ir vacio");
                }

                //throw new BannerException("Sin coincidencias");
                return Request.CreateResponse<List<RespuestaCoincidencia>>(HttpStatusCode.OK, _bannerProcessor.ConsultarCoincidencias(prospecto));
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
