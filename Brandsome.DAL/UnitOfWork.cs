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

        #endregion


        #region public 

        public IAspNetUserRepository UserRepository => userRepository ?? new AspNetUserRepository(_context);
        public IBusinessRepository BusinessRepository => businessRepository ?? new BusinessRepository(_context);
        public IBusinessFollowRepository BusinessFollowRepository => businessFollowRepository ?? new BusinessFollowRepository(_context);
        public IBusinessReviewRepository BusinessReviewRepository => businessReviewRepository ?? new BusinessReviewRepository(_context);
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
