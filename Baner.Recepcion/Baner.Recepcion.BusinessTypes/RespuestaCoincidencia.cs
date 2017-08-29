using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public class RespuestaCoincidencia
    {
        public string IdBanner { get; set; }
        public string IdCRM { get; set; }
        public string Nombre { get; set; }
        public CustomDate fechaNacimiento { get; set; }
        public string Telefono { get; set; }

        public DateTime? fecha { get; set; }
        public string Sexo { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string ContieneHA { get; set; }


    }
}
