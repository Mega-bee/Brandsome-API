using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class ResponseModel
    {
        public DataModel Data { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public ValidationResultModel Validation { get; set; }

    }

    public partial class DataModel
    {
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

    public partial class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }

    public partial class ValidationResultModel
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            ValidationErrors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new
                 ValidationError(key, x.ErrorMessage)))
                .ToList();
        }
    }
}
