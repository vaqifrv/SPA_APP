using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Security;
using App.Membership.Repositories.Abstract;
using App.Web.UI.Infrastructure;
using App.Membership.Providers;
using App.Membership.Repositories.NHibernate;
using App.Core.Validation;
using App.Membership.Domain;
using App.Repository.Models.Messages;
using App.Web.UI.Areas.Security.Models;

namespace App.Web.UI.Areas.Security.Controllers.API
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly AgMembershipProvider _provider;
        private readonly AgRoleProvider _roleProvider;
        private readonly IProfilePropertyRepository _profilePropertyRepository;
        private readonly IProfileRepository _profileRepository;
        private IUserRepository _userRepository;
        private readonly ILoggingRepository _loggingRepository;
        private ILogActionRepository _logActionRepository;
        private readonly IRoleRepository _roleRepository;
        private IRightRepository _rightRepository;


        public UserController()
        {
            _logActionRepository = new LogActionRepository();
            _provider = new AgMembershipProvider();
            _roleProvider = new AgRoleProvider();
            _profilePropertyRepository = new ProfilePropertyRepository();
            _profileRepository = new ProfileRepository();
            _userRepository = new UserRepository();
            _loggingRepository = new LoggingRepository();
            _roleRepository = new RoleRepository();
            _rightRepository = new RightRepository();
        }

        [HttpGet, Route("GetAllUsers")]
        public ObjectWrapperWithNameOfSet GetAllUsers()
        {
            try
            {
                var users = System.Web.Security.Membership.GetAllUsers();
                return new ObjectWrapperWithNameOfSet("", users);
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }

        }

        [HttpGet, Route("GetAllUsers2")]
        public BaseApiResponse<MembershipUserCollection> GetAllUsers2()
        {
            try
            {
                var users = System.Web.Security.Membership.GetAllUsers();
                return new BaseApiResponse<MembershipUserCollection>("", users);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        [HttpGet, Route("getAll/{pageSize}/{pageNumber}/{userName?}")]
        public ObjectWrapperWithNameOfSet GetAll(int pageSize, int pageNumber, string userName = null)
        {
            try
            {
                var data = System.Web.Security.Membership.GetAllUsers().Cast<MembershipUser>();

                if (userName != "null" && userName != null)
                {
                    data = data.Where(user => user.UserName.Contains(userName)).ToList();
                }

                var totalCount = data.Count();

                var totalPages = Math.Ceiling((double)totalCount / pageSize);

                var users = data.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                var result = new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Users = users,
                    Success = true
                };

                return new ObjectWrapperWithNameOfSet("", result);
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>()
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }
        }

        [HttpGet, Route("GetUserDataByUsername/{username}")]
        public ObjectWrapperWithNameOfSet GetUserDataByUsername(string username)
        {
            try
            {
                var user = _provider.GetUser(username, false);

                var model = new AdminEditViewModel
                {
                    User = new User { Username = user?.UserName, IsEnabled = !user.IsLockedOut },
                    CheckedRoles = RolesInUserList(_roleProvider.GetRolesForUserCustom(username))
                };

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<AdminEditViewModel>
                {
                    Success = true,
                    Value = model
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<AdminEditViewModel>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }

        }

        [HttpPost, Route("Create")]
        public ObjectWrapperWithNameOfSet Create([FromBody]User model)
        {
            try
            {
                MembershipCreateStatus st;
                var user = System.Web.Security.Membership.CreateUser(model.Username, model.Password, null, null, null, true, out st);

                var result = new
                {
                    Status = st,
                    Username = user?.UserName
                };

                return new ObjectWrapperWithNameOfSet("", result);
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }
        }



        private IList<CheckedItem<Role>> RolesInUserList(IList<Role> appliedRoles)
        {
            IList<CheckedItem<Role>> list = null;

            if (appliedRoles != null)
            {
                list = appliedRoles
                            .Select(x => new CheckedItem<Role> { Checked = true, Item = x })
                            .OrderBy(x => x.Item.Name)
                            .Union(_roleProvider.GetAllRolesCustom()
                                                .Where(x => !appliedRoles.Contains(x))
                                                .OrderBy(x => x.Name)
                                                .Select(x => new CheckedItem<Role> { Checked = false, Item = x })
                                                )
                            .ToList();
            }
            else
            {
                list = _roleProvider.GetAllRolesCustom()
                                        .OrderBy(x => x.Name)
                                        .Select(x => new CheckedItem<Role> { Checked = false, Item = x })
                                        .ToList();
            }


            return list;
        }

        [HttpPost, Route("Update")]
        public ObjectWrapperWithNameOfSet Update([FromBody]AdminEditViewModel userModel)
        {
            try
            {
                var roles = userModel.CheckedRoles
               .Where(r => r.Checked)
               .Select(r => r.Item.Name).ToArray();

                _roleProvider.SetRolesForUser(userModel.User.Username, roles);

                if (userModel.User.IsEnabled)
                {
                    _provider.ChangeUserEnabledStatus(userModel.User.Username, true);
                }
                else
                {
                    _provider.ChangeUserEnabledStatus(userModel.User.Username, false);
                }

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Success = false,
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }

        }

        [HttpPost, Route("delete")]
        public ObjectWrapperWithNameOfSet Delete(string userName)
        {
            try
            {
                System.Web.Security.Membership.DeleteUser(userName);
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>()
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>()
                {
                    Success = false,
                    Value = null,
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }

        }

        [HttpGet, Route("Lock/{userName}/_checked")]
        public ObjectWrapperWithNameOfSet Lock(string userName, bool _checked)
        {
            try
            {
                _provider.ChangeUserEnabledStatus(userName, !_checked);

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Success = false,
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }
        }

        [HttpGet, Route("ResetPassword/{username}/{newPassword}/{confirmPassword}")]
        public ObjectWrapperWithNameOfSet ResetPassword(string username, string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword != confirmPassword)
                    return new ObjectWrapperWithNameOfSet("", new ListResponse<string>()
                    {
                        Success = false,
                        Errors = new List<BrokenRule>() { new BrokenRule("Passwords are not the same!") }
                    });


                _provider.ResetPasswordByAdmin(username, newPassword);

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>()
                {
                    Success = true
                });
            }
            catch (Exception e)
            {

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>()
                {
                    Errors = new List<BrokenRule>() { new BrokenRule(e.Message) }
                });
            }

        }

        [HttpGet, Route("GetProfileDataByUsername/{username}")]
        public ObjectWrapperWithNameOfSet GetProfileDataByUsername(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return new ObjectWrapperWithNameOfSet("", new ListResponse<string>
                    {
                        Success = false
                    });
                }
                var allProperties = _profilePropertyRepository.GetAllProperties();

                if (!allProperties.Success)
                    return new ObjectWrapperWithNameOfSet("", allProperties.Errors);
                var listResponseProfile = _profileRepository.GetUserProfile(username);

                if (!listResponseProfile.Success)
                    return new ObjectWrapperWithNameOfSet("", listResponseProfile.Errors);

                var model = new ProfileViewModel
                {
                    username = username,
                    propertyList = allProperties.TotalItems > 0 ? allProperties.List : null,
                    profileData = listResponseProfile.TotalItems > 0 ? listResponseProfile.List : null
                };

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<ProfileViewModel>()
                {
                    Success = true,
                    Errors = null,
                    Value = model
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<ProfileViewModel>
                {
                    Success = false,
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) },
                    Value = null
                });
            }

        }

        [HttpGet, Route("GetUserDetailsbyUseName/{username}")]
        public ObjectWrapperWithNameOfSet Details(string username)
        {
            try
            {
                var logsForUser = _loggingRepository.GetLogsForUser(username);
                if (!logsForUser.Success)
                    return new ObjectWrapperWithNameOfSet("", logsForUser.Errors);

                var allRolesForUser = _roleRepository.GetAllRolesForUser(username);
                if (!allRolesForUser.Success)
                    return new ObjectWrapperWithNameOfSet("", allRolesForUser.Errors);

                var model = new UserDetailsViewModel
                {
                    User = _provider.GetUser(username, false),
                    Logs = logsForUser.TotalItems > 0 ? logsForUser.List : null,
                    Roles = allRolesForUser.TotalItems > 0 ? allRolesForUser.List : null
                };

                var listRight = new List<Right>();

                if (model.Roles != null)
                {
                    foreach (var item in model.Roles)
                    {
                        listRight.AddRange(item.Rights);
                    }
                }

                model.Rights = listRight;

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<UserDetailsViewModel>
                {
                    Success = true,
                    Errors = null,
                    Value = model
                });
            }
            catch (Exception e)
            {

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<UserDetailsViewModel>
                {
                    Success = false,
                    Errors = new List<BrokenRule>() { new BrokenRule(e.Message) },
                    Value = null
                });
            }

        }
    }
}