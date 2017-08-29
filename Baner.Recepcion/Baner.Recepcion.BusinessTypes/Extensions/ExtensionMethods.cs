using Baner.Recepcion.BusinessTypes.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baner.Recepcion.BusinessTypes.Extensions
{
    public static class ExtensionMethods
    {
        public static void Validate(this BusinessTypeBase target)
        {
            //
            // All input is evil. Essential validation goes here...
            //

            if (target == null)
            {
                throw new BusinessLayerException("No se proporciono una entidad para validar");
            }
            var context = new ValidationContext(target);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(target, context, results, true) == false)
            {
                var errors = results.Select(r => r.ErrorMessage);
                throw new BusinessLayerValidationException(errors);
            }

        }

        public static bool StringToBoolTransfom(this string target)
        {
            bool resultado = false;
            if (!string.IsNullOrWhiteSpace(target))
            {
                if (target.ToUpper() == "Y")
                    return true;
                else
                    return false;
            }
            return resultado;
        }

        public static bool StringToBoolTransformVive(this string target)
        {           
            bool resultado = true;
            if (!string.IsNullOrWhiteSpace(target))
            {
                if (target.ToUpper() == "Y")
                    return false;
                else
                    return true;
            }
            return resultado;
        }

        public static DateTime GetDate(this CustomDate target)
        {
            return new DateTime(target.Year, target.Month, target.Day);
        }


    }
}
