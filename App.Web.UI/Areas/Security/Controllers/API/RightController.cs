using App.Core.Validation;
using App.Membership.Domain;
using App.Membership.Repositories;
using App.Membership.Repositories.Abstract;
using App.Repository.Models.Messages;
using App.Web.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace App.Web.UI.Areas.Security.Controllers.API
{
    [Authorize(Roles = "Administrators"), RoutePrefix("api/right")]
    public class RightController : ApiController
    {
        private readonly IRightRepository _rightRepository;

        public RightController()
        {
            _rightRepository = RepositoryFactory.GetRightRepository();
        }

        [HttpGet, Route("getallRights")]
        public ObjectWrapperWithNameOfSet GetAllRights()
        {
            try
            {
                var rights = _rightRepository.GetAllRights();
                if (!rights.Success)
                    return new ObjectWrapperWithNameOfSet("", rights.Errors);
                var result = new
                {
                    Value = rights.List.Select(x => new { x.Id, x.Name, x.Description }),
                    Success = true
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

        [HttpGet, Route("GetAll/{pageSize}/{pageNumber}/{rigthName?}")]
        public ObjectWrapperWithNameOfSet GetAll(int pageSize, int pageNumber, string rigthName = null)
        {
            try
            {
                var resultRight = _rightRepository.GetAllRights();
                if (!resultRight.Success)
                    return new ObjectWrapperWithNameOfSet("", resultRight.Errors);

                var data = resultRight.List;
                if (rigthName != "null" && rigthName != null)
                {
                    data = data.Where(rigth => rigth.Name.Contains(rigthName)).ToList();
                }

                var totalCount = data.Count;

                var totalPages = Math.Ceiling((double)totalCount / pageSize);

                var rights = data.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                var result = new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    List = rights.Select(x => new { x.Id, x.Name, x.Description }),
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

        [HttpPost, Route("Save")]
        public ObjectWrapperWithNameOfSet Save([FromBody] Right right)
        {
            try
            {
                Right updateRight = null;
                if (right.Id != 0)
                {
                    var resultRight = _rightRepository.FindBy(right.Id);
                    if (resultRight.Value != null)
                    {
                        updateRight = resultRight.Value;
                        updateRight.LogEnabled = right.LogEnabled;
                        updateRight.Name = right.Name;
                        updateRight.Description = right.Description;
                    }
                }
                else
                {
                    updateRight = right;
                }

                _rightRepository.Update(updateRight);

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<Right>
                {
                    Success = true,
                    Value = updateRight
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

        [HttpDelete, Route("Delete")]
        public ObjectWrapperWithNameOfSet Delete(int rightId)
        {
            try
            {
                var rightResponse = _rightRepository.FindBy(rightId);
                if (rightResponse.Value != null)
                {
                    _rightRepository.Delete(rightResponse.Value.Id);
                    return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                    {
                        Success = true
                    });
                }
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string> { Success = true });
            }
            catch (Exception e)
            {
                return new ObjectWrapperWithNameOfSet("", new ValueResponse<string>
                {
                    Errors = new List<BrokenRule> { new BrokenRule(e.Message) }
                });
            }
        }

        [HttpGet, Route("GetItem/{id}")]
        public ObjectWrapperWithNameOfSet GetItem(int id)
        {
            try
            {
                var rightResult = _rightRepository.FindBy(id);
                if (!rightResult.Success)
                    return new ObjectWrapperWithNameOfSet("", rightResult.Errors);

                rightResult.Value.Roles = null;

                return new ObjectWrapperWithNameOfSet("", new ValueResponse<Right>
                {
                    Success = true,
                    Value = rightResult.Value
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

    }
}