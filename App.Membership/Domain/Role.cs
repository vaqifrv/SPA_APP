﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Membership.Domain
{
    public class Role : EntityBase, IValidatableObject
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Right> Rights { get; set; }
        public virtual IList<User> Users { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Role))
            {
                Role objToCompare = (Role)obj;
                return objToCompare.Id == this.Id && objToCompare.Name == this.Name;
            }
            else
                return false;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //List<ValidationResult> errors=new List<ValidationResult>();
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Name.Trim()))
                yield return new ValidationResult("Rolun adı boş ola bilmez", new string[] { "Name" });



        }
    }
}