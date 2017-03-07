using System;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Validation.Attributes
{
    public class RequiredDateTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateTime = (DateTime) value;
            if (dateTime == DateTime.MinValue)
                return false;
            return true;
        }
    }
}