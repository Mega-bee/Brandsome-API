using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{

    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class BusinessesController : APIBaseController
    {
        private readonly IBusinessBL _Bbl;

        public BusinessesController(IBusinessBL bBL)
        {
            _Bbl = bBL;
        }

  
        [AllowAnonymous]
        [HttpGet]
        public async Task <IActionResult> GetBusinesses([FromQuery] string sortBy, [FromQuery] List<int> services)
        {
            return Ok(await _Bbl.GetBusinsses(services,sortBy, Request));
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


        [HttpPost("{businessId}")]
        public async Task<IActionResult> FollowBusiness([FromRoute] int businessId , [FromForm] bool IsFollow)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.FollowBusiness(uid, businessId , IsFollow));
        }

        [AllowAnonymous]
        [HttpPost("{businessId}")]
      
        public async Task<IActionResult> RegisterNewPhoneClick([FromRoute] int businessId)
        {
            string uid = null;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Ok(await _Bbl.RegisterNewPhoneClick(uid, businessId ));
        }


        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] CreateReview_VM review)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.AddReview(review, uid));
        }


        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromForm] CreateBusiness_VM business)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.CreateBusiness(uid, business));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBusiness([FromForm] CreateBusiness_VM business)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.UpdateBusiness( business));
        } 
        
        [HttpPut("{businessCityId}")]
        public async Task<IActionResult> DeleteBusinessCity([FromRoute] int businessCityId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusinessCity(uid,businessCityId));
        } 
        
        [HttpPut("{businessServiceId}")]
        public async Task<IActionResult> DeleteBusinessService([FromRoute] int businessServiceId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusinessService(uid, businessServiceId));
        }

        [HttpGet("{businessId}")]
        public async Task<IActionResult> GetBusinessCities([FromRoute] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.GetBusinessCities(businessId, uid));
        }

        [HttpGet("{businessId}")]
        public async Task<IActionResult> GetBusinessServices([FromRoute] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.GetBusinessServices(businessId, uid));
        }



        [HttpPut("{businessId}")]
        public async Task<IActionResult> DeleteBusiness([FromRoute] int businessId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _Bbl.DeleteBusiness(uid, businessId));
        }





    }
}
