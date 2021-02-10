using System.ComponentModel.DataAnnotations;

namespace SGE.Servicio.Comun
{
    public class ValidateEmail : RegularExpressionAttribute
    {
        private const string DefaultErrorMessage = "Ingrese Correctamente el Email";

        public ValidateEmail() :
            base("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$")
        { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(DefaultErrorMessage, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!base.IsValid(value))
                {
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }
}
