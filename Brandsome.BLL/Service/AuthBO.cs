using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL.Models;
using Brandsome.DAL;
using Brandsome.DAL.Data;

namespace Brandsome.BLL.Service
{
    public class AuthBO : BaseBO, IAuthBO
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BrandsomeDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthBO(IUnitOfWork unit, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, BrandsomeDbContext context, NotificationHelper notificationHelper,RoleManager<IdentityRole> roleManager) : base(unit, mapper, notificationHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        private async Task CheckRoles()
        {
            if (!await _roleManager.RoleExistsAsync(AppSetting.UserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = AppSetting.UserRole, NormalizedName = AppSetting.UserRoleNormalized });
            }
            if (!await _roleManager.RoleExistsAsync(AppSetting.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = AppSetting.AdminRole, NormalizedName = AppSetting.AdminRoleNormalized });
            }
        }

        public async Task<ResponseModel> RequestOtp(string phoneNumber, string userName, string deviceToken)
        {
            ApplicationUser appUser = null;
            ResponseModel responseModel = new ResponseModel();
            string Otp = "";
            string content = $"Your OTP is {Otp}";
            await CheckRoles();
            var user = await _uow.UserRepository.GetAll().Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (user == null)
            {
                appUser = new ApplicationUser()
                {
                    PhoneNumber = phoneNumber,
                    DateOfBirth = DateTime.Now,
                    CreatedDate = DateTime.UtcNow,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    UserName = userName,


                };
               IdentityResult res= await _userManager.CreateAsync(appUser);
                if(res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, AppSetting.UserRole);
                     Otp = Helpers.Generate_otp();
                    appUser.Otp = Otp;
                    await _userManager.UpdateAsync(appUser);
                     content = $"Your OTP is {Otp}";
                    Helpers.SendSMS(phoneNumber, content);
                    responseModel.Data = new DataModel { Data = "", Message = "OTP has been sent to your phone number"};
                    responseModel.StatusCode = 200;
                    responseModel.ErrorMessage = "";
                    return responseModel;
                } else
                {
                    responseModel.Data = new DataModel { Data = "", Message = "" };
                    responseModel.StatusCode = 400;
                    responseModel.ErrorMessage = res.Errors.FirstOrDefault().Description;
                    return responseModel;
                }
            }
            Otp = user.Otp;
            Helpers.SendSMS(phoneNumber, content);
            responseModel.Data = new DataModel { Data = "", Message = "OTP has been sent to your phone number" };
            return responseModel;
        }

        public async Task<ResponseModel> VerifyOtp(string phoneNumber,string otp)
        {
            ResponseModel responseModel = new ResponseModel();
            ApplicationUser appUser = new ApplicationUser();
            var user = await _uow.UserRepository.GetAll().Where(x => x.PhoneNumber == phoneNumber).Select(x=> new { x.Id, x.Otp}).FirstOrDefaultAsync();
            if(user == null)
            {
                responseModel.Data = new DataModel { Data = "", Message = "" };
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "User not found";
            } 
            if(user.Otp == otp)
            {
                appUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(appUser);
                var claims = Tools.GenerateClaims(appUser,roles);
                string JwtToken = Tools.GenerateJWT(claims);
                responseModel.Data = new DataModel { Data = JwtToken, Message = "Phone number verified sucessfully" };
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 200;
                return responseModel;
            }
            else
            {
                responseModel.Data = new DataModel { Data = "", Message = "" };
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "Incorrect otp";
                return responseModel;
            }

        }

        public async Task<ResponseModel> CompleteProfile(CompleteProfile_VM profile,string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            var user = await _userManager.FindByIdAsync(uid);
            if(user == null)
            {
                responseModel.Data = new DataModel { Data = "", Message = "" };
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "User not found";
                return responseModel;
            }
            user.DateOfBirth = profile.Birthday ?? null;
            user.GenderId = profile.GenderId ?? null;
            IFormFile file = profile.ImageFile;
            if(file != null)
            {
                //if(user.Image != null)
                //{
                //    Helpers.DeleteFile(user.Image, "wwwroot/uploads");
                //}
                string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);

                user.Image = NewFileName;
            }
           IdentityResult updateResult =  await _userManager.UpdateAsync(user);
            if(!updateResult.Succeeded)
            {
                responseModel.ErrorMessage = "Failed to update user info";
                responseModel.StatusCode = 400;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Profile completed" };
            return responseModel;
        }

        public async Task<ResponseModel> GetAccountSettings(string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            AccountSettings_VM settings = await _uow.UserRepository.GetAll().Where(x=> x.Id == uid).Select(x=> new AccountSettings_VM
            {
                 Businesses = x.Businesses.Select(b => new AccountSettingsBusiness_VM
                 {
                      Id = b.Id,
                       Name = b.BusinessName
                 }).ToList(),
            }).FirstOrDefaultAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = settings, Message = "" };
            return responseModel;
        }

        //public async Task<ResponseModel> GetFollowedBusinesses(string uid)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    List<FollowedBusiness_VM> businesses = await _uow.BusinessFollowRepository.GetAll().Where(x => x.UserId == uid).Select(bf => new FollowedBusiness_VM
        //    {
        //        Id = bf.Id,
        //        Image = bf.Business.Image,
        //        Name = bf.Business.BusinessName,
        //        Type = bf.Business.
        //     })
        //}
        //        public async Task<ResponseModel> EmailSignIn(EmailSignIn_VM model)
        //        {
        //            ResponseModel responseModel = new ResponseModel();
        //            ApplicationUser res = await _userManager.FindByEmailAsync(model.Email);
        //            if (res == null)
        //            {
        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "User Doesn't Exist";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            var pass = await _userManager.CheckPasswordAsync(res, model.Password);
        //            if (!pass)
        //            {
        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Username/Password Combination is Not Correct";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            var signIn = await _signInManager.PasswordSignInAsync(res, model.Password, isPersistent: true, lockoutOnFailure: false);
        //            if (!signIn.Succeeded)
        //            {
        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Sign In Failed";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            AccProfile profile = _uow.ProfileRepos.GetByIdWithPredicateAndIncludes(x => x.Email == res.Email && x.IsDeleted == false, x => x.Role);
        //            if (profile == null)
        //            {
        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Failed To Fetch Profile";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            if (!string.IsNullOrEmpty(model.DeviceToken))
        //            {
        //                bool deviceTokenExists = await _context.UserDevicetokens.Where(x => x.UserId == res.Id && x.Token == model.DeviceToken).FirstOrDefaultAsync() != null;
        //                if (!deviceTokenExists)
        //                {
        //                    var newDeviceToken = new UserDevicetoken()
        //                    {
        //                        Token = model.DeviceToken,
        //                        UserId = res.Id
        //                    };
        //                    await _context.UserDevicetokens.AddAsync(newDeviceToken);
        //                    await _context.SaveChangesAsync();
        //                }
        //            }

        //            var claims = Tools.GenerateClaims(res, profile.Role);
        //            string JwtToken = Tools.GenerateJWT(claims);
        //            User_VM user = new User_VM()
        //            {
        //                Id = profile.Id,
        //                Name = profile.Name,
        //                Token = JwtToken
        //            };
        //            responseModel.StatusCode = 200;
        //            responseModel.ErrorMessage = "";
        //            responseModel.Data = new DataModel
        //            {
        //                Data = user,
        //                Message = ""
        //            };
        //            return responseModel;
        //        }

        //        //public async Task<ResponseModel> ForgetPassword(string identifier, HttpRequest Request)
        //        //{

        //        //    ResponseModel responseModel = new ResponseModel();
        //        //    var emailUser = await _userManager.FindByEmailAsync(identifier);
        //        //    if (emailUser != null)
        //        //    {
        //        //        var token = await _userManager.GeneratePasswordResetTokenAsync(emailUser);
        //        //        var encodedToken = Encoding.UTF8.GetBytes(token);
        //        //        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        //        //        string url = $"{Request.Scheme}://{Request.Host}/ResetPassword?email={identifier}&token={validToken}";

        //        //        bool isEmailSent = await Tools.SendEmailAsync(identifier, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
        //        //             $"<p>To reset your password <a href='{url}'>Click here</a></p>");
        //        //        if (isEmailSent)
        //        //        {
        //        //            responseModel.StatusCode = 200;
        //        //            responseModel.ErrorMessage = "";
        //        //            responseModel.Data = new DataModel
        //        //            {
        //        //                Data = "",
        //        //                Message = "Details to reset password have been sent to email"
        //        //            };
        //        //            return responseModel;
        //        //        }
        //        //        responseModel.StatusCode = 500;
        //        //        responseModel.ErrorMessage = "Failed To Send Email";
        //        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        //        return responseModel;
        //        //    }

        //        //    responseModel.StatusCode = 404;
        //        //    responseModel.ErrorMessage = "You will receive an email if your user is registered with Sentinel";
        //        //    responseModel.Data = new DataModel { Data = "", Message = "" };
        //        //    return responseModel;
        //        //}

        //        public async Task<ResponseModel> CompleteProfile(CompleteProfile_VM model, string uid, HttpRequest Request)
        //        {

        //            try
        //            {
        //                AspNetUser aspuser = _context.AspNetUsers.Where(x => x.Id == uid).FirstOrDefault();
        //                AccProfile user = _context.AccProfiles.Include(x => x.Gender).Include(x => x.Role).Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefault();
        //                ResponseModel responseModel = new ResponseModel();

        //                if (aspuser == null)
        //                {
        //                    responseModel.StatusCode = 404;
        //                    responseModel.ErrorMessage = "User was not Found";
        //                    responseModel.Data = new DataModel { Data = "", Message = "" };
        //                    return responseModel;
        //                }

        //                if (!string.IsNullOrEmpty(model.PhoneNumber))
        //                {
        //                    aspuser.PhoneNumber = model.PhoneNumber;
        //                    user.PhoneNumber = model.PhoneNumber;
        //                }
        //                if (!string.IsNullOrEmpty(model.Name))
        //                {
        //                    user.Name = model.Name;
        //                }
        //                if (model.Birthdate != default)
        //                {
        //                    user.BirthDate = model.Birthdate;
        //                }
        //                else
        //                {
        //                    user.BirthDate = new DateTime();
        //                }
        //                if (model.GenderId != default)
        //                {
        //                    user.GenderId = model.GenderId;
        //                }

        //                IFormFile file = model.ImageFile;
        //                if (file != null)
        //                {
        //                    string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);

        //                    user.ImageUrl = NewFileName;
        //                }
        //                else
        //                {
        //                    user.ImageUrl = "user-placeholder.png";
        //                }

        //                await _context.SaveChangesAsync();

        //                Profile_VM userProfile = new Profile_VM()
        //                {
        //                    Id = user.Id,
        //                    Name = user.Name ?? "",
        //                    Email = user.Email ?? "",
        //                    BirthDate = (DateTime)user.BirthDate != default ? (DateTime)user.BirthDate : new DateTime(),
        //                    GenderId = (int)user.GenderId,
        //                    Gender = user.Gender.Name ?? "",
        //                    ImageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{user.ImageUrl}",
        //                    PhoneNumber = user.PhoneNumber ?? "",
        //                    Role = user.Role.RoleName ?? "",
        //                    Token = "",

        //                };

        //                if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.PhoneNumber) || user.BirthDate == default || user.GenderId == default)
        //                {
        //                    responseModel.StatusCode = 200;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel
        //                    {
        //                        Data = userProfile,
        //                        Message = ""
        //                    };
        //                    return responseModel;
        //                }
        //                if (user.BirthDate != new DateTime())
        //                {
        //                    await _context.SaveChangesAsync();
        //                }

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = userProfile,
        //                    Message = ""
        //                };
        //                return responseModel;

        //            }
        //            catch (Exception ex)
        //            {
        //                ResponseModel responseModel = new ResponseModel();
        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = ex.ToString();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;
        //            }

        //        }


        //        public async Task<ResponseModel> SignUpWithEmail(EmailSignUp_VM model, int roleId, HttpRequest Request)
        //        {

        //            ResponseModel responseModel = new ResponseModel();
        //            bool isVerified = false;
        //            //EmailSignUpResponse_VM resp = new EmailSignUpResponse_VM();
        //            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword) || string.IsNullOrEmpty(model.PhoneNumber))
        //            {
        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Some Fields Are Missing";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = ""
        //                };
        //                return responseModel;
        //            }

        //            if (model.Password != model.ConfirmPassword)
        //            {
        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Passwords Don't Match";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = ""
        //                };
        //                return responseModel;
        //            }

        //            CheckifExist_VM obj = await CheckIfUserExists(model.Email, model.PhoneNumber);


        //            if (obj.Phone)
        //            {

        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Phone Number Already Exists";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = ""
        //                };
        //                return responseModel;
        //            }


        //            if (obj.Email)
        //            {
        //                //isVerified = await CheckIfVerified(model.Email);
        //                //if(!isVerified)
        //                //{
        //                //    await GenerateOtp(model.PhoneNumber);
        //                //}




        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Email Already Exists";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = ""
        //                };
        //                return responseModel;
        //            }


        //            IdentityResult res = await CreateUser(model);

        //            if (!res.Succeeded)
        //            {
        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = res.Errors.FirstOrDefault().Description;
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = ""
        //                };
        //            }




        //            AccProfile usersProfile = await CreateProfile(model, roleId);
        //            var newProfile = _context.AccProfiles.Include(x => x.Gender).Where(x => x.Id == usersProfile.Id).FirstOrDefault();
        //            AccProfileRole role = await _context.AccProfileRoles.FirstOrDefaultAsync(x => x.Id == roleId);
        //            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);



        //            List<Claim> claims = GenerateClaims(user, role);

        //            // generate jwt
        //            string JwtToken = GenerateJWT(claims);


        //            //resp.Email = user.Email;
        //            //resp.Token = JwtToken;

        //            Profile_VM userProfile = new Profile_VM()
        //            {
        //                Id = newProfile.Id,
        //                Name = newProfile.Name ?? "",
        //                Email = newProfile.Email ?? "",
        //                BirthDate = (DateTime)newProfile.BirthDate != default ? (DateTime)newProfile.BirthDate : new DateTime(),
        //                GenderId = (int)newProfile.GenderId,
        //                Gender = newProfile.Gender.Name ?? "",
        //                ImageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{newProfile.ImageUrl}",
        //                PhoneNumber = user.PhoneNumber ?? "",
        //                Role = newProfile.Role.RoleName ?? "",
        //                Token = JwtToken,
        //            };

        //            responseModel.StatusCode = 200;
        //            responseModel.ErrorMessage = "User Succesfully Created";
        //            responseModel.Data = new DataModel
        //            {
        //                Data = userProfile,
        //                Message = ""
        //            };
        //            return responseModel;
        //        }

        //        public async Task<CheckifExist_VM> CheckIfUserExists(string email, string phone)
        //        {
        //            CheckifExist_VM checkifExist_VM = new CheckifExist_VM();

        //            var accuser = _context.AccProfiles.Where(x => x.PhoneNumber == phone).FirstOrDefault();
        //            if (accuser != null)
        //                checkifExist_VM.Phone = true;
        //            ApplicationUser oldUser = await _userManager.FindByEmailAsync(email);
        //            if (oldUser != null)
        //                checkifExist_VM.Email = true;
        //            return checkifExist_VM;
        //        }

        //        public async Task<IdentityResult> CreateUser(EmailSignUp_VM model)
        //        {
        //            ApplicationUser user = new ApplicationUser();

        //            if (!string.IsNullOrEmpty(model.PhoneNumber))
        //            {
        //                user.PhoneNumber = model.PhoneNumber;
        //            }
        //            if (!string.IsNullOrEmpty(model.Email))
        //            {
        //                user.Email = model.Email;
        //                user.NormalizedEmail = model.Email;
        //                user.UserName = model.Email;
        //                user.NormalizedUserName = model.Email;

        //            }

        //            //if (!string.IsNullOrEmpty(model.Name))
        //            //{


        //            //}
        //            if (model.Birthdate != default)
        //            {
        //                user.BirthDate = model.Birthdate;
        //            }
        //            else
        //            {
        //                user.BirthDate = new DateTime();
        //            }
        //            if (model.GenderId != default)
        //            {
        //                user.GenderId = model.GenderId;
        //            }
        //            //if (!string.IsNullOrEmpty(model.DeviceToken))
        //            //{
        //            //    bool deviceTokenExists = await _context.UserDeviceTokens.Where(x => x.UserId == user.Id && x.Token == model.DeviceToken).FirstOrDefaultAsync() != null;
        //            //    if (!deviceTokenExists)
        //            //    {
        //            //        var newDeviceToken = new UserDeviceToken()
        //            //        {
        //            //            Token = model.DeviceToken,
        //            //            UserId = user.Id
        //            //        };
        //            //        await _context.UserDeviceTokens.AddAsync(newDeviceToken);
        //            //        await _context.SaveChangesAsync();
        //            //    }
        //            //}
        //            user.EmailConfirmed = true;
        //            user.PhoneNumberConfirmed = false;

        //            IdentityResult res = await _userManager.CreateAsync(user, model.Password);
        //            // check if user creation succeeded
        //            return res;

        //        }

        //        public async Task<ResponseModel> EmailSignIn(EmailSignIn_VM model, HttpRequest Request)
        //        {
        //            ResponseModel responseModel = new ResponseModel();


        //            AspNetUser aspres = null;
        //            ApplicationUser res = null;
        //            if (!model.Email.Contains('@'))
        //            {
        //                model.Email = Helpers.RemoveCountryCode(model.Email);
        //            }


        //            if (!string.IsNullOrEmpty(model.Email))
        //            {
        //                aspres = await _context.AspNetUsers.Where(x => x.Email == model.Email || x.PhoneNumber == model.Email).FirstOrDefaultAsync();
        //            }

        //            if (aspres == null)
        //            {



        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "User Doesn't Exist";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            res = await _userManager.FindByIdAsync(aspres.Id);

        //            //ApplicationUser res = await _userManager.FindByEmailAsync(model.identifier);
        //            bool isVerified = false;
        //            if (res == null)
        //            {



        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "User Doesn't Exist";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            var pass = await _userManager.CheckPasswordAsync(res, model.Password);
        //            if (!pass)
        //            {



        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Password Is Incorrect";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            var signIn = await _signInManager.PasswordSignInAsync(res, model.Password, isPersistent: true, lockoutOnFailure: false);
        //            if (!signIn.Succeeded)
        //            {
        //                if (signIn.IsNotAllowed)
        //                {



        //                    responseModel.StatusCode = 500;
        //                    responseModel.ErrorMessage = "Sign In Failed, Email Is Not Verified";
        //                    responseModel.Data = new DataModel { Data = "", Message = "" };
        //                    return responseModel;
        //                }



        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Sign In Failed";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            isVerified = await _userManager.IsEmailConfirmedAsync(res);

        //            AccProfile profile = await _context.AccProfiles.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == res.Email && x.IsDeleted == false);
        //            if (profile == null)
        //            {



        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Failed To Fetch Profile";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            if (!string.IsNullOrEmpty(model.DeviceToken))
        //            {
        //                res.DeviceToken = model.DeviceToken;
        //                IdentityResult result = await _userManager.UpdateAsync(res);
        //                if (!result.Succeeded)
        //                {


        //                    responseModel.StatusCode = 500;
        //                    responseModel.ErrorMessage = "Failed to Update Device Token";
        //                    responseModel.Data = new DataModel { Data = "", Message = "" };
        //                    return responseModel;
        //                }
        //            }

        //            //if (!string.IsNullOrEmpty(model.DeviceToken))
        //            //{
        //            //    bool deviceTokenExists = await _context.UserDeviceTokens.Where(x => x.UserId == res.Id && x.Token == model.DeviceToken).FirstOrDefaultAsync() != null;
        //            //    if (!deviceTokenExists)
        //            //    {
        //            //        var newDeviceToken = new UserDeviceToken()
        //            //        {
        //            //            Token = model.DeviceToken,
        //            //            UserId = res.Id
        //            //        };
        //            //        await _context.UserDeviceTokens.AddAsync(newDeviceToken);
        //            //        await _context.SaveChangesAsync();
        //            //    }
        //            //}

        //            var claims = GenerateClaims(res, profile.Role);
        //            string JwtToken = GenerateJWT(claims);
        //            Profile_VM userProfile = await _context.AccProfiles.Where(x => x.Id == profile.Id).Select(x => new Profile_VM
        //            {
        //                Email = x.Email,
        //                ImageUrl = x.ImageUrl != null ? $"{Request.Scheme}://{Request.Host}/Uploads/{x.ImageUrl}" : $"{Request.Scheme}://{Request.Host}/Uploads/user-placeholder.png",
        //                Role = x.Role.RoleName,
        //                Token = JwtToken,
        //                Id = x.Id,
        //                PhoneNumber = x.PhoneNumber,
        //                //OrderListId= x.OrderLists.FirstOrDefault().Id,
        //                Name = x.Name,
        //                BirthDate = x.BirthDate.HasValue != false ? x.BirthDate.Value : default,
        //                Gender = x.Gender.Name ?? "Not specified",
        //                GenderId = x.GenderId.HasValue ? (int)x.GenderId : 0,
        //                //IsProfileComplete= (bool)x.IsProfileComplete,
        //            }).FirstOrDefaultAsync();

        //            responseModel.StatusCode = 200;
        //            responseModel.ErrorMessage = "";
        //            responseModel.Data = new DataModel
        //            {
        //                Data = userProfile,
        //                Message = ""
        //            };
        //            return responseModel;
        //        }

        //        public List<Claim> GenerateClaims(ApplicationUser res, AccProfileRole role)
        //        {
        //            var claims = new List<Claim>()
        //                {
        //                new Claim(JwtRegisteredClaimNames.Email , res.Email ),
        //                new Claim(ClaimTypes.Name , res.UserName),
        //                new Claim("UID",res.Id),
        //                new Claim(ClaimTypes.Role , role.RoleName),
        //                new Claim(ClaimTypes.NameIdentifier, res.Id),
        //                };
        //            return claims;
        //        }

        //        public string GenerateJWT(List<Claim> claims)
        //        {
        //            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fdjfhjehfjhfuehfbhvdbvjjoq8327483rgh"));
        //            JwtSecurityToken token = new JwtSecurityToken(
        //               issuer: "https://localhost:44310",
        //               audience: "https://localhost:44310",
        //               claims: claims,
        //               notBefore: DateTime.Now,
        //               expires: DateTime.Now.AddYears(1),
        //               signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        //               );
        //            string JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        //            return JwtToken;
        //        }

        //        public async Task<ResponseModel> GetUserProfile(string uid, HttpRequest Request)
        //        {
        //            try
        //            {
        //                ResponseModel responseModel = new ResponseModel();
        //                bool isConfirmed = _context.AspNetUsers.Where(x => x.Id == uid).FirstOrDefault().EmailConfirmed;

        //                Profile_VM user = await _context.AccProfiles.Where(x => x.UserId == uid && x.IsDeleted == false)
        //                           .Select(x => new Profile_VM
        //                           {
        //                               Id = x.Id,
        //                               Name = x.Name ?? "",
        //                               Email = x.Email ?? "",
        //                               Gender = x.Gender.Name ?? "",
        //                               GenderId = x.GenderId.Value,
        //                               ImageUrl = x.ImageUrl != null ? $"{Request.Scheme}://{Request.Host}/Uploads/{x.ImageUrl}" : $"{Request.Scheme}://{Request.Host}/Uploads/user-placeholder.png",
        //                               BirthDate = x.BirthDate != null ? x.BirthDate.Value : DateTime.Now,
        //                               PhoneNumber = x.PhoneNumber ?? "",
        //                               Role = x.Role.RoleName,
        //                               Token = "",
        //                           })
        //                           .FirstOrDefaultAsync();

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = user,
        //                    Message = ""
        //                };
        //                return responseModel;
        //            }
        //            catch (Exception ex)
        //            {
        //                ResponseModel responseModel = new ResponseModel();



        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Something Went Wrong";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;
        //            }

        //        }



        //        public async Task<ResponseModel> ResetPassword(ResetPassword_VM model, string uid)
        //        {
        //            var user = await _userManager.FindByIdAsync(uid);
        //            ResponseModel responseModel = new ResponseModel();
        //            if (user == null)
        //            {



        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "User Doesn't Exist";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }


        //            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
        //            {

        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Old Password Is Invalid";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            if (model.NewPassword == model.OldPassword)
        //            {

        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "New password can't be the same as old password";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            if (model.NewPassword != model.ConfirmPassword)
        //            {
        //                responseModel.StatusCode = 400;
        //                responseModel.ErrorMessage = "Passwords don't match";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var encodedToken = Encoding.UTF8.GetBytes(token);
        //            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
        //            var decodedToken = WebEncoders.Base64UrlDecode(validToken);
        //            string normalToken = Encoding.UTF8.GetString(decodedToken);

        //            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = "Password has been reset succesfully"
        //                };
        //                return responseModel;
        //            }

        //            responseModel.StatusCode = 400;
        //            responseModel.ErrorMessage = result.Errors.Select(e => e.Description).FirstOrDefault();
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;

        //        }

        //        public async Task<ResponseModel> ForgetPassword(string identifier, HttpRequest Request)
        //        {

        //            ResponseModel responseModel = new ResponseModel();
        //            var emailUser = await _userManager.FindByEmailAsync(identifier);
        //            if (emailUser != null)
        //            {
        //                var token = await _userManager.GeneratePasswordResetTokenAsync(emailUser);
        //                var encodedToken = Encoding.UTF8.GetBytes(token);
        //                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        //                string url = $"{Request.Scheme}://{Request.Host}/ResetPassword?email={identifier}&token={validToken}";

        //                bool isEmailSent = await Helpers.SendEmailAsync(identifier, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
        //                     $"<p>To reset your password <a href='{url}'>Click here</a></p>");
        //                if (isEmailSent)
        //                {
        //                    responseModel.StatusCode = 200;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel
        //                    {
        //                        Data = "",
        //                        Message = "Details to reset password have been sent to email"
        //                    };
        //                    return responseModel;
        //                }

        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Failed To Send Email";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }

        //            responseModel.StatusCode = 404;
        //            responseModel.ErrorMessage = "User Not Found";
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;
        //        }

        //        public async Task<ResponseModel> ResetPasswordFromEmail(ResetPasswordFromEmail_VM model)
        //        {
        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            ResponseModel responseModel = new ResponseModel();
        //            if (user == null)
        //            {

        //                responseModel.StatusCode = 404;
        //                responseModel.ErrorMessage = "User Not Found";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }


        //            if (model.NewPassword != model.ConfirmPassword)
        //            {

        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "Passwords Don't Match";
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }


        //            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
        //            string normalToken = Encoding.UTF8.GetString(decodedToken);

        //            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

        //            if (result.Succeeded)
        //            {

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = "Password has been reset succesfully"
        //                };
        //                return responseModel;
        //            }

        //            responseModel.StatusCode = 400;
        //            responseModel.ErrorMessage = result.Errors.Select(e => e.Description).FirstOrDefault();
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;

        //        }

        //        public async Task<ResponseModel> UpdateProfile(UpdateProfile_VM updatedProfile, string uid, HttpRequest Request)
        //        {
        //            ResponseModel responseModel = new ResponseModel();
        //            AccProfile user = await _context.AccProfiles.Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefaultAsync();

        //            if (user != null)
        //            {
        //                ApplicationUser res = await _userManager.FindByIdAsync(user.UserId);
        //                if (!string.IsNullOrEmpty(updatedProfile.PhoneNumber))
        //                {
        //                    res.PhoneNumber = updatedProfile.PhoneNumber;
        //                    user.PhoneNumber = updatedProfile.PhoneNumber;
        //                }
        //                if (!string.IsNullOrEmpty(updatedProfile.Name))
        //                {
        //                    user.Name = updatedProfile.Name;
        //                }

        //                // res.GenderId = updatedProfile.GenderId != 0 ? updatedProfile.GenderId : 1;
        //                // user.GenderId = updatedProfile.GenderId != 0 ? updatedProfile.GenderId : 1;
        //                //if (updatedProfile.Email != null)
        //                //{
        //                //    user.Email = updatedProfile.Email;
        //                //    res.UserName = updatedProfile.Email;
        //                //    res.Email = updatedProfile.Email;
        //                //}


        //                if (updatedProfile.BirthDate != new DateTime())
        //                {
        //                    user.BirthDate = updatedProfile.BirthDate;
        //                    res.BirthDate = updatedProfile.BirthDate;
        //                }


        //                IFormFile file = updatedProfile.ImageFile;
        //                if (file != null)
        //                {
        //                    string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);
        //                    user.ImageUrl = NewFileName;
        //                }
        //                IdentityResult result = await _userManager.UpdateAsync(res);
        //                if (!result.Succeeded)
        //                {
        //                    responseModel.StatusCode = 500;
        //                    //responseModel.ErrorMessage = result.Errors.Select(e => e.Description).FirstOrDefault();
        //                    responseModel.ErrorMessage = "User was not Updated";
        //                    responseModel.Data = new DataModel { Data = "", Message = "" };
        //                    return responseModel;
        //                }
        //                await _context.SaveChangesAsync();


        //                string _Email = user.Email;


        //                Profile_VM userProfile = await _context.AccProfiles.Where(x => x.UserId == uid).Select(x =>
        //                new Profile_VM
        //                {
        //                    Email = x.Email ?? "",
        //                    ImageUrl = x.ImageUrl != null ? $"{Request.Scheme}://{Request.Host}/Uploads/{x.ImageUrl}" : $"{Request.Scheme}://{Request.Host}/Uploads/user-placeholder.png",
        //                    Role = x.Role.RoleName ?? "Not set",
        //                    Id = x.Id,
        //                    PhoneNumber = x.PhoneNumber ?? "Not Set",
        //                    Name = x.Name ?? "Not Set",
        //                    BirthDate = (DateTime)x.BirthDate,
        //                    Gender = x.Gender.Name ?? "",
        //                    GenderId = (int)x.GenderId,
        //                    Token = "",

        //                }).FirstOrDefaultAsync();

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = userProfile,
        //                    Message = ""
        //                };
        //                return responseModel;

        //            }

        //            responseModel.StatusCode = 404;
        //            responseModel.ErrorMessage = "User Not Found";
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;
        //        }

        //        public async Task<ResponseModel> ConfirmEmail(string email, string token)
        //        {
        //            ResponseModel responseModel = new ResponseModel();
        //            try
        //            {
        //                ApplicationUser user = await _userManager.FindByEmailAsync(email);
        //                IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        //                if (result.Succeeded)
        //                {

        //                    responseModel.StatusCode = 200;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel
        //                    {
        //                        Data = "Email Confirmed",
        //                        Message = ""
        //                    };
        //                    return responseModel;
        //                }

        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = result.Errors.Select(x => x.Description).FirstOrDefault();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //            }
        //            catch (Exception ex)
        //            {
        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = ex.ToString();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;
        //            }

        //        }

        //        public async Task<ResponseModel> ResendConfirmEmail(string email, HttpRequest Request)
        //        {
        //            try
        //            {
        //                ApplicationUser user = await _userManager.FindByEmailAsync(email);

        //                ResponseModel responseModel = new ResponseModel();

        //                var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));

        //                string url = $"{Request.Scheme}://{Request.Host}/api/Accounts/ConfirmEmail?token=" + token + "&email=" + user.Email;
        //                await Helpers.SendEmailAsync(user.Email, "Confirm Email", "<h1>Follow the instructions to confirm your email</h1>" +
        //                     $"<p> <a href='{url}'>Click here</a></p>");

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "Email Sent",
        //                    Message = ""
        //                };
        //                return responseModel;


        //            }
        //            catch (Exception)
        //            {
        //                ResponseModel responseModel = new ResponseModel();


        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "Some Thing Went Wrong",
        //                    Message = ""
        //                };
        //                return responseModel;

        //                throw;
        //            }

        //        }

        //        public async Task<ResponseModel> GenerateOtp(string phone)
        //        {
        //            ResponseModel responseModel = new ResponseModel();
        //            try
        //            {
        //                phone = Helpers.RemoveCountryCode(phone);

        //                var currphoneOtp = await _context.PhoneOtps.Where(x => x.PhoneNumber == phone).FirstOrDefaultAsync();
        //                if (currphoneOtp != null)
        //                {
        //                    var resend = await ResendOtp(phone);

        //                    responseModel.StatusCode = 200;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel
        //                    {
        //                        Data = "",
        //                        Message = "Otp has been Sent again to phone"
        //                    };
        //                    return responseModel;
        //                }
        //                var otp = Helpers.Generate_otp();

        //                PhoneOtp phoneOtp = new PhoneOtp()
        //                {
        //                    Otp = otp,
        //                    PhoneNumber = phone
        //                };

        //                await _context.PhoneOtps.AddAsync(phoneOtp);
        //                await _context.SaveChangesAsync();

        //                string content = $"your One-Time Password is : {otp}";

        //                Helpers.SendSMS(phone, content);

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = "Otp has been Sent to phone"
        //                };
        //                return responseModel;
        //            }
        //            catch (Exception ex)
        //            {

        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = ex.ToString();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;

        //            }


        //        }

        //        public async Task<ResponseModel> ConfirmOtp(string otp, string phone)
        //        {
        //            ResponseModel responseModel = new ResponseModel();
        //            try
        //            {
        //                phone = Helpers.RemoveCountryCode(phone);

        //                var phoneOtp = await _context.PhoneOtps.Where(x => x.PhoneNumber == phone).FirstOrDefaultAsync();

        //                int dbotp = Convert.ToInt32(phoneOtp.Otp);
        //                int useotp = Convert.ToInt32(otp);

        //                if (dbotp == useotp)
        //                {
        //                    responseModel.StatusCode = 200;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel
        //                    {
        //                        Data = true,
        //                        Message = "Otp ConfirmOtp"
        //                    };
        //                    return responseModel;
        //                }

        //                else
        //                {
        //                    // generate otp again
        //                    var newotp = Helpers.Generate_otp();
        //                    phoneOtp.Otp = newotp;
        //                    responseModel.StatusCode = 401;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel
        //                    {
        //                        Data = "",
        //                        Message = "Otp is Incorrect"
        //                    };
        //                    return responseModel;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = ex.ToString();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;

        //            }


        //        }

        //        public async Task<AccProfile> CreateProfile(EmailSignUp_VM model, int roleId)
        //        {
        //            // create user profile and add role based on roleId
        //            AccProfile newProfile = new AccProfile();
        //            newProfile.RoleId = roleId;
        //            newProfile.UserId = _context.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault().Id;
        //            if (!string.IsNullOrEmpty(model.PhoneNumber))
        //            {
        //                newProfile.PhoneNumber = model.PhoneNumber;
        //            }
        //            if (!string.IsNullOrEmpty(model.Email))
        //            {
        //                newProfile.Email = model.Email;
        //            }

        //            if (!string.IsNullOrEmpty(model.Name))
        //            {
        //                newProfile.Name = model.Name;

        //            }
        //            if (model.Birthdate != default)
        //            {
        //                newProfile.BirthDate = model.Birthdate;
        //            }
        //            else
        //            {
        //                newProfile.BirthDate = new DateTime();
        //            }
        //            if (model.GenderId != default)
        //            {
        //                newProfile.GenderId = model.GenderId;
        //            }
        //            IFormFile file = model.ImageFile;
        //            if (file != null)
        //            {
        //                string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);

        //                newProfile.ImageUrl = NewFileName;
        //            }
        //            else
        //            {
        //                newProfile.ImageUrl = "user-placeholder.png";
        //            }

        //            newProfile.DateCreated = DateTime.UtcNow;
        //            newProfile.IsDeleted = false;


        //            // check if user sent profile picture
        //            await _context.AccProfiles.AddAsync(newProfile);
        //            await _context.SaveChangesAsync();
        //            return newProfile;
        //        }
        //        public async Task<ResponseModel> ResendOtp(string phone)
        //        {
        //            ResponseModel responseModel = new ResponseModel();

        //            try
        //            {

        //                phone = Helpers.RemoveCountryCode(phone);


        //                var phoneOtp = await _context.PhoneOtps.Where(x => x.PhoneNumber == phone).FirstOrDefaultAsync();


        //                var otp = Helpers.Generate_otp();

        //                phoneOtp.Otp = otp;

        //                await _context.SaveChangesAsync();
        //                string content = $"your One-Time Password is : {otp}";
        //                Helpers.SendSMS(phone, content);

        //                responseModel.StatusCode = 200;
        //                responseModel.ErrorMessage = "";
        //                responseModel.Data = new DataModel
        //                {
        //                    Data = "",
        //                    Message = "Otp has been Resent to phone"
        //                };
        //                return responseModel;
        //            }
        //            catch (Exception ex)
        //            {
        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = ex.ToString();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;

        //            }

        //        }


        //        public async Task<ResponseModel> ConfirmAccount(string phonenumber)
        //        {
        //            ResponseModel responseModel = new ResponseModel();
        //            try
        //            {
        //                phonenumber = Helpers.RemoveCountryCode(phonenumber);

        //                var aspuser = _context.AspNetUsers.Where(x => x.PhoneNumber == phonenumber).FirstOrDefault();

        //                if (aspuser == null)
        //                {
        //                    responseModel.StatusCode = 404;
        //                    responseModel.ErrorMessage = "User was not Found";
        //                    responseModel.Data = new DataModel { Data = "", Message = "" };
        //                    return responseModel;
        //                }
        //                else
        //                {
        //                    aspuser.EmailConfirmed = true;
        //                    aspuser.PhoneNumberConfirmed = true;
        //                    await _context.SaveChangesAsync();




        //                    responseModel.StatusCode = 200;
        //                    responseModel.ErrorMessage = "";
        //                    responseModel.Data = new DataModel { Data = true, Message = "" };
        //                    return responseModel;
        //                }
        //            }
        //            catch (Exception ex)
        //            {



        //                responseModel.StatusCode = 500;
        //                responseModel.ErrorMessage = ex.ToString();
        //                responseModel.Data = new DataModel { Data = "", Message = "" };
        //                return responseModel;
        //                throw;
        //            }

        //        }

        //        public async Task<bool> CheckIfVerified(string email)
        //        {
        //            try
        //            {
        //                bool isVerified = false;
        //                if (string.IsNullOrEmpty(email))
        //                {
        //                    return isVerified;
        //                }
        //                else
        //                {
        //                    var user = await _userManager.FindByEmailAsync(email);
        //                    if (user == null)
        //                    {
        //                        return isVerified;
        //                    }
        //                    isVerified = user.PhoneNumberConfirmed;
        //                    return isVerified;
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //                throw;
        //            }
        //        }

    }
}
