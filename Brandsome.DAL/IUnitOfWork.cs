using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL
{
    public interface IUnitOfWork
    {


        IAspNetUserRepository UserRepository { get; }
        IBusinessRepository BusinessRepository { get; }
        IBusinessFollowRepository BusinessFollowRepository { get; }
        IBusinessReviewRepository BusinessReviewRepository { get; }
        IBusinessCityRepository BusinessCityRepository { get; }
        IBusinessServiceRepository BusinessServiceRepository { get; }
        IPostRepository PostRepository { get; }
        IPostMediaRepository PostMediaRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ISubcategoryRepository SubcategoryRepository { get; }
        IServiceRepository ServiceRepository { get; }
        ICityRepository CityRepository { get; }
        IPostLikeRepository PostLikeRepository { get; }
        IBusinessPhoneClickRepository BusinessPhoneClickRepository { get; }
        IPostLikeLogRepository PostLikeLogRepository { get; }
        IPostViewRepository PostViewRepository { get; }
        IBusinessFollowLogRepository BusinessFollowLogRepository { get; }
        void Save();
    }
}
