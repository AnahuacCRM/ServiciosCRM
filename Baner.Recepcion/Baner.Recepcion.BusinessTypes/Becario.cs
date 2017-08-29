using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
   public class Becario: BusinessTypeBase
    {
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(60, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Nombre { get; set; }

        [MaxLength(60, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Segundo_Nombre { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(30, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Apellido_Paterno { get; set; }

        [MaxLength(29, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Apellido_Materno { get; set; }

        [MaxLength(6, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Telefono_Lada { get; set; }

        [MaxLength(12, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Telefono_Numero { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(128, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Correo_Electronico { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Nivel { get; set; }

        [MaxLength(12, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Codigo { get; set; }

        [MaxLength(180, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(3, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Campus { get; set; }

        [MaxLength(5, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Pais { get; set; }

        [MaxLength(3, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Estado { get; set; }

        [MaxLength(5, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Municipio { get; set; }

        public string Periodo { get; set; }

        public string Sexo { get; set; }

        public CustomDate Fecha_Nacimiento { get; set; }

        public string Grado { get; set; }

        public string  Colegio { get; set; }

        //[MaxLength(150, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        //public string OtroEstado { get; set; }


        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(25, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(200, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string SubOrigen { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(3, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string VPD { get; set; }
    }
}
