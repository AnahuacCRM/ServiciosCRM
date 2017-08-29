using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Direccion : BusinessTypeBase
    {
 

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La calle es requerida")]
        [MaxLength(75, ErrorMessage = "La longitud máxima de calle es de {1} caracteres")]
        public string Calle { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El numero exterior es requerido")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del numero exterior es de {1} caracteres")]
        public string Numero { get; set; }

        //public string NumeroInterior { get; set; }
        
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El Codigo postal es requerido")]
        [MaxLength(30, ErrorMessage = "La longitud máxima del codigo postal es de {1} caracteres")]
        public string CodigoPostal { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La colonia es requerido")]
        [MaxLength(75, ErrorMessage = "La longitud máxima de la colonia es de {1} caracteres")]
        public string Colonia { get; set; }

       // [Required(AllowEmptyStrings = false, ErrorMessage = "La delegación o municipio es requerido")]
        [MaxLength(5, ErrorMessage = "La longitud máxima de la delegación o municipio es de {1} caracteres")]
        public string DelegacionMunicipioId { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La ciudads es requerido")]
        //[MaxLength(50, ErrorMessage = "La longitud máxima de la ciudad es de {1} caracteres")]
        //public string Ciudad { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El estado es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima del estado es de {1} caracteres")]
        public string Estado { get; set; }

       // [Required(AllowEmptyStrings = false, ErrorMessage = "El país es requerido")]
        [MaxLength(5, ErrorMessage = "La longitud máxima del país es de {1} caracteres")]
        public string PaisId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} de la direccion del solicitante es requerido ")]
        [MaxLength(2, ErrorMessage = "La longitud máxima del tipo de dirección es de {1} caracteres")]
        public string TipoDireccionId { get; set; }

       
    }
}
