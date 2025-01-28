using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.CustomValidation
{
    public class DateTimeValidatiion:ValidationAttribute
    {


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           if (value is not null)
            {
                var date = (DateTime)value;
                if (date.Year < 2000)
                {
                    return new ValidationResult("[Should not be Less than Jan 01, 2000");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null;
            
        }

    }


}
