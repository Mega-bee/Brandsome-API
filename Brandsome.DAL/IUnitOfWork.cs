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
        void Save();
    }
}
