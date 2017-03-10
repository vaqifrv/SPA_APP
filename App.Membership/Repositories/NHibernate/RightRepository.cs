using System;
using System.Collections.Generic;
using System.Linq;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Repositories.Abstract;

namespace App.Membership.Repositories.NHibernate
{


    public class RightsComparer : IEqualityComparer<Right>
    {
        public bool Equals(Right x, Right y)
        {
            return (x.Id == y.Id);
        }

        public int GetHashCode(Right obj)
        {
            return obj.GetHashCode();
        }
    }

    public class RightRepository : RepositoryBase<Right, int>, IRightRepository
    {

        public ValueResponse<bool> CheckUserHasRight(string username, string rightName)
        {
            try
            {
                Right right = null;
                Role role = null;
                User user = null;
                var count = Session.QueryOver(() => right)
                    .JoinAlias(() => right.Roles, () => role)
                    .JoinAlias(() => role.Users, () => user)
                    .Where(() => user.Username == username)
                    .And(() => right.Name == rightName)
                    .ToRowCountQuery()
                    .FutureValue<int>();

                return new ValueResponse<bool> { Value = count.Value > 0 };
            }
            catch (Exception ex)
            {
                return new ValueResponse<bool> { Value = false, Errors = new List<Exception> { ex } };
            }

        }

        public ListResponse<Right> GetAllRights()
        {
            try
            {
                var rightResult = Session.QueryOver<Right>()
                    .List<Right>();
                return new ListResponse<Right> { List = rightResult };
            }
            catch (Exception ex)
            {
                return new ListResponse<Right> { Errors = new List<Exception> { ex } };
            }

        }



        public ValueResponse<Right> FindByName(string name)
        {
            try
            {
                var resultRight = Session.QueryOver<Right>()
                   .Where(x => x.Name == name).Take(1).SingleOrDefault<Right>();
                return new ValueResponse<Right> { Value = resultRight };
            }
            catch (Exception ex)
            {
                return new ValueResponse<Right> { Errors = new List<Exception> { ex } };
            }
        }

        public ListResponse<Right> GetRightsForUser(string username)
        {
            try
            {
                Right right = new Right();
                Role role = null;
                User user = null;

                var result = Session.QueryOver<Right>(() => right)
                                  .JoinAlias(() => right.Roles, () => role)
                                  .JoinAlias(() => role.Users, () => user)
                                  .Where(() => user.Username == username)
                                  .List<Right>()
                                  .Distinct<Right>(new RightsComparer())
                                  .ToList();
                return new ListResponse<Right> { List = result };
            }
            catch (Exception ex)
            {
                return new ListResponse<Right> { Errors = new List<Exception> { ex } };
            }

        }


    }
}
