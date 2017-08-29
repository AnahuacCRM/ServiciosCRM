using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class SolicitaBeca : BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de id_Cuenta es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id_Cuenta es de {1} caracteres")]
        public string id_Cuenta { get; set; }

        [Required(ErrorMessage = "Debe tener registrada información de solicitud de beca")]
        public List<InformacionBeca> SolicitudBecas { get; set; }
    }
    public class InformacionBeca : BusinessTypeBase
    {
        /// <summary>
        /// Tipo de beca
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Tipo de beca es requerido")]
        [MaxLength(8, ErrorMessage = "La longitud máxima de Tipo de beca es de {1} caracteres")]
        public string TipoBeca { get; set; }

        /// <summary>
        /// Descripción Beca
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Descripción Beca es requerido")]
        [MaxLength(30, ErrorMessage = "La longitud máxima de Descripción Beca es de {1} caracteres")]
        public string DescripcionBeca { get; set; }

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
        /// Fecha solicitud beca
        /// </summary>
        public CustomDate FechaSolicitudBeca { get; set; }

    }
}
