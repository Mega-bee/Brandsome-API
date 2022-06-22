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

    public class AccountsController : APIBaseController
    {
        private readonly IAuthBO _auth;

        public AccountsController(IAuthBO auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> RequestOtp([FromForm] string phoneNumber,[FromForm] string username)
        {
            return Ok(await _auth.RequestOtp(phoneNumber, username));
        }  
        
        [HttpPost]
        public async Task<IActionResult> ResendOtp([FromForm] string phoneNumber)
        {
            return Ok(await _auth.ResendOtp(phoneNumber));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromForm] string phoneNumber, [FromForm] string otp)
        {
            return Ok(await _auth.VerifyOtp(phoneNumber, otp));
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAccountSetings()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.GetAccountSettings(uid));
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] CompleteProfile_VM profile)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.CompleteProfile(profile, uid, Request));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetFollowedBusinessses()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.GetFollowedBusinesses(uid,Request));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetProfile()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.GetProfile(uid, Request));
        }

    }


}
