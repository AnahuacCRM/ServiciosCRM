using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
      public class CambiaEmail:BusinessTypeBase
    {
        /// <summary>
        /// IdBanner
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        [MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        public string IdBanner { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Lista de Información de EMail es requerida")]
        public List<infoCambiaEmails> lstinfoCambiaEmails { get; set; }

    }
    public class infoCambiaEmails : BusinessTypeBase
    {
        /// <summary>
        /// VPDI
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        public string VPDI { get; set; }

        /// <summary>
        /// Tipo Correo Electronico 
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo Correo Electronico es requerido")]
        [MaxLength(4, ErrorMessage = "La longitud máxima de Tipo Correo Electronico  es de {1} caracteres")]
        public string TipoCorreoElectronico { get; set; }

        /// <summary>
        /// Secuencia Correo
        /// </summary>    
        [Required(AllowEmptyStrings = false, ErrorMessage = "Secuencia de Correo es requerido")]
        [MaxLength(18, ErrorMessage = "La longitud máxima de Secuencia de Correo es de {1} caracteres")]
        public string SecuenciaCorreo { get; set; }

        /// <summary>
        /// Tipo Operacion 
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo Operacion  es requerido")]
        [MaxLength(1, ErrorMessage = "La longitud máxima de Tipo Operacion  es de {1} caracter")]
        public string TipoOperacion { get; set; }      

        /// <summary>
        /// Correo Electronico
        /// </summary>  
        [Required(AllowEmptyStrings = false, ErrorMessage = "Correo Electronico es requerido")]
        [MaxLength(128, ErrorMessage = "La longitud máxima de  Correo Electronico es de {1} caracteres")]
        public string CorreoElectronico { get; set; }

        /// <summary>
        /// Correo Electronico IndPreferido
        /// </summary>       
        [MaxLength(1, ErrorMessage = "La longitud máxima de Correo Electronico IndPreferido es de {1} caracter")]
        public string CorreoElectronicoIndPreferido { get; set; }
    }
}
