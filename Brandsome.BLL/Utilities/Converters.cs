using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities
{
    public class Converters :IConverters
    { 
        private readonly IUnitOfWork _uow;

        public Converters(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Business_VM>> GetBusinessesVM(HttpRequest request)
        {
            List<Business_VM> businesses = await _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false).Select(business => new Business_VM
            {
                Id = business.Id,
                Cities = business.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = bc.Id,
                    Name = bc.City.Title
                }).ToList(),
                Description = business.Description,
                Name = business.BusinessName ?? "",
                Image = $"{request.Scheme}://{request.Host}/Images/{business.Image}",
                PostCount = business.BusinessPostCount ?? 0,
                ReviewCount = business.BusinessReviewCount ?? 0,
                ViewCount = business.BusinessViewCount ?? 0,
                FollowCount = business.BusinessFollowCount ?? 0,
                Services = business.BusinessServices.Where(bs => bs.IsDeleted == false).Select(bs => new BusinessService_VM
                {
                    Id = (int)bs.ServiceId,
                    Name = bs.Service.Title
                }).ToList()
            }).OrderByDescending(x => x.Id).ToListAsync();
            return businesses;

        }

        public Business_VM GetBusinessesVM(Business business, HttpRequest request)
        {
            Business_VM businesses = new Business_VM
            {
                Id = business.Id,
                Cities = business.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = bc.Id,
                    Name = bc.City.Title
                }).ToList(),
                Description = business.Description,
                Name = business.BusinessName ?? "",
                Image = $"{request.Scheme}://{request.Host}/Images/{business.Image}",
                PostCount = business.BusinessPostCount ?? 0,
                ReviewCount = business.BusinessReviewCount ?? 0,
                ViewCount = business.BusinessViewCount ?? 0,
                FollowCount = business.BusinessFollowCount ?? 0,
                Services = business.BusinessServices.Where(bs => bs.IsDeleted == false).Select(bs => new BusinessService_VM
                {
                    Id = (int)bs.ServiceId,
                    Name = bs.Service.Title
                }).ToList()
            };
            return businesses;

        }

        public IEnumerable<Post_VM> GetPostVM(IEnumerable<Post> query,HttpRequest request,string uid)
        {
            return query.Select<Post, Post_VM>(p => new Post_VM
            {
                Name = p.BusinessCity.Business.BusinessName ?? "",
                Description = p.Descrption ?? "",
                LikeCount = p.PostLikeCount ?? 0,
                Id = p.Id,
                IsLiked = p.PostLikes.Where(pl => pl.UserId == uid && pl.IsDeleted == false).FirstOrDefault() != null,
                Type = p.BusinessService.Service.SubCategory.Category.Title + "/" + p.BusinessService.Service.SubCategory.Title + "/" + p.BusinessService.Service.Title,
                City = p.BusinessCity.City.Title,
                PostMedia = p.PostMedia.Select(pm => new PostMedia_VM
                {
                    Id = pm.Id,
                    Url = $"{request.Scheme}://{request.Host}/posts/media/{pm.FilePath}",
                    MediaTypeId = pm.PostTypeId ?? 0,
                    MediaTypeName = pm.PostType.Title ?? "",

                }).ToList(),
                ProfileImage = $"{request.Scheme}://{request.Host}/Images/{p.BusinessCity.Business.Image}"
            });
        } 

    }
}

