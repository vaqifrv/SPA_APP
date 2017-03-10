using System;
using System.Collections.Generic;
using System.Linq;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public class UserRepositoryFake : IUserRepository
    {
        public static List<User> AllUsers
        {
            get
            {
                var resp = new List<User>();
                resp.AddRange(Users);
                //   resp.AddRange(LockedUsers);
                return resp;
            }
        }

        //  public static List<Domain.Entities.User> LockedUsers = new List<Domain.Entities.User>();
        public static List<User> Users = new List<User>();

        public ResponseBase Lock(string username)
        {
            try
            {
                var user = Users.Where(u => u.Username == username).Select(u => u).FirstOrDefault();
                var result = new ResponseBase();
                if (user != null)
                {
                    user.IsEnabled = false;
                    //  Users.Remove(U);
                    // LockedUsers.Add(U);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase { Errors = new List<Exception> { ex } };
            }

        }

        public ValueResponse<bool> ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Username == username && u.Password == oldPassword);
                var result = new ValueResponse<bool>();

                if (user == null)
                    result.Value = false;
                else
                    user.Password = newPassword;

                result.Value = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ValueResponse<bool> { Errors = new List<Exception> { ex } };
            }

        }

        public ValueResponse<bool> ValidateUser(string username, string password)
        {
            try
            {
                var hasUser = Users.Any(u => u.Username == username && u.Password == password);
                return new ValueResponse<bool> { Value = hasUser };
            }
            catch (Exception ex)
            {
                return new ValueResponse<bool> { Value = false, Errors = new List<Exception> { ex } };
            }
        }

        public ValueResponse<User> Add(User entity)
        {
            try
            {
                if (entity == null)
                {
                    return new ValueResponse<User>
                    {
                        Errors = new List<Exception> { new Exception("Entity object is null!") }
                    };
                }
                Users.Add(entity);
                return new ValueResponse<User> { Value = entity };
            }
            catch (Exception ex)
            {
                return new ValueResponse<User> { Errors = new List<Exception> { ex } };
            }
        }

        public ValueResponse<User> Update(User entity)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Username == entity.Username);
                if (user == null) return new ValueResponse<User> { };
                user.IsDeleted = entity.IsDeleted;
                user.IsEnabled = entity.IsEnabled;
                user.Password = entity.Password;
                return new ValueResponse<User> { Value = user };
            }
            catch (Exception ex)
            {
                return new ValueResponse<User> { Errors = new List<Exception> { ex } };
            }
        }


        public ValueResponse<User> FindBy(string id)
        {
            var user = Users.FirstOrDefault(u => u.Username == id);

            if (user == null)
            {
                return new ValueResponse<User> { };
            }

            var right = new Right
            {
                Name = "Update",
                Id = 1
            };
            var rights = new List<Right> { right };

            var role = new Role
            {
                Id = 1,
                Name = "admin",
                Rights = rights
            };

            if (user.Roles == null)
                user.Roles = new List<Role> { role }; //new List<Role>();

            return new ValueResponse<User> { Value = user };

        }


        public ValueResponse<User> Delete(string id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Username == id);
                if (user == null)
                {
                    return new ValueResponse<User>
                    {
                        Errors = new List<Exception> { new Exception("No data found!") }
                    };
                }
                Users.Remove(user);
                return new ValueResponse<User> { Value = user };
            }
            catch (Exception ex)
            {
                return new ValueResponse<User> { Errors = new List<Exception> { ex } };
            }

        }
        public ListResponse<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex >= 0)
            {
                var users = Users.Skip(pageIndex * (pageSize - 1)).Take(pageSize).ToList();
                totalRecords = users.Count;
                var result = new ListResponse<User>();
                if (users.Count > 0)
                    result.List = users;
                return result;
            }
            totalRecords = 0;
            return null;
        }


        public ResponseBase DeleteUsersForTest()
        {
            throw new NotImplementedException();
        }


        public ValueResponse<bool> IsNewDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
