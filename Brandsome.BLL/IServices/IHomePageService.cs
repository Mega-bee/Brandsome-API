using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IHomePageService
    {
        Task<ResponseModel> GetMainLists(HttpRequest request, string uid);
    }
}