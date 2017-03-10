using System;
using System.Collections.Generic;
using System.Linq;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Repositories.Abstract;

namespace App.Membership.Repositories.NHibernate
{
    public class RoleRepository : RepositoryBase<Role, int>, IRoleRepository
    {


        public ValueResponse<Role> FindByName(string name)
        {
            try
            {
                var resultRole = Session.QueryOver<Role>()
                    .Where(x => x.Name == name)
                    .SingleOrDefault();
                return new ValueResponse<Role> { Value = resultRole };
            }
            catch (Exception ex)
            {
                return new ValueResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }


        public ListResponse<Role> GetRolesByNames(string[] roles)
        {
            try
            {
                var roleList = Session.QueryOver<Role>()
                    .AndRestrictionOn(x => x.Name)
                    .IsIn(roles)
                    .List();
                return new ListResponse<Role> { List = roleList.Count > 0 ? roleList : null };
            }
            catch (Exception ex)
            {
                return new ListResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }

        public ListResponse<Role> GetAllRolesForUser(string userName)
        {
            try
            {
                var user = Session.QueryOver<User>().Where(x => x.Username == userName).SingleOrDefault();
                ;
                return new ListResponse<Role> { List = user?.Roles?.ToList() };
            }
            catch (Exception ex)
            {
                return new ListResponse<Role>
                {
                    Errors = new List<Exception> { ex }
                };
            }
        }

        public ListResponse<Role> GetAllRoles()
        {
            try
            {
                var roleList = Session.QueryOver<Role>()
                     .List<Role>();
                return new ListResponse<Role> { List = roleList?.ToList() };
            }
            catch (Exception ex)
            {
                return new ListResponse<Role>
                {
                    Errors = new List<Exception> { ex }
                };
            }

        }
    }
}
