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
    public class srvAsociaBannerProspectoController : ApiController
    {
        private readonly ILogger _logger;
        private readonly ILeadProcessor _leadProcessor;
        public srvAsociaBannerProspectoController(ILogger logger, ILeadProcessor leadProcessor)
        {
            _logger = logger;
            _leadProcessor = leadProcessor;
        }

        [HttpPost]
        public HttpResponseMessage AsociaBannerProspecto(PreOportunidad preOportunidad)
        {
            try
            {
                if (preOportunidad == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El objeto Preoportunidad no puede ir vacia");

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _leadProcessor.ActualizaLead(preOportunidad));
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }

        }

    }
}
