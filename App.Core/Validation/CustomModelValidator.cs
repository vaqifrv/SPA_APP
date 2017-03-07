using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace App.Core.Validation
{
    /// <summary>
    ///     Gets list of error messages for entity that specified on its own class
    ///     via DataAnnotation attributes and CustomValidation method
    /// </summary>
    public static class CustomModelValidator
    {
        public static IList<BrokenRule> Validate(object obj)
        {
            var validationStage = "";
            var validatable = obj as AbstractEntity;
            if (validatable != null)
            {
                validationStage = validatable.ValidationStage;
            }
            return Validate(obj, "", validationStage);
        }

        public static IList<BrokenRule> Validate(object obj, string prefix, string validationStage)
        {
            var annotationValidationResults = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, context, annotationValidationResults, true);
            var validationResults = annotationValidationResults
                .Select(x => new BrokenRule(x.ErrorMessage, string.Join(",", x.MemberNames)))
                .ToList(); //annotationlardan gelen errorlar

            var validatable = obj as AbstractEntity;
            if (validatable != null)
            {
                validatable.ValidationStage = validationStage;
                validationResults.AddRange(validatable.Validate(context)); //CustomValidation metodundan gelen errorlar
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                foreach (var rule in validationResults)
                {
                    rule.Member = prefix + "." + rule.Member;
                }
            }

            return validationResults;
        }
    }
}