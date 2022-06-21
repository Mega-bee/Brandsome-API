using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IAuthBO
    {
        Task<ResponseModel> CompleteProfile(CompleteProfile_VM profile, string uid, HttpRequest request);
        Task<ResponseModel> GetAccountSettings(string uid);
        Task<ResponseModel> RequestOtp(string phoneNumber, string userName, string deviceToken);
        Task<ResponseModel> VerifyOtp(string phoneNumber, string otp);
        Task<ResponseModel> GetFollowedBusinesses(string uid, HttpRequest request);

        Task<ResponseModel> GetProfile(string uid, HttpRequest request);

        Task<ResponseModel> ResendOtp(string phoneNumber);
    }
}