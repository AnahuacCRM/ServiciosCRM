using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class CambiaSolicitudAdmision :BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        public Guid? Id_Oportunidad { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        //[MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        //public string IdBanner { get; set; }

        /// <summary>
        /// Periodo 
        ///// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo es requerido")]
        //[MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        //public string Periodo { get; set; }

        /// <summary>
        /// Programa
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Programa es requerido")]
        [MaxLength(12, ErrorMessage = "La longitud máxima del Programa es de {1} caracteres")]
        public string Programa { get; set; }

        /// <summary>
        /// Campus
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Campus es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de Campus es de {1} caracteres")]
        public string Campus { get; set; }


        /// <summary>
        /// Escuela
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Escuela es requerido")]
        [MaxLength(2, ErrorMessage = "La longitud máxima de Escuela es de {1} caracteres")]
        public string Escuela { get; set; }

        /// <summary>
        /// VPDI
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        //[MaxLength(4, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        //public string VPDI { get; set; }

        /// <summary>
        /// NumeroSolicitud
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El número de solicitud es requerido")]
        //public int? NumeroSolicitud { get; set; }
    }
}
