using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;
using System.Security.Claims;

namespace Brandsome.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class AccountsController : APIBaseController
    {
        private readonly IAuthBO _auth;

        public AccountsController(IAuthBO auth)
        {
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RequestOtp([FromForm] string phoneNumber,[FromForm] string username)
        {
            return Ok(await _auth.RequestOtp(phoneNumber, username));
        }  
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResendOtp([FromForm] string phoneNumber)
        {
            string uid = null;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Ok(await _auth.ResendOtp(phoneNumber,uid));
        }

        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromForm] string phoneNumber, [FromForm] string otp)
        {
            string uid = null;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Ok(await _auth.VerifyOtp(phoneNumber, otp,uid));
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountSetings()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.GetAccountSettings(uid,Request));
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.GetProfile(uid, Request));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] CompleteProfile_VM profile)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.CompleteProfile(profile, uid, Request));
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowedBusinessses()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.GetFollowedBusinesses(uid,Request));
        }

        [HttpPut]
        public async Task<IActionResult> RefreshFcmToken([FromForm] string token)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.RefreshFcmToken(uid, token));
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAccount()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.DeleteAccount(uid));
        }

    }


}
