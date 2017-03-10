namespace App.Membership.Services.NetworkAuthorize.Model
{
    public class NetworkAccessRule
    {
        public string Access { get; set; }
        public string Role { get; set; }
        public string Network { get; set; }
        public bool HasAccess
        {
            get
            {
                return (Access == "allow");
            }
            set
            {
                if (value)
                    this.Access = "allow";
                else
                    this.Access = "deny";
            }
        }


        public NetworkAccessRule()
        {
        }

        public NetworkAccessRule(Configuration.NetworkAuthorizeElement element)
        {
            this.Access = element.HasAccess ? "allow" : "deny";
            this.Role = element.Role;
            this.Network = element.Network;
        }
    }
}
