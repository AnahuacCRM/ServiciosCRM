using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class OtorgaCredito:BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Cuenta es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id de Cuenta es de {1} caracteres")]
        public string id_Cuenta { get; set; }

        [Required(ErrorMessage = "Debe tener registrada información de otorgamiento de crédito")]
        public List<InformacionOtorgaCredito> InfoCreditos { get; set; }
    }
    public class InformacionOtorgaCredito : BusinessTypeBase
    {
        public InformacionOtorgaCredito() { }
        public InformacionOtorgaCredito(string descripcionCredito, string campusVPDI, string periodo, CustomDate fechaOtorgaCredito) {
            this.DescripcionCredito = descripcionCredito;
            this.CampusVPDI = campusVPDI;
            this.Periodo = periodo;
            this.FechaOtorgaCredito = fechaOtorgaCredito;
        }
        /// <summary>
        /// Descripción Crédito
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Descripción Crédito es requerido")]
        [MaxLength(30, ErrorMessage = "La longitud máxima de Descripción Crédito es de {1} caracteres")]
        public string DescripcionCredito { get; set; }

        /// <summary>
        /// Campus VPDI
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Campus VPDI es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de Campus VPDI es de {1} caracteres")]
        public string CampusVPDI { get; set; }

        /// <summary>
        /// Periodo
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima de Periodo es de {1} caracteres")]
        public string Periodo { get; set; }

        /// <summary>
        /// Fecha otorga cédito
        /// </summary>
        public CustomDate FechaOtorgaCredito { get; set; }

    }
}
