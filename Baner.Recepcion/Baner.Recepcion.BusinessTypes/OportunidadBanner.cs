using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class OportunidadBanner: BusinessTypeBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo id_oportunidad_crm es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima de la id_cuenta es de {1} caracteres")]
        public string id_oportunidad_crm { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo id_cuenta_crm es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud máxima de la id_cuenta_crm es de {1} caracteres")]
        public string id_cuenta_crm { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo IDBANNER es requerido")]
        [MaxLength(9, ErrorMessage = "La longitud máxima de la IDBANNER es de {1} caracteres")]
        public string IDBANNER { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo Periodo es requerido")]
        [MaxLength(4, ErrorMessage = "La longitud máxima de la Periodo es de {1} caracteres")]
        public string Periodo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo VPDI es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima de la VPDI es de {1} caracteres")]
        public string VPDI { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Atributo Programa es requerido")]
        [MaxLength(2, ErrorMessage = "La longitud máxima de la Programa es de {1} caracteres")]
        public string Programa { get; set; }

        [MaxLength(2, ErrorMessage = "La longitud máxima de la Número_solictud_de_admisión es de {1} caracteres")]
        public string Número_solictud_de_admisión { get; set; }




    }
}
