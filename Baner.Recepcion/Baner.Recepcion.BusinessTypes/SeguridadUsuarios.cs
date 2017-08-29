using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
    public static class SeguridadUsuarios
    {
        public static string Encriptar(string psw)
        {
            string datoencriptado = string.Empty;
           
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(psw);
            datoencriptado = Convert.ToBase64String(encryted);
            return datoencriptado;
        }

        public static string DesEncriptar(string datoencriptado)
        {
            string datoDesencriptado = string.Empty;
          
            byte[] decryted = Convert.FromBase64String(datoencriptado);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            datoDesencriptado = System.Text.Encoding.Unicode.GetString(decryted);
            return datoDesencriptado;
        }
    }
}
