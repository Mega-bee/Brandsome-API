using Brandsome.BLL.ViewModels;
using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities
{
    public interface IConverters
    {
        Task<List<Business_VM>> GetBusinessesVM(HttpRequest request);
        Business_VM GetBusinessesVM(Business business, HttpRequest request);
        IEnumerable<Post_VM> GetPostVM(IEnumerable<Post> query, HttpRequest request, string uid);
    }
}