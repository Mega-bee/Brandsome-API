using Brandsome.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class NotificationsController : APIBaseController
    {
        private readonly INotificationBL _notificationBL;

        public NotificationsController(INotificationBL notificationBL)
        {
            _notificationBL = notificationBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _notificationBL.GetUserNotifications(Request, uid));
        }
    }
}
