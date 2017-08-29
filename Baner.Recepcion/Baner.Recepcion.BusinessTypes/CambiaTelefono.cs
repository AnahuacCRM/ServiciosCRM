using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class CambiaTelefono : BusinessTypeBase
    {
        /// <summary>
        /// IdBanner
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        [MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        public string IdBanner { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lista de Información de telefonos es requerida")]
        public List<ListaCambiaTelefonos> lstInformacionTelefonos { get; set; }

    }

        public class ListaCambiaTelefonos : BusinessTypeBase
        {
            /// <summary>
            /// VPDI
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "VPDI es requerido")]
            [MaxLength(6, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
            public string VPDI { get; set; }

            /// <summary>
            /// Tipo Telefono 
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo Telefono es requerido")]
            [MaxLength(4, ErrorMessage = "La longitud máxima de Tipo Telefono es de {1} caracteres")]
            public string TipoTelefono { get; set; }

            /// <summary>
            /// Secuencia Telefono
            /// </summary>    
            [Required(AllowEmptyStrings = false, ErrorMessage = "Secuencia Telefono es requerido")]
            [Range(0, 999999, ErrorMessage = "Secuencia Telefono máxima  es  de 999999")]        
            public int? SecuenciaTelefono { get; set; }

            /// <summary>
            /// Tipo Operacion 
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo Operacion  es requerido")]
            [MaxLength(1, ErrorMessage = "La longitud máxima de Tipo Operacion  es de {1} caracter")]
            public string TipoOperacion { get; set; }

            /// <summary>
            /// Teléfono Area
            /// </summary>       
            [MaxLength(6, ErrorMessage = "La longitud máxima de TelefonoArea es de {1} caracteres")]
            public string TelefonoArea { get; set; }

            /// <summary>
            /// Teléfono
            /// </summary>  
            [Required(AllowEmptyStrings = false, ErrorMessage = "Telefono es requerido")]
            [MaxLength(12, ErrorMessage = "La longitud máxima de Telefono es de {1} caracteres")]
            public string Telefono { get; set; }

            /// <summary>
            /// Telefono Preferido
            /// </summary>       
            [MaxLength(1, ErrorMessage = "La longitud máxima de Telefono Preferido es de {1} caracter")]
            public string TelefonoPreferido { get; set; }
        }
    
}
