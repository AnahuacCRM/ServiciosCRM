using Baner.Recepcion.BusinessInterfaces;
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
    public class MarcaTransferidoController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;

        public MarcaTransferidoController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        [HttpPost]
        public HttpResponseMessage Post(List<Guid> OportunidadesId)
        {
            try
            {
                if (OportunidadesId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No se proporcionaron las oportunidades a procesar");

                if(!OportunidadesId.Any())
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No se proporcionaron las oportunidades a procesar");

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _prospectProcessor.MarcarTransferido(OportunidadesId));

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }
        }
    }
}
