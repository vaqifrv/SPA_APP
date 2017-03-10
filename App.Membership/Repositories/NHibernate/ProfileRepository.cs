using System;
using System.Collections.Generic;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Repositories.Abstract;

namespace App.Membership.Repositories.NHibernate
{
    public class ProfileRepository : RepositoryBase<Profile, int>, IProfileRepository
    {
        public ListResponse<Profile> GetUserProfile(string username)
        {
            try
            {
                var resultData = Session.QueryOver<Profile>().Where(p => p.User.Username == username).List<Profile>();
                var result = new ListResponse<Profile> {  };
                if (resultData != null)
                    result.List = resultData;
                return result;
            }
            catch (Exception ex)
            {
                return new ListResponse<Profile> {  Errors = new List<Exception> { ex } };
            }
        }

        public new ListResponse<Profile> FindAll(string propertyName, object propertyValue, bool ascending)
        {
            return base.FindAll(propertyName, propertyValue, ascending);
        }
    }
}
