using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class CambiosTipoAdmision : BusinessTypeBase
    {
        public CambiosTipoAdmision() { }
        public CambiosTipoAdmision(string idOportunidad, string tipoAlumno, string tipoAdmision)
        {
            this.Id_Oportunidad = idOportunidad;
            this.TipoAlumno = tipoAlumno;
            this.TipoAdmision = tipoAdmision;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Id_Oportunidad { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //[MaxLength(9, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        //public string IdBanner { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(1, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string TipoAlumno { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string TipoAdmision { get; set; }


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

        /// <summary>
        /// Periodo 
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Periodo es requerido")]
        //[MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        //public string Periodo { get; set; }
    }
}
