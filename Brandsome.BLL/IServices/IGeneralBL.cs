using Brandsome.BLL.ViewModels;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IGeneralBL
    {
        Task<ResponseModel> GetCities();
    }
}