using Brandsome.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{

    public class InterestsController : APIBaseController
    {
        private readonly IInterestsBL _interestsBL;

        public InterestsController(IInterestsBL interestsBL)
        {
            _interestsBL = interestsBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _interestsBL.GetCategories());
        }
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSubcategories([FromRoute] int categoryId)
        {
            return Ok(await _interestsBL.GetSubCategories(categoryId));
        }

        [HttpGet("{subcategoryId}")]
        public async Task<IActionResult> GetServices([FromRoute] int subcategoryId)
        {
            return Ok(await _interestsBL.GetServices(subcategoryId));
        }

        [HttpGet]
        public async Task<IActionResult> GetSearchCategories()
        {
            return Ok(await _interestsBL.GetSearchCategories());
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> SetUserInterests([FromForm] List<int> services)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _interestsBL.SetInterests(uid,services));
        }

    }
}
