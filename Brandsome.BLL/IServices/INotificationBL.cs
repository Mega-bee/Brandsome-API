using Brandsome.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface INotificationBL
    {
        Task<ResponseModel> GetUserNotifications(HttpRequest request, string uid);
    }
}