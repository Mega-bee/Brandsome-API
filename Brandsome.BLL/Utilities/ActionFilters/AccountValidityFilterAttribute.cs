using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities.ActionFilters
{
    public class AccountValidityFilterAttribute : IActionFilter
    {
        private readonly IUnitOfWork _unit;

        public AccountValidityFilterAttribute(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            string uid = null;
            var claims = new ClaimsIdentity("Custom");
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
    
            if (identity.IsAuthenticated)
            {
                uid = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                bool? isDeleted =  _unit.UserRepository.GetAll(u => u.Id == uid).Select(u => u.IsDeleted).FirstOrDefault();
                
                if ((bool)isDeleted)
                {
                    ResponseModel responseModel = new ResponseModel();
                    responseModel.Data = new DataModel { Data = "", Message = "" };
                    responseModel.ErrorMessage = "Account is deleted";
                    responseModel.StatusCode = 401;
                    context.Result = new UnauthorizedObjectResult(responseModel);
                }


            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
