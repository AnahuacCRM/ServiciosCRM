using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Examinado : BusinessTypeBase
    {
        public Examinado() { }
        public Examinado(string idOportunidad, string campus, string programa, string promedioPreparatoria, string tipoAlumno,string PuntuacionSobre)
        {
            this.id_Oportunidad = idOportunidad;
            this.Campus = campus;
            this.Programa = programa;
            this.PromedioPreparatoria = promedioPreparatoria;
            this.TipoAlumno = tipoAlumno;
            this.PuntualizacionSobresaliente = PuntuacionSobre;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string id_Oportunidad { get; set; }

        /// <summary>
        /// Identificador del registro en Baner
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        //[MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        //public string IdBanner { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El número de solicitud es requerido")]
        //public int? NumeroSolicitud { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Campus { get; set; }

        /// <summary>
        /// Programa
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Programa es requerido")]
        [MaxLength(12, ErrorMessage = "La longitud máxima del programa es de {1} caracteres")]
        public string Programa { get; set; }

        /// <summary>
        /// Promedio Preparatoria
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Promedio de preparatoria es requerido")]
        [MaxLength(24, ErrorMessage = "La longitud máxima del Promedio Preparatoria es de {1} caracteres")]
        public string PromedioPreparatoria { get; set; }

        /// <summary>
        /// Periodo
        /// </summary>
        //[MaxLength(6, ErrorMessage = "La longitud máxima del Periodo es de {1} caracteres")]
        //public string Periodo { get; set; }

        /// <summary>
        /// Tipo Alumno
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Tipo de alumno es requerido")]
        [MaxLength(1, ErrorMessage = "La longitud máxima del Tipo alumno es de {1} caracteres")]
        public string TipoAlumno { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(1, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        [RegularExpression("^(?:Y|N)$", ErrorMessage = "El atributo {0} solo acepta los valores Y ó N.")]
        public string PuntualizacionSobresaliente { get; set; }
        /// <summary>
        /// VPDI
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        //[MaxLength(4, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        //public string VPDI { get; set; }
    }
}
