using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using App.Core.Infrastructure;
using App.Core.Validation;
using App.Core.Validation.Abstract;
using Newtonsoft.Json;

namespace App.Core
{
    public class AbstractEntity
    {
        public virtual string ValidationStage { get; set; }

        public virtual IList<BrokenRule> Validate(ValidationContext validationContext)
        {
            //collecting DataAnnotation errors and custom errors from entities
            var result = CustomValidation(validationContext).ToList();

            //collecting errors from database and adding these errors to entity errors
            result.AddRange(CallExternalValidation(validationContext));

            //returns this list of errors
            return result;
        }

        /// <summary>
        ///     Validation from database
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        private IList<BrokenRule> CallExternalValidation(ValidationContext validationContext)
        {
            var validation = IoC.Current.GetObject<ICustomValidation>(validationContext.ObjectType.Name);
            return validation.Validate(validationContext);
        }

        /// <summary>
        ///     Custom entity validation, in order to validate entity with custom validations
        ///     you must override this on entity class
        /// </summary>
        /// <param name="validationContext">context to validate</param>
        /// <returns>List of broken rules</returns>
        public virtual IList<BrokenRule> CustomValidation(ValidationContext validationContext)
        {
            return new List<BrokenRule>();
        }

        public virtual object CustomClone()
        {
            var str = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new NHibernateContractResolver()
            });
            return JsonConvert.DeserializeObject(str, GetType());
        }
    }
}
