using System;
using System.Collections.Generic;
using System.Linq;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Repositories.Abstract;

namespace App.Membership.Repositories.NHibernate
{
    public class ProfilePropertyRepository : RepositoryBase<ProfileProperty, string>, IProfilePropertyRepository
    {
        public ListResponse<ProfileProperty> GetAllProperties()
        {
            try
            {
                var resultProfile = Session.QueryOver<ProfileProperty>()
                .OrderBy(property => property.OrderId).Asc
                .OrderBy(property => property.CreatedDate).Asc
                .List<ProfileProperty>();
                return new ListResponse<ProfileProperty> { List = resultProfile?.ToList() };
            }
            catch (Exception ex)
            {
                return new ListResponse<ProfileProperty>
                {
                    Errors = new List<Exception> { ex }
                };
            }

        }
    }
}
