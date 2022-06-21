using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet]
        public IActionResult GetBusinsses([FromForm] int serviceId, [FromForm] string sortBy)
        {
            return Ok(_Bbl.GetBusinsses(serviceId,sortBy));
        }
           
        
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet]
        public IActionResult GetBusiness([FromForm] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(_Bbl.GetBusiness(uid, businessId));
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> FollowBusiness([FromForm] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.FollowBusiness(uid, businessId));
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromForm] CreateReview_VM review)
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
        [HttpPut]
        public async Task<IActionResult> DeleteBusinessCity([FromForm] int businessCityId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusinessCity(uid,businessCityId));
        } 
        
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> DeleteBusinessService([FromForm] int businessServiceId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusinessService(uid, businessServiceId));
        }
        
        
    


    }
}
