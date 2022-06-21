using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{
   
    public class PostsController : APIBaseController
    {
        private readonly IBusinessBL _Bbl;

        public PostsController(IBusinessBL bbl)
        {
            _Bbl = bbl;
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePost_VM post)
        {
            return Ok(await _Bbl.CreatePost(post));
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromForm] CreatePost_VM post)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.UpdatePost(uid, post));
        }

     
    }
}
