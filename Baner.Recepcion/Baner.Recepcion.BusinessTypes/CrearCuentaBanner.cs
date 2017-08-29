using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
  public  class CrearCuentaBanner: BusinessTypeBase
    {
        public string id_Cta { get; set; }

        public string id_Oportunidad { get; set; }

        public string Nombre { get; set; }

        public string Segundo_Nombre { get; set; }

        public string ApellidoPaterno { get; set; }

        public string Apellido_Materno { get; set; }

        public CustomDate Fecha_Nacimiento { get; set; }

        public Correo Correo { get; set; }

        public string Campus { get; set; }

        public string Sexo { get; set; }

        public Telefono Telefono { get; set; }

        public string Id_Banner_Vinculante { get; set; }

        public string Periodo { get; set; }

        public string VPD { get; set; }

        public string Programa { get; set; }

    }
}
