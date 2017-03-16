using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using App.Membership.Domain;
using App.Membership.Repositories.Abstract;

namespace App.Membership.Providers
{
    public class AgRoleProvider : RoleProvider
    {
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private IRightRepository _rightRepository;


        public AgRoleProvider(IUserRepository userRepository, IRoleRepository roleRepository,
            IRightRepository rightRepository)
        {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._rightRepository = rightRepository;
        }

        public AgRoleProvider()
            : this(
                Repositories.RepositoryFactory.GetUserRepository(), Repositories.RepositoryFactory.GetRoleRepository(),
                Repositories.RepositoryFactory.GetRightRepository())
        {
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var roles = _roleRepository.GetRolesByNames(roleNames);
            foreach (var username in usernames)
            {
                var user = _userRepository.FindBy(username);
                if (user.Value == null) continue;
                if (user.Value.Roles == null)
                {
                    user.Value.Roles = new List<Role>();
                }
                if (roles?.List != null)
                {
                    foreach (var role in roles.List)
                    {
                        if (user.Value.Roles.Contains(role)) continue;
                        user.Value.Roles.Add(role);
                    }
                }
                _userRepository.Update(user.Value);
            }
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void CreateRole(string roleName)
        {
            CreateRole(new Role
            {
                Name = roleName
            });
        }

        public void CreateRole(Role role)
        {
            var rolename = role.Name;
            if (string.IsNullOrEmpty(rolename))
                throw new ProviderException("Role name cannot be empty or null.");
            if (rolename.Contains(","))
                throw new ArgumentException("Role names cannot contain commas.");
            if (RoleExists(rolename))
                throw new ProviderException("Role name already exists.");
            if (rolename.Length > 255)
                throw new ProviderException("Role name cannot exceed 255 characters.");

            _roleRepository.Add(role);
        }

        public void UpdateRole(Role role)
        {
            var originalRoleResult = _roleRepository.FindBy(role.Id);
            if (originalRoleResult.Value == null)
                throw new ProviderException("Role can not find.");
            var rolename = role.Name;
            if (string.IsNullOrEmpty(rolename))
                throw new ProviderException("Role name cannot be empty or null.");
            if (rolename.Contains(","))
                throw new ArgumentException("Role names cannot contain commas.");
            if (originalRoleResult.Value.Name != rolename && RoleExists(rolename))
                throw new ProviderException("Role name already exists.");
            if (rolename.Length > 255)
                throw new ProviderException("Role name cannot exceed 255 characters.");

            _roleRepository.Update(role);
        }

        public Role FindRoleById(int id)
        {
            var result = _roleRepository.FindBy(id);
            return result.Value;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            try
            {
                var roleResult = _roleRepository.FindByName(roleName);
                if (roleResult.Value == null)
                    return false;
                _roleRepository.Delete(roleResult.Value.Id);
                return true;

            }
            catch (Exception)
            {
                if (throwOnPopulatedRole)
                    throw;
                return false;
            }

        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var roleResult = _roleRepository.FindByName(roleName);
            if (roleResult.Value == null)
                return null;
            var regex = new Regex(usernameToMatch);
            if (usernameToMatch.Length == 0)
                return roleResult.Value.Users.Select(u => u.Username).ToArray();
            return roleResult.Value.Users.Where(u => regex.IsMatch(u.Username)).Select(u => u.Username).ToArray();
        }

        public override string[] GetAllRoles()
        {
            return GetAllRolesCustom().Select(x => x.Name).ToArray();
        }

        public IList<Role> GetAllRolesCustom()
        {
            var listResponse = _roleRepository.GetAllRoles();
            return listResponse.TotalItems > 0 ? listResponse.List : new List<Role>();
        }

        public override string[] GetRolesForUser(string username)
        {
            //var userResult = userRepository.FindBy(username);
            return GetRolesForUserCustom(username).Select(x => x.Name).ToArray();
        }


        public IList<Role> GetRolesForUserCustom(string username)
        {
            var userResult = _userRepository.FindBy(username);
            return userResult?.Value?.Roles;
        }


        public IList<Right> GetRightsForUser(string username)
        {
            var rightsResult = _rightRepository.GetRightsForUser(username);
            return rightsResult.List;
        }

        public IList<Right> GetAllRights()
        {
            return _rightRepository.GetAllRights().List;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var roleResult = _roleRepository.FindByName(roleName);
            return roleResult.Value?.Users.Select(x => x.Username).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = _userRepository.FindBy(username);
            var isUserInRole = user?.Value?.Roles?.Any(x => x.Name == roleName);
            return isUserInRole != null && (bool)isUserInRole;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var roles = _roleRepository.GetRolesByNames(roleNames);
            foreach (var username in usernames)
            {
                var user = _userRepository.FindBy(username);
                if (roles.List != null)
                {
                    foreach (var role in roles.List)
                    {
                        if (user.Value == null) continue;
                        var first = user.Value.Roles.FirstOrDefault(x => x.Id == role.Id);
                        user.Value.Roles.Remove(first);
                    }
                }
                if (user.Value != null) _userRepository.Update(user.Value);
            }
        }

        public void SetRolesForUser(string username, string[] roleNames)
        {
            var userResult = _userRepository.FindBy(username);
            if (userResult.Value == null) return;

            if (userResult.Value.Roles == null)
                userResult.Value.Roles = new List<Role>();
            else
                userResult.Value.Roles.Clear();

            var rolesResult = _roleRepository.GetRolesByNames(roleNames);

            if (rolesResult.List != null)
            {
                foreach (var role in rolesResult.List)
                {
                    userResult.Value.Roles.Add(role);
                }
            }

            _userRepository.Update(userResult.Value);
        }

        public override bool RoleExists(string roleName)
        {
            return _roleRepository.FindByName(roleName).Value != null;
        }
    }
}
