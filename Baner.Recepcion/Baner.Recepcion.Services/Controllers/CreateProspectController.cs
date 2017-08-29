using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Exceptions;
using Baner.Recepcion.BusinessTypes.RespuestasServicio;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Baner.Recepcion.OperationalManagement.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Baner.Recepcion.Services.Controllers
{
    [Authorize]
    /// <summary>
    /// Controlador para nuevos prospectos
    /// </summary>
    public class CreateProspectController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;
        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prospectProcessor"></param>
        public CreateProspectController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            _logger = logger;
            _prospectProcessor = prospectProcessor;
        }

        /// <summary>
        /// Metodo para crear un nuevo prospecto en CRM
        /// </summary>
        /// <param name="prospect">Clase que define la estructura del prospecto</param>
        /// <returns>Respuesta en formato HTTPResponseMessage, Incluye el Guid que es el identificador de CRM</returns>
        [HttpPost]
        public HttpResponseMessage Create(NewProspect prospect)
        {


            try
            {
                if (prospect == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El prospecto no puede ir vacio");
                }

                var resultadocreacion = _prospectProcessor.Create(prospect);
                if (resultadocreacion.Warnings.Any())
                    return Request.CreateResponse<ResponseNewProspect>(HttpStatusCode.Accepted, resultadocreacion);
                else
                    return Request.CreateResponse<ResponseNewProspect>(HttpStatusCode.OK, resultadocreacion);
            }
            catch (BusinessLayerValidationException ex)
            {
                _logger.Error("BusinessLayerValidationException :" + ex.Message);
                string errores = "";
                foreach (var e in ex.Errors)
                {
                    errores += string.Format("{0}\n ", e);
                }
                _logger.Error("BusinessLayerValidationException Errores : " + errores);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, errores, ex);
            }
            catch (PickListException ex)
            {
                _logger.Error("PickListException  " + ex.Message);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (LookupException ex)
            {
                _logger.Error("LookupException  " + ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception   " + ex.Message);
                //_logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Build());
            }

        }

        //[HttpGet]
        //public string HelloWorld()
        //{
        //    return "Helloworld";
        //}
    }
}
