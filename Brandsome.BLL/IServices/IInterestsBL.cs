using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IInterestsBL
    {
        Task<ResponseModel> GetCategories();
        Task<ResponseModel> GetServices(int subcategoryId);
        Task<ResponseModel> GetSubCategories(int categoryId);

        Task<ResponseModel> GetSearchCategories();
        //Task<ResponseModel> GetMainLists(string uid,HttpRequest request);
    }
}