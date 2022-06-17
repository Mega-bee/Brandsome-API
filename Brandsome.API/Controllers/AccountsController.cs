﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Brandsome.BLL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Brandsome.BLL.IServices;
using Brandsome.BLL.ViewModels;

namespace Brandsome.API.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly IAuthBO _auth;

        public AccountsController(IAuthBO auth)
        {
            _auth = auth;
        }

        //[HttpPost]
        //public async Task<IActionResult> SignIn([FromForm] EmailSignIn_VM model)
        //{
        //    ResponseModel resp = await _auth.EmailSignIn(model);
        //    return Ok(resp);
        //}

        //[HttpPost]
        //public async Task<IActionResult> ForgetPassword([FromForm] string identifier)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    ResponseModel forgetPassword = await _auth.ForgetPassword(identifier, Request);
        //    return Ok(forgetPassword);

        //}

        //[HttpPost("{roleId}")]
        //public async Task<IActionResult> EmailSignUp([FromForm] EmailSignUp_VM model, [FromRoute] int roleId)
        //{

        //    ResponseModel signup = await _auth.SignUpWithEmail(model, roleId, Request);
        //    return Ok(signup);

        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> CompleteProfile([FromForm] CompleteProfile_VM model)
        //{
        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel completeProfile = await _auth.CompleteProfile(model, uid, Request);
        //    return Ok(completeProfile);

        //}

        //[HttpPost]
        //public async Task<IActionResult> EmailSignIn([FromForm] EmailSignIn_VM model)
        //{
        //    return Ok(await _auth.EmailSignIn(model, Request));


        //}

        //[HttpGet]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> GetUserProfile()
        //{

        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel getUserProfile = await _auth.GetUserProfile(uid, Request);
        //    return Ok(getUserProfile);


        //}

        //[HttpPost]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> ResetPassword([FromForm] ResetPassword_VM model)
        //{

        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel resetPassword = await _auth.ResetPassword(model, uid);
        //    return Ok(resetPassword);

        //}



        //[HttpPut]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfile_VM model)
        //{
        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel updateProfile = await _auth.UpdateProfile(model, uid, Request);
        //    return Ok(updateProfile);

        //}

        //[HttpPost]
        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{

        //    ResponseModel confirmEmail = await _auth.ConfirmEmail(email, token);
        //    return Ok(confirmEmail);
        //}


        //[HttpPost]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> ResendConfirmEmail([FromForm] string email)
        //{

        //    ResponseModel resendEmail = await _auth.ResendConfirmEmail(email, Request);
        //    return Ok(resendEmail);

        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> GenerateOtp([FromForm] string phone)
        //{

        //    ResponseModel GenerateOtp = await _auth.GenerateOtp(phone);
        //    return Ok(GenerateOtp);


        //}


        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmOtp([FromForm] string otp, [FromForm] string phone)
        //{

        //    ResponseModel ConfirmOtp = await _auth.ConfirmOtp(otp, phone);
        //    return Ok(ConfirmOtp);

        //}



        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ResendOtp([FromForm] string phone)
        //{
        //    ResponseModel ResendOtp = await _auth.ResendOtp(phone);
        //    return Ok(ResendOtp);
        //}




        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmAccount([FromForm] string phone)
        //{

        //    ResponseModel ResendOtp = await _auth.ConfirmAccount(phone);
        //    return Ok(ResendOtp);
        //}


    }


}