using System.ComponentModel.DataAnnotations;

namespace Baner.Recepcion.BusinessTypes
{
    public class Telefono : BusinessTypeBase
    {

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La lada es requerida")]
        [MaxLength(6, ErrorMessage = "La longitud máxima de la lada es de {1} caracteres")]
        public string LadaTelefono { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El número telefónico es requerido")]
        [MaxLength(12, ErrorMessage = "La longitud máxima del número telefónico es de {1} caracteres")]
        public string Telefono1 { get; set; }

        
        [MaxLength(4, ErrorMessage = "La longitud máxima del tipo de teléfono es de {1} caracteres")]
        public string TipoTelefono { get; set; }

        [MaxLength(1, ErrorMessage = "La longitud máxima del si es preferido el telefono es de {1} caracteres")]
        public string PreferidoTelefono { get; set; }

       


    }
}