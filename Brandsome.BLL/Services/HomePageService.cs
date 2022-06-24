using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Services
{
    public class HomePageService : BaseBO, IHomePageService
    {

        public HomePageService(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }
        public async Task<ResponseModel> GetMainLists(HttpRequest request, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            SplashScreen_VM mainLists = new SplashScreen_VM();
            mainLists.Categories = await _uow.CategoryRepository.GetAll(x => x.IsDeleted == false).Select(c => new Category_VM
            {
                Id = c.Id,
                Name = c.Title,
                SubCategories = c.SubCategories.Where(sc => sc.IsDeleted == false).Select(sc => new SubCategory_VM
                {
                    Id = sc.Id,
                    Name = sc.Title,
                    Services = sc.Services.Where(s => s.IsDeleted == false).Select(s => new Service_VM
                    {
                        Id = s.Id,
                        Name = s.Title
                    }).ToList(),
                }).ToList(),
            }).ToListAsync();
            mainLists.Cities = await _uow.CityRepository.GetAll(x => x.IsDeleted == false).Select(c => new City_VM
            {
                Id = c.Id,
                Name = c.Title,
            }).ToListAsync();
            mainLists.Posts = await _uow.PostRepository.GetAll(x => x.IsDeleted == false && x.BusinessCity.IsDeleted == false && x.BusinessService.IsDeleted == false).Select(p => new Post_VM
            {
                Name = p.BusinessCity.Business.BusinessName ?? "",
                Description = p.Descrption ?? "",
                LikeCount = p.PostLikeCount ?? 0,
                Id = p.Id,
                ProfileImage = $"{request.Scheme}://{request.Host}/Images/{p.BusinessCity.Business.Image}",
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
                 
            }).ToListAsync();
            mainLists.Businesses = await _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false).Select(business => new Business_VM
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
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = mainLists, Message = "" };
            return responseModel;
        }

    }
}
