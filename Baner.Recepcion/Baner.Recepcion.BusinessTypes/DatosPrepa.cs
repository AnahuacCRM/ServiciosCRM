using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class DatosPrepa: BusinessTypeBase
    {


        /// <summary>
        /// Identificador del registro en Baner
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de cuenta es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        public string id_Cuenta { get; set; }
        ///// <summary>
        ///// Identificador del registro en Baner
        ///// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        //[MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        //public string IdBanner { get; set; }

        /// <summary>
        /// Preparatoria
        /// </summary>
       
        [MaxLength(6, ErrorMessage = "La longitud máxima de Preparatoria es de {1} caracteres")]
        public string Preparatoria { get; set; }

        /// <summary>
        /// Preparatoria
        /// </summary>
        [MaxLength(24, ErrorMessage = "La longitud máxima de Promedio Preparatoria es de {1} caracteres")]
        public string PromedioPreparatoria { get; set; }

        /// <summary>
        /// VPDI
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        [MaxLength(4, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        public string VPDI { get; set; }

    }
}
