using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class PadreoTutor : BusinessTypeBase
    {

     

        [MaxLength(1, ErrorMessage = "La longitud máxima del parentesco es de {1} caracter")]
        public string Parentesco { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del padre o tutor es requerido")]
        [MaxLength(60, ErrorMessage = "La longitud máxima del nombre del padre o tutor es de {1} caracteres")]
        public string FirstName { get; set; }

        
        [MaxLength(60, ErrorMessage = "La longitud máxima del segundo nombre del padre o tutor es de {1} caracteres")]
        public string MiddleName { get; set; }
        //[MaxLength(30, ErrorMessage = "La longitud máxima del segundo nombre es de {1} caracteres")]
        //public string MiddleName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Los apellidos del padre o tutor son requerido")]
        [MaxLength(60, ErrorMessage = "La longitud máxima de los apellidos del padre o tutor es de {1} caracteres")]
        public string LastName { get; set; }

        [MaxLength(1, ErrorMessage = "La longitud máxima de si vive el padre o tutor es de {1} caracter")]
        public string Vive { get; set; }

        
        //[MaxLength(2, ErrorMessage = "La longitud máxima del tipo de direccion es de {1} caracteres")]
        //public string TipoDireccion { get; set; }

        public Direccion Direcciones { get; set; }


    }
}
