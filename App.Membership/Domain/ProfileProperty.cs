using System;

namespace App.Membership.Domain
{
    public class ProfileProperty
    {
        public virtual string Name { get; set; }
        public virtual string Label { get; set; }
        public virtual string RegExp { get; set; }
        public virtual string ControlType { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual int OrderId { get; set; }

    }
}
