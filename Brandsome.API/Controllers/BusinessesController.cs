using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{
   
    public class BusinessesController : APIBaseController
    {
        private readonly IBusinessBL _Bbl;

        public BusinessesController(IBusinessBL bBL)
        {
            _Bbl = bBL;
        }

        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("{serviceId?}")]
        public async Task <IActionResult> GetBusinesses([FromQuery] string sortBy, [FromRoute] int serviceId = 0)
        {
            return Ok(await _Bbl.GetBusinsses(serviceId,sortBy, Request));
        }


        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("{businessId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBusiness([FromRoute] int businessId)
        {
            string uid = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Ok(await _Bbl.GetBusiness(uid, businessId, Request));
        }


        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //[HttpPost("{businessId}")]
        //public async Task<IActionResult> FollowBusiness([FromRoute] int businessId)
        //{
        //    string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    return Ok(await _Bbl.FollowBusiness(uid, businessId));
        //}


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] CreateReview_VM review)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.AddReview(review, uid));
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromForm] CreateBusiness_VM business)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.CreateBusiness(uid, business));
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> UpdateBusiness([FromForm] CreateBusiness_VM business)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.UpdateBusiness( business));
        } 
        
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("{businessCityId}")]
        public async Task<IActionResult> DeleteBusinessCity([FromRoute] int businessCityId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusinessCity(uid,businessCityId));
        } 
        
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("{businessServiceId}")]
        public async Task<IActionResult> DeleteBusinessService([FromRoute] int businessServiceId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusinessService(uid, businessServiceId));
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("{businessId}")]
        public async Task<IActionResult> GetBusinessCities([FromRoute] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.GetBusinessCities(businessId, uid));
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("{businessId}")]
        public async Task<IActionResult> GetBusinessServices([FromRoute] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.GetBusinessServices(businessId, uid));
        }





    }
}
