using Brandsome.BLL.ViewModels;
using System.Threading.Tasks;

namespace Brandsome.BLL.IServices
{
    public interface IPostsBL
    {
        Task<ResponseModel> CreatePost(CreatePost_VM post);
        Task<ResponseModel> DeletePost(string uid, int postId);
        Task<ResponseModel> UpdatePost(string uid, CreatePost_VM post);
        Task<ResponseModel> LikePost(string uid, int postId, bool isLike);
    }
}