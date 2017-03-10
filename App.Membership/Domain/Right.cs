using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Membership.Domain
{
    [JsonObject(IsReference = true)]
    public class Right : EntityBase
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Role> Roles { get; set; }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Right))
            {
                Right objToCompare = (Right)obj;
                return objToCompare.Id == this.Id && objToCompare.Name == this.Name;
            }
            else
                return false;
        }
        public virtual bool LogEnabled { get; set; }
    }
}
