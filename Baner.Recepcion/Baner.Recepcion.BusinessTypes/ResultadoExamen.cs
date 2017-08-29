using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class ResultadoExamen : BusinessTypeBase
    {
        public ResultadoExamen() { }
        public ResultadoExamen(string idCuenta, string vpdi, List<InformacionResultado> resultadosDeExamen) {
            this.id_Cuenta = idCuenta;
            this.VPDI = vpdi;
            this.ResultadosdeExamen = resultadosDeExamen;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string id_Cuenta { get; set; }

        /// <summary>
        /// VPDI
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        [MaxLength(4, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        public string VPDI { get; set; }

        [Required(ErrorMessage = "Debe contar con al menos un resultado de examen para poder actualizarse")]
        //[MinLength(1, ErrorMessage = "Debe contar con al menos un resultado de examen para poder actualizarse")]
        public List<InformacionResultado> ResultadosdeExamen { get; set; }
    }

    public class InformacionResultado : BusinessTypeBase
    {
        public InformacionResultado() { }
        public InformacionResultado(string codigoExamen, CustomDate fechaResultado) {
            CodigoExamen = codigoExamen;
            FechaResultado = fechaResultado;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        [MaxLength(4, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string CodigoExamen { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        //[MaxLength(1, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracter")]
        //public string BanderaScore { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido")]
        public CustomDate FechaResultado { get; set; }
    }
}
