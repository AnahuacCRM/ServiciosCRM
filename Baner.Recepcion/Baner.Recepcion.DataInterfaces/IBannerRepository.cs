using Baner.Recepcion.BusinessTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.DataInterfaces
{
    public interface IBannerRepository
    {
        List<RespuestaCoincidencia> ConsultarCoincidencias(Coincidencias coincidencia);

        bool CreateAccountBanner(CrearCuentaBanner createaccountbanner);


    }
}
