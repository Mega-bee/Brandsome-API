using Brandsome.BLL.ViewModels;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IInterestsBL
    {
        Task<ResponseModel> GetCategories();
        Task<ResponseModel> GetServices(int subcategoryId);
        Task<ResponseModel> GetSubCategories(int categoryId);
        Task<ResponseModel> GetMainLists();
    }
}