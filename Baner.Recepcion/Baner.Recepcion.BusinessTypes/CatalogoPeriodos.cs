using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class CatalogoPeriodos : BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Periodo es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima de Periodo  es de {1} caracteres")]
        public string Periodo { get; set; }

        /// <summary>
        /// Descripción periodo
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Descripcion periodo es requerido")]
        [MaxLength(30, ErrorMessage = "La longitud máxima de Descripcion periodo  es de {1} caracteres")]
        public string Descripcion { get; set; }


        /// <summary>
        /// Tipo Periodo
        /// </summary>

        // [MaxLength(3, ErrorMessage = "La longitud máxima del tipo de periodo  es de {1} caracteres")]
        public string Tipo_Periodo { get; set; }

        [MaxLength(4, ErrorMessage = "La longitud máxima de Periodo  es de {1} caracteres")]
        public string Ano_academico { get; set; }

        /// <summary>
        /// Fecha inicio periodo
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Fecha inicio periodo es requerido")]
        public CustomDate Fecha_de_Inicio_Periodo { get; set; }
        /// <summary>
        /// Fecha fin periodo
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Fecha fin periodo es requerido")]
        public CustomDate Fecha_de_Fin_Periodo { get; set; }
        /// <summary>
        /// Fecha inicio alojamiento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Fecha inicio alojamiento es requerido")]
        public CustomDate Fecha_Inicio_Alojamiento { get; set; }
        /// <summary>
        /// Fecha fin alojamiento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Fecha fin alojamiento periodo es requerido")]
        public CustomDate Fecha_Fin_Alojamiento { get; set; }


    }
}
