﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class Coincidencias : BusinessTypeBase
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        [MaxLength(60, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        //public string psNombre { get; set; }
        public string Nombre { get; set; }

        [MaxLength(60, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        // public string psSegundoNombre { get; set; }
        public string Segundo_Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        [MaxLength(30, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Apellido_Paterno { get; set; }

        [MaxLength(29, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Apellido_Materno { get; set; }

        public CustomDate Fecha_Nacimiento { get; set; }

        [MaxLength(128, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Correo_Electrónico { get; set; }

        [MaxLength(1, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Sexo { get; set; }

        [MaxLength(6, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Codigo_Area { get; set; }

        [MaxLength(12, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string Numero_Telefonico { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El atributo {0} es requerido")]
        [MaxLength(6, ErrorMessage = "La longitud máxima del atributo {0} es de {1} caracteres")]
        public string VPD { get; set; }
    }
}
