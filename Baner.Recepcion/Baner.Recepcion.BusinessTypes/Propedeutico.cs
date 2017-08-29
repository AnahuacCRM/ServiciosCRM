using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Propedeutico : BusinessTypeBase
    {
        public Propedeutico() { }
        public Propedeutico(string idOportunidad, string periodoPL, int solicitudAdmisionPL, string decisionAdmision, string vpdi, string campusAdmisionPL, string programaPL)
        {
            this.id_Oportunidad = idOportunidad;
            this.PeriodoPL = periodoPL;
            this.SolicitudAdmisionPL = solicitudAdmisionPL;
            this.DecisionAdmision = decisionAdmision;
            this.VPDI = vpdi;
            this.CampusAdmisionPL = campusAdmisionPL;
            this.ProgramaPL = programaPL;
        }
        /// <summary>
        /// Identificador del registro en Baner
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id_Oportunidad es de {1} caracteres")]
        public string id_Oportunidad { get; set; }
        //public string id_Cuenta { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El número de solicitud es requerido")]
        //public int? NumeroSolicitud { get; set; }

        ///// <summary>
        ///// Periodo 
        ///// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo es requerido")]
        //[MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        //public string Periodo { get; set; }

        /// <summary>
        /// Periodo PL
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo PL es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del Periodo PL es de {1} caracteres")]
        public string PeriodoPL { get; set; }

        /// <summary>
        /// Solicitud de Admisión a PL
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "La Solicitud de Admisión a PL es requerida")]
        //[MaxLength(1, ErrorMessage = "La longitud máxima de Solicitud de Admisión a PL es de {1} caracteres")]
        public int SolicitudAdmisionPL { get; set; }

        /// <summary>
        /// Decision de Admisión
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "La Decision de Admisión es requerida")]
        [MaxLength(2, ErrorMessage = "La longitud máxima de Decision de Admisión es de {1} caracteres")]
        public string DecisionAdmision { get; set; }

        /// <summary>
        /// VPDI
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        public string VPDI { get; set; }

        ///// <summary>
        ///// VPDIPL
        ///// </summary>
        ////[Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        //[MaxLength(3, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        //public string VPDIPL { get; set; }

        /// <summary>
        /// Campus de admisión a PL 
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El atributo{0} es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de Campus de admisión a PL  es de {1} caracteres")]
        public string CampusAdmisionPL { get; set; }

        /// <summary>
        /// Programa PL
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Programa PL es requerido")]
        [MaxLength(12, ErrorMessage = "La longitud máxima del Programa PL es de {1} caracteres")]
        public string ProgramaPL { get; set; }
    }
}
