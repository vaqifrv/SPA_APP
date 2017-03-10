using System;
using System.Configuration;
using System.Linq;
using App.Membership.Repositories.Abstract;
using System.Web.Profile;
using App.Membership.Domain;

namespace App.Membership.Providers
{
    public class AgProfileProvider : ProfileProvider
    {
        IProfileRepository _profileRepo;
        IProfilePropertyRepository _propertyRepo;

        public AgProfileProvider(IProfileRepository profileRepo, IProfilePropertyRepository propertyRepo)
        {
            this._profileRepo = profileRepo;
            this._propertyRepo = propertyRepo;
        }

        public AgProfileProvider()
            : this(
                Repositories.RepositoryFactory.GetProfileRepository(),
                Repositories.RepositoryFactory.GetProfilePropertyRepository())
        {

        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection ppc)
        {
            var username = (string)context["UserName"];
            var isAuthenticated = (bool)context["IsAuthenticated"];
            var userProfiles = _profileRepo.GetUserProfile(username);
            var svc = new SettingsPropertyValueCollection();
            foreach (SettingsProperty prop in ppc)
            {
                var pv = new SettingsPropertyValue(prop);
                if (userProfiles.TotalItems > 0)
                {
                    var profile = userProfiles.List.FirstOrDefault(x => x.Property.Name == prop.Name);
                    if (profile != null)
                        pv.PropertyValue = profile.PropertyValue;
                }
                svc.Add(pv);
            }
            return svc;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection ppvc)
        {
            var username = (string)context["UserName"];
            var isAuthenticated = (bool)context["IsAuthenticated"];

            var userProfiles = _profileRepo.GetUserProfile(username);
            var profileProperties = _propertyRepo.GetAllProperties();

            foreach (SettingsPropertyValue pv in ppvc)
            {
                if (_propertyRepo.FindBy(pv.Name) == null)
                    _propertyRepo.Add(new ProfileProperty { Name = pv.Name });

                if (userProfiles.TotalItems > 0 && userProfiles.List.ToList().Exists(x => x.Property.Name == pv.Name))
                {
                    var firstOrDefault = userProfiles.List.FirstOrDefault(x => x.Property.Name == pv.Name);
                    if (firstOrDefault != null)
                    {
                        var itemId = firstOrDefault.ItemId;
                        _profileRepo.Delete(itemId);
                    }
                }

                _profileRepo.Add(new Profile
                {
                    Property = new ProfileProperty { Name = pv.Name },
                    PropertyValue = pv.PropertyValue.ToString(),
                    User = new User { Username = username }
                });

            }

        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
        }
    }
}
