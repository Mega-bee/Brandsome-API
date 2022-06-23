using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IBusinessBL
    {
        Task<ResponseModel> AddReview(CreateReview_VM review, string uid);
        Task<ResponseModel> CreateBusiness(string uid, CreateBusiness_VM business);
        Task<ResponseModel> CreatePost(CreatePost_VM post);
        Task<ResponseModel> DeleteBusinessCity(string uid, int businessCityId);
        Task<ResponseModel> DeleteBusinessService(string uid, int businessServiceId);
        Task<ResponseModel> GetBusinessServices(int businessId, string uid);
        Task<ResponseModel> GetBusinessCities(int businessId, string uid);
        Task<ResponseModel> FollowBusiness(string uid, int businessId);
        Task<ResponseModel> GetBusiness(string uid, int businessId, HttpRequest request);
        Task<ResponseModel> GetBusinsses(List<int> services, string sortBy, HttpRequest request);
        Task<ResponseModel> UpdateBusiness(CreateBusiness_VM business);
        Task<ResponseModel> UpdatePost(string uid, CreatePost_VM post);
        Task<ResponseModel> DeleteBusiness(string uid, int businessId);
    }
}