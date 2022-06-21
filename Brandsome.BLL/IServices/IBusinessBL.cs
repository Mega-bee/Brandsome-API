using Brandsome.BLL.ViewModels;
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
        Task<ResponseModel> FollowBusiness(string uid, int businessId);
        Task<ResponseModel> GetBusiness(string uid, int businessId);
        ResponseModel GetBusinsses(int serviceId, string sortBy);
        Task<ResponseModel> UpdateBusiness(CreateBusiness_VM business);
        Task<ResponseModel> UpdatePost(string uid, CreatePost_VM post);
    }
}