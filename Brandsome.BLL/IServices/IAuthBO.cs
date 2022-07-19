using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IAuthBO
    {
        Task<ResponseModel> CompleteProfile(CompleteProfile_VM profile, string uid, HttpRequest request);
        Task<ResponseModel> GetAccountSettings(string uid, HttpRequest request);
        Task<ResponseModel> RequestOtp(string phoneNumber, string userName);
        Task<ResponseModel> VerifyOtp(string phoneNumber, string otp,string uid);
        Task<ResponseModel> GetFollowedBusinesses(string uid, HttpRequest request);

        Task<ResponseModel> GetProfile(string uid, HttpRequest request);

        Task<ResponseModel> ResendOtp(string phoneNumber, string uid);
        Task<ResponseModel> RefreshFcmToken(string uid, string token);
        Task<ResponseModel> DeleteAccount(string uid);

    }
}