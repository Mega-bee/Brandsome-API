using Brandsome.DAL.Models;
using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brandsome.DAL.Repos;

namespace Brandsome.DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        protected readonly BrandsomeDbContext _context;

        public UnitOfWork(BrandsomeDbContext context)
        {
            _context = context;
        }



        #region private 

        private IAspNetUserRepository userRepository;
        private IBusinessRepository businessRepository;
        private IBusinessFollowRepository businessFollowRepository;
        private IBusinessReviewRepository businessReviewRepository;
        private IBusinessCityRepository businessCityRepository;
        private IBusinessServiceRepository businessServiceRepository;
        private IPostRepository postRepository;
        private IPostMediaRepository postMediaRepository;
        private ICategoryRepository categoryRepository;
        private ISubcategoryRepository subcategoryRepository;
        private IServiceRepository serviceRepository;
        private ICityRepository cityRepository;
        private IPostLikeRepository postLikeRepository;
        private IBusinessPhoneClickRepository businessPhoneClickRepository;
        private IPostLikeLogRepository postLikeLogRepository;
        private IPostViewRepository postViewRepository;
        private IBusinessFollowLogRepository businessFollowLogRepository;

        #endregion


        #region public 

        public IAspNetUserRepository UserRepository => userRepository ?? new AspNetUserRepository(_context);
        public IBusinessRepository BusinessRepository => businessRepository ?? new BusinessRepository(_context);
        public IBusinessFollowRepository BusinessFollowRepository => businessFollowRepository ?? new BusinessFollowRepository(_context);
        public IBusinessReviewRepository BusinessReviewRepository => businessReviewRepository ?? new BusinessReviewRepository(_context);
        public IBusinessCityRepository BusinessCityRepository => businessCityRepository ?? new BusinessCityRepository(_context);
        public IBusinessServiceRepository BusinessServiceRepository => businessServiceRepository ?? new BusinessServiceRepository(_context);
        public IPostRepository PostRepository => postRepository ?? new PostRepository(_context);
        public IPostMediaRepository PostMediaRepository => postMediaRepository ?? new PostMediaRepository(_context);
        public ICategoryRepository CategoryRepository => categoryRepository ?? new CategoryRepository(_context);
        public ISubcategoryRepository SubcategoryRepository => subcategoryRepository ?? new SubcategoryRepository(_context);
        public IServiceRepository ServiceRepository => serviceRepository ?? new ServiceRepository(_context);
        public ICityRepository CityRepository => cityRepository ?? new CityRepository(_context);
        public IPostLikeRepository PostLikeRepository => postLikeRepository ?? new PostLikeRepository(_context);
        public IBusinessPhoneClickRepository BusinessPhoneClickRepository => businessPhoneClickRepository ?? new BusinessPhoneClickRepository(_context);
        public IPostLikeLogRepository PostLikeLogRepository => postLikeLogRepository ?? new PostLikeLogRepository(_context);
        public IPostViewRepository PostViewRepository => postViewRepository ?? new PostViewRepository(_context);
        public IBusinessFollowLogRepository BusinessFollowLogRepository => businessFollowLogRepository ?? new BusinessFollowLogRepository(_context);
        #endregion





        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
