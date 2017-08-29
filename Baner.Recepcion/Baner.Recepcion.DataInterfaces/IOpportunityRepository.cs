using Baner.Recepcion.BusinessTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataInterfaces
{
    public interface IOpportunityRepository
    {
        List<Guid> RetrieveOportunidades(Guid Cuenta, Guid opportunityid);

        void DeactivateOportunity(string entityName, Guid OportunidadId);

        void CerrarOportunidadComoGanada(string entityName, Guid OportunidadId);

        List<Guid> RetrieveProspectOportunidades(Guid ProspectoId);

        List<Guid> RetrieveProspectOportunidades(string IdBanner);

        List<Guid> RetrieveOpportunityME(string idBanner);

        void ReopenOpportunity(string entityName, Guid OpportunityId);

        int RetrieveStatusById(Guid OpportunityId);

        Coincidencias ObtenerPreOportunidad(Guid LeadId);

        bool MarcarTransferido(List<Guid> OportunidadesId);
        /// <summary>
        /// Retorna true si la oportunidad está Cerrada de lo contrario false
        /// </summary>
        /// <param name="opId">Id Oportunidad</param>
        bool IsClosed(Guid opId);

        /// <summary>
        /// Retorna true si la oportunidad está Abierta de lo contrario false
        /// </summary>
        /// <param name="opId">Id Oportunidad</param>
        bool IsOpen(Guid opId);
    }
}
