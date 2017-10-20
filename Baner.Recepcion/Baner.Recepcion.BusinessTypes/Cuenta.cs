using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    /// <summary>
    /// Servicio 2 CRM.
    /// </summary>
    public class Cuenta: BusinessTypeBase
    {
        //public Guid? IdCRM { get; set; }

        //public Guid? IdOpportunity { get; set; }


        /// <summary>
        /// Identificador del registro en Baner
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de banner es requerido")]
        [MaxLength(9, ErrorMessage = "La longitud máxima del id de banner es de {1} caracteres")]
        public string IdBanner { get; set; }

        /// <summary>
        /// Nombre del solicitante
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Nombre es requerido")]
        [MaxLength(60, ErrorMessage = "La longitud máxima del nombre es de {1} caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Segundo Nombre del solicitante
        /// </summary>
        [MaxLength(60, ErrorMessage = "La longitud máxima del segundo nombre es de {1} caracteres")]
        public string Segundo_Nombre { get; set; }

        //La transformacion de apellidos es de 30 caracteres cada uno maximo
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido paterno es requerido")]
        [MaxLength(60, ErrorMessage = "La longitud máxima de los apellidos es de {1} caracteres")]
        public string Apellidos { get; set; }


        //[MaxLength(30, ErrorMessage = "La longitud máxima del apellido materno es de {1} caracteres")]
        //public string ApellidoMaterno { get; set; }


        public CustomDate Fecha_de_nacimiento { get; set; }


        public List<Telefono> Telefonos { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El CampusVPD es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del CampusVPD es de {1} caracteres")]
        public string CampusVPD { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Campus es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del Campus es de {1} caracteres")]
        public string Campus { get; set; }


        public int? Numero_Solicitud { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "El estatus de la solicitud es requerido")]
        [MaxLength(2, ErrorMessage = "La longitud máxima del estatus de la solicitud es de {1} caracteres")]
        public string Estatus_Solicitud { get; set; }

        [MaxLength(1, ErrorMessage = "La longitud máxima del sexo es de {1} caracter")]
        public string Sexo { get; set; }


        [MaxLength(2, ErrorMessage = "La longitud máxima de la nacionalidad es de {1} caracteres")]
        public string Nacionalidad { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El colegio de procedencia es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del colegio de procedencia es de {1} caracteres")]
        public string Colegio_Procedencia { get; set; } //Account Number

        [MaxLength(24, ErrorMessage = "La longitud máxima del promedio es de {1} caracteres")]
        public string Promedio { get; set; }

        //[Required(ErrorMessage = "Se debe proporcionar la direccion predeterminada")]
        public List<Direccion> Direcciones { get; set; }

        public List<Correo> Correos { get; set; }
        //6n
        [Required(AllowEmptyStrings = false, ErrorMessage = "El periodo de ingreso es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del periodo de ingreso es de {1} caracteres")]
        public string PeriodoId { get; set; }
        // 6s
        [Required(AllowEmptyStrings = false, ErrorMessage = "El programa es requerido")]
        [MaxLength(12, ErrorMessage = "La longitud máxima del programa es de {1} caracteres")]
        public string Programa1 { get; set; }
        //2s
        [Required(AllowEmptyStrings = false, ErrorMessage = "La escuela es requerido")]
        [MaxLength(2, ErrorMessage = "La escuela es de {1} caracteres")]
        public string Escuela { get; set; }
        //1s
        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo de alumno es requerido")]
        [MaxLength(1, ErrorMessage = "La longitud máxima del tipo de alumno es de {1} caracter")]
        public string Codigo_Tipo_Alumno { get; set; }
        //6s
        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo admision es requerido")]
        [MaxLength(2, ErrorMessage = "La longitud máxima del tipo admision es de {1} caracteres")]
        public string Codigo_Tipo_admision { get; set; }

       

        [MaxLength(2, ErrorMessage = "La longitud máxima de Religion Id es de {1} caracteres")]
        public string ReligionId { get; set; }



        [MaxLength(1, ErrorMessage = "La longitud máxima del codigo del estado civil es de {1} caracter")]
        public string EstadoCivil { get; set; }


      


        public List<PadreoTutor> PadreoTutor { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(2, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string Nivel { get; set; }
        
    }
}
