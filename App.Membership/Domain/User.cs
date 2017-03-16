using System;
using System.Collections.Generic;

namespace App.Membership.Domain
{
    public class User : EntityBase
    {
        private string username;
        public virtual string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value.ToLower();
            }
        }
        public virtual string Password { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? DisabledDate { get; set; }
        public virtual IList<Role> Roles { get; set; }

        /* public override bool ChangePassword(string oldPassword, string newPassword)
                                 {
                                     Abstract.IUserRepository repository = null; 
                                     return repository.ChangePassword(this.UserName, oldPassword, newPassword);
                                 }*/

        
    }

}