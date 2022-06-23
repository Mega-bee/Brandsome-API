using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class PostsController : APIBaseController
    {
        private readonly IPostsBL _postsBL;

        public PostsController(IPostsBL postsBl)
        {
            _postsBL = postsBl;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePost_VM post)
        {
            return Ok(await _postsBL.CreatePost(post));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromForm] CreatePost_VM post)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _postsBL.UpdatePost(uid, post));
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> DeletePost([FromRoute] int postId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _postsBL.DeletePost(uid,postId));
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> LikePost([FromRoute] int postId,[FromForm] bool isLike)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _postsBL.LikePost(uid,postId,isLike));
        }


    }
}
