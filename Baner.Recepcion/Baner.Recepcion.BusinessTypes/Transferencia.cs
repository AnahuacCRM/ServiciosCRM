using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public  class Transferencia:BusinessTypeBase
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del id de id_Oportunidad es de {1} caracteres")]
        public string id_Oportunidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Periodo es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        public string Periodo { get; set; }

        /// <summary>
        /// Campus Origen
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Campus Origen es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de Campus Origen es de {1} caracteres")]
        public string Campus_Origen { get; set; }

        /// <summary>
        /// Campus Destino
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Campus Destino es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de Campus Destino es de {1} caracteres")]
        public string Campus_Destino { get; set; }
    }
}
