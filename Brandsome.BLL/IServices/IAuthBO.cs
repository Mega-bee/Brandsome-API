using Brandsome.BLL.ViewModels;
using Brandsome.DAL.Data;
using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IAuthBO
    {
        //Task<CheckifExist_VM> CheckIfUserExists(string email, string phone);
        //Task<bool> CheckIfVerified(string email);
        //Task<ResponseModel> CompleteProfile(CompleteProfile_VM model, string uid, HttpRequest Request);
        //Task<ResponseModel> ConfirmAccount(string phonenumber);
        //Task<ResponseModel> ConfirmEmail(string email, string token);
        //Task<ResponseModel> ConfirmOtp(string otp, string phone);
        //Task<AccProfile> CreateProfile(EmailSignUp_VM model, int roleId);
        //Task<IdentityResult> CreateUser(EmailSignUp_VM model);
        //Task<ResponseModel> EmailSignIn(EmailSignIn_VM model);
        //Task<ResponseModel> EmailSignIn(EmailSignIn_VM model, HttpRequest Request);
        //Task<ResponseModel> ForgetPassword(string identifier, HttpRequest Request);
        //List<Claim> GenerateClaims(ApplicationUser res, AccProfileRole role);
        //string GenerateJWT(List<Claim> claims);
        //Task<ResponseModel> GenerateOtp(string phone);
        //Task<ResponseModel> GetUserProfile(string uid, HttpRequest Request);
        //Task<ResponseModel> ResendConfirmEmail(string email, HttpRequest Request);
        //Task<ResponseModel> ResendOtp(string phone);
        //Task<ResponseModel> ResetPassword(ResetPassword_VM model, string uid);
        //Task<ResponseModel> ResetPasswordFromEmail(ResetPasswordFromEmail_VM model);
        //Task<ResponseModel> SignUpWithEmail(EmailSignUp_VM model, int roleId, HttpRequest Request);
        //Task<ResponseModel> UpdateProfile(UpdateProfile_VM updatedProfile, string uid, HttpRequest Request);
    }
}