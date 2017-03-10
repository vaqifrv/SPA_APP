using App.Membership.Infrastructure.Messages;
using System;
using App.Membership.Domain;

namespace App.Membership.Repositories.Abstract
{
    class RightRepositoryFake : IRightRepository
    {

        public ValueResponse<Right> FindByName(string name)
        {
            return new ValueResponse<Right> { Value = new Right { Name = name, Id = 1 } };
        }

        public ValueResponse<bool> CheckUserHasRight(string username, string rightName)
        {
            throw new NotImplementedException();
        }


        public ListResponse<Right> GetAllRights()
        {
            throw new NotImplementedException();
        }


        public ListResponse<Right> GetRightsForUser(string username)
        {
            throw new NotImplementedException();
        }

        public ValueResponse<Right> Add(Right entity)
        {
            throw new NotImplementedException();
        }

        public ValueResponse<Right> Update(Right entity)
        {
            throw new NotImplementedException();
        }

        public ValueResponse<Right> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ValueResponse<Right> FindBy(int id)
        {
            throw new NotImplementedException();
        }
    }
}
