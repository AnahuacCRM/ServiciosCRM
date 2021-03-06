﻿using Baner.Recepcion.BusinessInterfaces;
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
    /// <summary>
    /// Servicio 2 CRM.
    /// </summary>
    [Authorize]
    //Controlador para crear una cuenta nueva

    public class CreateAccountController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IProspectProcessor _prospectProcessor;

        public CreateAccountController(ILogger logger, IProspectProcessor prospectProcessor)
        {
            this._logger = logger;
            this._prospectProcessor = prospectProcessor;
        }

        [HttpPost]
        public HttpResponseMessage Create(Cuenta Cuenta)
        {
            
            try
            {
                
                if (Cuenta == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El prospecto no puede ir vacio");
                }
                //Mandamos ejecutar el proceso de registro en crm
                var resultadocreacion = _prospectProcessor.Create(Cuenta);
               
                if (resultadocreacion.Warnings.Any())
                    return this.Request.CreateResponse<ResponseNewProspect>(HttpStatusCode.Accepted, resultadocreacion);
                else
                    return this.Request.CreateResponse<ResponseNewProspect>(HttpStatusCode.OK, resultadocreacion);
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
    }


}
