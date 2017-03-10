namespace App.Membership.Domain
{
    public class Profile
    {
        public virtual int ItemId { get; set; }
        public virtual User User { get; set; }
        public virtual ProfileProperty Property { get; set; }
        public virtual string PropertyValue { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var profile = obj as Profile;
            if (profile == null)
                return false;
            if (User.Username == profile.User.Username && Property.Name == profile.Property.Name)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (User.Username + "|" + Property.Name).GetHashCode();
        }  
    }
}
