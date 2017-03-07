using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Validation.Abstract
{
    /// <summary>
    ///     Validation from database
    /// </summary>
    public interface ICustomValidation
    {
        IList<BrokenRule> Validate(ValidationContext validationContext);
    }
}