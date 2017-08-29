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
    public class ValidarPreOportunidadController : ApiController
    {
        private readonly ILogger _logger;
        private readonly ILeadProcessor _leadProcessor;
        public ValidarPreOportunidadController(ILogger logger, ILeadProcessor leadProcessor)
        {
            _logger = logger;
            _leadProcessor = leadProcessor;
        }


        [HttpPost]
        public HttpResponseMessage Validar(PreOportunidad preoportunidad)
        {
            try
            {
                if (preoportunidad == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La informacion de la preoportunidad no puede ir vacia");
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _leadProcessor.ActualizaLead(preoportunidad));

                //return Request.CreateResponse(HttpStatusCode.OK);
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
