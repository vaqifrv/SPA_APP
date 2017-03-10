using System;
using System.Collections.Generic;
using System.Linq;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public class RoleRepositoryFake : IRoleRepository
    {
        private static readonly List<Role> Roles = new List<Role>
        {
            new Role {Id = 1, Name = "admin"},
            new Role {Id = 2, Name = "simpleUser"},
            new Role {Id = 3, Name = "difficultUser"},
            new Role {Id = 4, Name = "superAdmin"},
            new Role {Id = 5, Name = "Quest"}
        };

        public ValueResponse<Role> Add(Role entity)
        {
            if (Roles.FirstOrDefault(r => r.Name == entity.Name) != null)
                return new ValueResponse<Role> { Value = entity };

            if (Roles.Count == 0)
                entity.Id = 1;
            else
                entity.Id = Roles.Select(r => r.Id).Last() + 1;
            Roles.Add(entity);
            return new ValueResponse<Role> { Value = entity };
        }

        public ValueResponse<Role> Update(Role entity)
        {
            var role = Roles.FirstOrDefault(r => r.Id == entity.Id);
            if (role == null) return new ValueResponse<Role>();
            role.Name = entity.Name;
            role.Rights = entity.Rights;
            role.Users = entity.Users;
            role.Description = entity.Description;
            return new ValueResponse<Role> { Value = role };
        }

        public ValueResponse<Role> Delete(int id)
        {
            try
            {
                var role = Roles.FirstOrDefault(r => r.Id == id);
                var result = new ValueResponse<Role>();
                if (role == null)
                    return result;
                Roles.Remove(role);
                result.Value = role;
                return result;
            }
            catch (Exception ex)
            {
                return new ValueResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }

        public ValueResponse<Role> FindBy(int id)
        {
            try
            {
                var role = Roles.FirstOrDefault(r => r.Id == id);
                var result = new ValueResponse<Role>();
                if (role != null)
                    result.Value = role;
                return result;
            }
            catch (Exception ex)
            {
                return new ValueResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }

        public ValueResponse<Role> FindByName(string name)
        {
            try
            {
                var resultRole = Roles.FirstOrDefault(r => r.Name == name);
                return new ValueResponse<Role> { Value = resultRole };
            }
            catch (Exception ex)
            {
                return new ValueResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }



        public ListResponse<Role> GetAllRoles()
        {
            try
            {
                return new ListResponse<Role> { List = Roles };
            }
            catch (Exception ex)
            {
                return new ListResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }

        public ListResponse<Role> GetAllRolesForUser(string userName)
        {
            throw new NotImplementedException();
        }


        public ListResponse<Role> GetRolesByNames(string[] roles)
        {
            try
            {
                var role = Roles.Where(r => roles.Contains(r.Name)).ToList();
                var result = new ListResponse<Role>();
                if (role.Count > 0)
                    result.List = role;
                return result;
            }
            catch (Exception ex)
            {
                return new ListResponse<Role> { Errors = new List<Exception> { ex } };
            }
        }
    }
}
