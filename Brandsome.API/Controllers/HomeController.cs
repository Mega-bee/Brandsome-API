using Brandsome.BLL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{
  
    public class HomeController : APIBaseController
    {
        private readonly IHomePageService _homePageService;

        public HomeController(IHomePageService homePageService)
        {
            _homePageService = homePageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomePage()
        {
            string uid = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Ok(await _homePageService.GetMainLists(Request,uid));
        }
    }
}
