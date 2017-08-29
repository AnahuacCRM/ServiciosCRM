using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class ExaminadoPI : BusinessTypeBase
    {
        public ExaminadoPI() { }
        public ExaminadoPI(string idOportunidad, string decisionAdmision) {
            this.id_Oportunidad = idOportunidad;
            this.DecisionAdmision = decisionAdmision;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id de oportunidad es de {1} caracteres")]
        public string id_Oportunidad { get; set; }
        //public Guid? OportunidadIdCRM { get; set; }

        ///// <summary>
        ///// Identificador del registro en Baner
        ///// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        //[MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        //public string IdBanner { get; set; }

        ///// <summary>
        ///// Número de Solicitud
        ///// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El número de solicitud es requerido")]
        //public int? NumeroSolicitud { get; set; }

        /// <summary>
        /// Decision de Admisión
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "La Decision de Admisión es requerida")]
        [MaxLength(2, ErrorMessage = "La longitud máxima de Decision de Admisión es de {1} caracteres")]
        public string DecisionAdmision { get; set; }

        //    /// <summary>
        //    /// VPDI
        //    /// </summary>
        //    [Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        //    [MaxLength(4, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        //    public string VPDI { get; set; }

        //    /// <summary>
        //    /// Periodo 
        //    /// </summary>
        //    [Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo es requerido")]
        //    [MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        //    public string Periodo { get; set; }
    }
}
