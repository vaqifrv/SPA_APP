using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using NHibernate.Util;
using App.Membership.Providers;
using App.Membership.Repositories.Abstract;
using App.Core.Validation;
using App.Web.UI.Infrastructure;
using App.Membership.Domain;
using App.Membership.Repositories;
using App.Repository.Models.Messages;
using App.Web.UI.Areas.Security.Models;

namespace App.Web.UI.Areas.Security.Controllers.API
{
    [Authorize, RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        #region Dependency Injection
        private AgRoleProvider _agRoleProvider;
        private IRightRepository _rightRepository;

        public RoleController()
        {
            _rightRepository = RepositoryFactory.GetRightRepository();
            _agRoleProvider = new AgRoleProvider();
        }
        #endregion

        [HttpGet, Route("GetAllRoles")]
        public ObjectWrapperWithNameOfSet GetAllRoles()
        {
            try
            {
                var roles = _agRoleProvider.GetAllRolesCustom();
                return new ObjectWrapperWithNameOfSet("", new ListResponse<Role>
                {
                    List = roles
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }

        }

        [HttpGet, Route("GetAll/{pageSize}/{pageNumber}/{roleName?}")]
        public ObjectWrapperWithNameOfSet GetAll(int pageSize, int pageNumber, string roleName = null)
        {
            try
            {
                var data = _agRoleProvider.GetAllRolesCustom();

                if (roleName != "null" && roleName != null)
                {
                    data = data.Where(x => x.Name.Contains(roleName)).ToList();
                }

                var totalCount = data.Count();

                var totalPages = Math.Ceiling((double)totalCount / pageSize);

                var roles = data.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                var result = new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    List = roles.Select(x => new { x.Id, x.Name, x.Description }),
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

        [HttpGet, Route("GetDataForCreateOrEdit/{id?}")]
        public ValueResponse<RolesDictionaryEditViewModel> GetDataForCreateOrEdit(int? id = null)
        {
            try
            {
                var role = id.HasValue ? _agRoleProvider.FindRoleById(id.Value) : new Role();

                var model = new RolesDictionaryEditViewModel
                {
                    Role = role,
                    CheckedRights = RightsInRoleList(role.Rights)
                };

                model.CheckedRights.ForEach(x => x.Item.Roles = null);
                model.Role.Rights = null;
                model.Role.Users = null;

                return new ValueResponse<RolesDictionaryEditViewModel>
                {
                    Value = model,
                    Success = true
                };

            }
            catch (Exception e)
            {
                return new ValueResponse<RolesDictionaryEditViewModel>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) },
                    Success = false
                };
            }
        }

        [HttpPost, Route("Save")]
        public ObjectWrapperWithNameOfSet Save([FromBody] RolesDictionaryEditViewModel model)
        {
            try
            {
                var errors = model.Role.Validate(new ValidationContext(model.Role, null, null));

                if (errors.Any())
                {
                    var brokenRules = new List<BrokenRule>();
                    foreach (var error in errors)
                    {
                        foreach (var memberName in error.MemberNames)
                        {
                            brokenRules.Add(new BrokenRule("Role_" + memberName + ", Message: " + error.ErrorMessage));
                        }
                    }

                    return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                    {
                        Errors = brokenRules
                    });
                }


                Role role = null;
                role = model.Role.Id == 0 ? new Role() : _agRoleProvider.FindRoleById(model.Role.Id);


                role.Name = model.Role.Name;
                role.Description = model.Role.Description;

                role.Rights = (model.CheckedRights
                                    .Where(x => x.Checked)
                                    .Select<CheckedItem<Right>, Right>(x => x.Item).ToList());

                if (role.Id == 0)
                {
                    _agRoleProvider.CreateRole(role);
                }
                else
                {
                    _agRoleProvider.UpdateRole(role);
                }


                return new ObjectWrapperWithNameOfSet("", new ValueResponse<Role>
                {
                    Value = role,
                    Success = true
                });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", null);
            }


        }

        [HttpDelete, Route("Delete/{id}")]
        public ObjectWrapperWithNameOfSet Delete(int id)
        {
            try
            {
                var role = _agRoleProvider.FindRoleById(id);
                if (_agRoleProvider.DeleteRole(role.Name, false))
                {
                    return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                    {
                    });
                }
                else
                {
                    return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                    {
                        Errors = new List<BrokenRule> { new BrokenRule("Error occour when deleting role") }
                    });
                }
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }

        }

        public IList<CheckedItem<Right>> RightsInRoleList(IList<Right> appliedRights)
        {
            IList<CheckedItem<Right>> list = null;

            if (appliedRights != null)
            {
                list = appliedRights
                            .Select<Right, CheckedItem<Right>>(x => new CheckedItem<Right> { Checked = true, Item = x })
                            .OrderBy(x => x.Item.Name)
                            .Union(_rightRepository.GetAllRights().List
                                                .Where(x => !appliedRights.Contains(x))
                                                .OrderBy(x => x.Name)
                                                .Select<Right, CheckedItem<Right>>(x => new CheckedItem<Right> { Checked = false, Item = x })
                                                )
                            .ToList();
            }
            else
            {
                list = _rightRepository.GetAllRights().List
                                        .OrderBy(x => x.Name)
                                        .Select<Right, CheckedItem<Right>>(x => new CheckedItem<Right> { Checked = false, Item = x })
                                        .ToList();
            }


            return list;
        }
    }
}