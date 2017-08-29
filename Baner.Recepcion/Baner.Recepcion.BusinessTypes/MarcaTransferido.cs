using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes
{
   public class MarcaTransferido
    {
        public MarcaTransferido() { }

        public MarcaTransferido(string idOportunidad, string campus)
        {
            this.id_Oportunidad = idOportunidad;
            this.Campus_Origen = campus;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Identificador de Oportunidad es requerido")]
        [MaxLength(36, ErrorMessage = "La longitud maxima del atributo {0} es de {1} caracteres.")]
        public string id_Oportunidad { get; set; }

        /// <summary>
        /// Campus
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Campus es requerido")]
        [MaxLength(3, ErrorMessage = "La longitud máxima de Campus es de {1} caracteres")]
        public string Campus_Origen { get; set; }
    }
}
