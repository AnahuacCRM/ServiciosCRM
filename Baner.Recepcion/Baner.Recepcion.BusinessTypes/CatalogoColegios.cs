using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class CatalogoColegios:BusinessTypeBase
    {

        /// <summary>
        /// Clave Periodo
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Clave colegio es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima de Clave colegio  es de {1} caracteres")]
        public string Clave_Colegio { get; set; }

        /// <summary>
        /// Nombre colegio
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre colegio es requerido")]
        [MaxLength(30, ErrorMessage = "La longitud máxima de Nombre colegio  es de {1} caracteres")]
        public string Nombre_Colegio { get; set; }

        /// <summary>
        /// Calle
        /// </summary>
        [MaxLength(75, ErrorMessage = "La longitud máxima de Calle  es de {1} caracteres")]
        public string Calle { get; set; }

        /// <summary>
        /// Número
        /// </summary>     
        [MaxLength(75, ErrorMessage = "La longitud máxima de Número es de {1} caracteres")]
        public string Numero { get; set; }


        /// <summary>
        /// Colonia
        /// </summary>      
        [MaxLength(75, ErrorMessage = "La longitud máxima de Colonia es de {1} caracteres")]
        public string Colonia { get; set; }

        ///// <summary>
        ///// Ciudad
        ///// </summary>      
        //[MaxLength(50, ErrorMessage = "La longitud máxima de Ciudad es de {1} caracteres")]
        //public string Ciudad { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>      
        [MaxLength(5, ErrorMessage = "La longitud máxima de Municipio es de {1} caracteres")]
        public string Municipio { get; set; }

        /// <summary>
        /// Estado
        /// </summary>      
        [MaxLength(5, ErrorMessage = "La longitud máxima de Estado es de {1} caracteres")]
        public string Estado { get; set; }

        /// <summary>
        /// País
        /// </summary>      
        [MaxLength(5, ErrorMessage = "La longitud máxima de País es de {1} caracteres")]
        public string Pais { get; set; }

        /// <summary>
        /// Código Postal
        /// </summary>      
        [MaxLength(30, ErrorMessage = "La longitud máxima de Código Postal es de {1} caracteres")]
        public string Codigo_Postal { get; set; }
      

        /// <summary>
        /// Tipo Colegio
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo Colegio es requerido")]
        [MaxLength(2, ErrorMessage = "La longitud máxima de Tipo Colegio  es de {1} caracter")]
        public string Tipo_Colegio { get; set; }

        ///// <summary>
        ///// Lista de Contactos
        ///// </summary>
        //public List<Contactos> lstContactos { get; set; }

    }

    public class Contactos : BusinessTypeBase
    {

        /// <summary>
        /// VPDI
        /// </summary>      
        [MaxLength(6, ErrorMessage = "La longitud máxima de VPDI es de {1} caracteres")]
        public string VPDI { get; set; }

        /// <summary>
        /// Contacto
        /// </summary>      
        [MaxLength(230, ErrorMessage = "La longitud máxima de Contacto es de {1} caracteres")]
        public string Contacto { get; set; }

        /// <summary>
        /// Tipo Contacto
        /// </summary>      
        [MaxLength(4, ErrorMessage = "La longitud máxima de Tipo contacto es de {1} caracteres")]
        public string TipoContacto { get; set; }
    }
}
