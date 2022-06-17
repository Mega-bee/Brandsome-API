using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Brandsome.BLL.ViewModels;
using Brandsome.BLL.Utilities.Logging;

namespace Brandsome.BLL.Utilities.CustomMiddleWare
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionHandler(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext);
            }


        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            ResponseModel resp = new ResponseModel()
            {
                Data = new DataModel
                {
                    Data = "",
                    Message = ""
                },
                ErrorMessage = "Something Went Wrong",
                StatusCode = 500
            };
            string jsonString = JsonSerializer.Serialize(resp);
            return context.Response.WriteAsync(jsonString);
        }



    }
}
