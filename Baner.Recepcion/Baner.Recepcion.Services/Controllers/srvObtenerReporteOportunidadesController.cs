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
    public class srvObtenerReporteOportunidadesController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectoProcessor;
        public srvObtenerReporteOportunidadesController(ILogger plogger, IProspectProcessor pibannerProcessor)
        {
            _logger = plogger;
            _prospectoProcessor = pibannerProcessor;

        }


        [HttpGet]
        public HttpResponseMessage ObtenerReporte()
        {
            try
            {
               
                // return Request.CreateResponse<RespuestaCoincidencia>(HttpStatusCode.OK, _bannerProcessor.ConsultarCoincidencias(Prospecto));
                return Request.CreateResponse<List<RepOportunidades>>(HttpStatusCode.OK, _prospectoProcessor.ObtenerDatosReport());
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
