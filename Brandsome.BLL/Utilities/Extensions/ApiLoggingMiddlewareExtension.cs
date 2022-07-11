using Brandsome.BLL.Utilities.CustomMiddleWare;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities.Extensions
{
    public static class ApiLoggingMiddlewareExtension
    {
        public static void ConfigureCustomApiLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiLoggingMiddleware>();
        }
    }
}
