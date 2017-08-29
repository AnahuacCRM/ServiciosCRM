using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baner.Recepcion.BusinessTypes;
using Baner.Recepcion.BusinessTypes.Extensions;
using Baner.Recepcion.OperationalManagement.Extensions;

namespace Baner.Recepcion.BusinessLayer
{
    public class LeadProcessor: ILeadProcessor
    {

        private readonly ILogger _logger;
        private readonly ILeadRepository _leadRepository;
        public LeadProcessor(ILogger logger, ILeadRepository leadRepository)
        {
            _logger = logger;
            _leadRepository = leadRepository;
        }

        public bool ActualizaLead(PreOportunidad preOportunidad)
        {
            try
            {
                preOportunidad.Validate();
               

                return _leadRepository.ActualizaLead(preOportunidad);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Build());
                throw ;
            }
        }
    }
}
