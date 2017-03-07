using App.Core.Validation;
using System.Collections.Generic;

namespace App.Web.UI.Infrastructure
{
    public class BaseApiResponse<T>
    {
        public string NameOfSet { get; set; }
        public T Data { get; set; }

        public List<BrokenRule> ErrorList { get; set; }

        public BaseApiResponse(string nameOfSet, T data)
        {
            NameOfSet = nameOfSet;
            if (data != null)
                Data = data;
        }

        public BaseApiResponse(List<BrokenRule> errorList)
        {
            if (errorList != null)
                ErrorList = errorList;
        }

    }
}