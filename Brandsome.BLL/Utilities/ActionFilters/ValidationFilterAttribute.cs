using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ResponseModel resp = new ResponseModel()
                {
                    ErrorMessage = "Validation Errors",
                    StatusCode = 400,
                    Data = new DataModel
                    {
                        Data = "",
                        Message = "",
                    },
                    Validation = new ValidationResultModel(context.ModelState)
                };
                context.Result = new OkObjectResult(resp);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
