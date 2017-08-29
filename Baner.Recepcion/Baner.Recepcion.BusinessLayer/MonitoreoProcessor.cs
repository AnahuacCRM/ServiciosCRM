using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessLayer
{
    public class MonitoreoProcessor : IMonitoreoProcessor
    {
        private readonly ILogger _logger;
        private readonly IMonitoreoConexion _MonitoreoConexion;

        public MonitoreoProcessor(ILogger logger, IMonitoreoConexion monitoreoConexion)
        {
            _logger = logger;
            _MonitoreoConexion = monitoreoConexion;

        }


        public bool MonitoreoConexionCRM()
        {
            return _MonitoreoConexion.NotificaConexionCRM();

        }
    }
}
