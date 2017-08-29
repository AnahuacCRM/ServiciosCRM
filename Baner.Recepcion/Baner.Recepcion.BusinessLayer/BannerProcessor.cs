using Baner.Recepcion.BusinessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.OperationalManagement.Extensions;
using Baner.Recepcion.BusinessTypes.Extensions;

namespace Baner.Recepcion.BusinessLayer
{
    public class BannerProcessor : IBannerProcessor
    {
        private readonly ILogger _logger;
        private readonly IBannerRepository _bannerRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public BannerProcessor(ILogger logger, IBannerRepository bannerRepository, IOpportunityRepository opportunityRepository)
        {
            _logger = logger;
            _bannerRepository = bannerRepository;
            _opportunityRepository = opportunityRepository;

        }

        public List<RespuestaCoincidencia> ConsultarCoincidencias(Coincidencias coincidencia)
        {
            try
            {
                coincidencia.Validate();
                if (coincidencia.Fecha_Nacimiento!=null)
                {
                    coincidencia.Fecha_Nacimiento.Validate();
                }

                return _bannerRepository.ConsultarCoincidencias(coincidencia);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Build());
                throw ;
            }
        }

        public Coincidencias ObtenerPreOportunidad(Guid LeadId)
        {
            try
            {
                return _opportunityRepository.ObtenerPreOportunidad(LeadId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw ;
            }
        }

        public bool CrearCuentaBanner(CrearCuentaBanner crearcuentabanner)
        {
            bool todobien = false;
            try
            {
                return _bannerRepository.CreateAccountBanner(crearcuentabanner);
                todobien = true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Build());
            }
            return todobien;
        }
    }
}
