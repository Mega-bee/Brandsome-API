using Brandsome.DAL.Models;
using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL.Repos
{
    public class PostLikeRepository : GenericRepos<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(BrandsomeDbContext context) : base(context)
        {
        }
    }
}
