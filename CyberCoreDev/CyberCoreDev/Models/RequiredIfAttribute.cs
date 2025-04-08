using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string _propertyName;
        private object[] _targetValues;

        public RequiredIfAttribute(string propertyName, object targetValue, string errorMessage = "")
            : this(propertyName, new[] { targetValue }, errorMessage) { }

        public RequiredIfAttribute(string propertyName, object[] targetValues, string errorMessage = "")
        {
            _propertyName = propertyName;
            _targetValues = targetValues;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var propertyValue = instance.GetType().GetProperty(_propertyName)?.GetValue(instance);

            if (_targetValues.Any(v => propertyValue?.Equals(v) == true) && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}