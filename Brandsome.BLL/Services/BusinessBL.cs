using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Brandsome.DAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Services
{
    public class BusinessBL : BaseBO, IBusinessBL
    {
        public BusinessBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetBusinsses( List<int> services, string sortBy, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Business_VM> businesses =await _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false).Select(business => new Business_VM
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


            if (services != null)
            {
                if(services.Count > 0)
                {
                    businesses = businesses.Where(x => x.Services.Any(s => services.Contains(s.Id))).ToList();
                }  
               
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if(sortBy == "A-Z")
                {
                    businesses = businesses.OrderBy(x=> x.Name).ToList();
                } else
                {
                    var propertyInfo = typeof(Business_VM).GetProperty(sortBy);
                    businesses = businesses.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                }
            }
            responseModel.Data = new DataModel { Data = businesses, Message = "" };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            return responseModel;

        }

        public async Task<ResponseModel> GetBusiness(string uid, int businessId, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessInfo_VM business = await _uow.BusinessRepository.GetAll().Where(x => x.Id == businessId).Select(b => new BusinessInfo_VM
            {
                Id = b.Id,
                Cities = b.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = bc.Id,
                    Name = bc.City.Title ?? ""
                }).ToList(),
                Description = b.Description ?? "",
                FollowCount = b.BusinessFollowCount ?? 0,
                PhoneNumber = b.BusinessPhone ?? "",
                Image = $"{request.Scheme}://{request.Host}/Images/{b.Image}",
                PostCount = b.BusinessPostCount ?? 0,
                Name = b.BusinessName ?? "",
                Posts = b.BusinessServices.SelectMany(bs => bs.Posts.Where(p => p.IsDeleted == false && p.BusinessCity.IsDeleted == false && p.BusinessService.IsDeleted == false)).Select(p => new Post_VM
                {
                    Name = b.BusinessName ?? "",
                    Description = p.Descrption ?? "",
                    LikeCount = p.PostLikeCount ?? 0,
                    Id = p.Id,
                    IsLiked = p.PostLikes.Where(pl => pl.UserId == uid && pl.IsDeleted == false).FirstOrDefault()!= null,
                    Type = p.BusinessService.Service.SubCategory.Category.Title + "/" + p.BusinessService.Service.SubCategory.Title + "/" + p.BusinessService.Service.Title,
                    City = p.BusinessCity.City.Title,
                    PostMedia = p.PostMedia.Select(pm => new PostMedia_VM
                    {
                        Id = pm.Id,
                        Url = $"{request.Scheme}://{request.Host}/posts/media/{pm.FilePath}",
                        MediaTypeId = pm.PostTypeId ?? 0,
                        MediaTypeName = pm.PostType.Title ?? "",
                         
                    }).ToList(),
                    //Cities = p.busi
                }).ToList(),
                ReviewCount = b.BusinessReviewCount ?? 0,
                Reviews = b.BusinessReviews.Select(br => new ReviewBase_VM
                {
                    Description = br.Description ?? "",
                    Id = br.Id,
                    Name = br.User.UserName ?? "",
                    Image = $"{request.Scheme}://{request.Host}/Uploads/{br.User.Image}"
                }).ToList(),
                 Services = b.BusinessServices.Select(bs => new BusinessService_VM
                 {
                      Id=bs.Id,
                       Name = bs.Service.Title ?? ""
                 }).ToList(),
                  IsUserBusiness = uid == b.UserId,
                   Type = b.BusinessServices.Where(bs => bs.IsDeleted == false).First().Service.SubCategory.Category.Title + "/" + b.BusinessServices.Where(bs => bs.IsDeleted == false).First().Service.SubCategory.Title,
                    ViewCount = b.BusinessViewCount ?? 0,
            }).FirstOrDefaultAsync();
            responseModel.Data = new DataModel { Data = business, Message = "" };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            return responseModel;
        }

        //public async Task<ResponseModel> FollowBusiness(string uid, int businessId)
        //{

        //    ResponseModel responseModel = new ResponseModel();
        //    Business business = await _uow.BusinessRepository.GetFirst(x => x.IsDeleted == false && x.Id == businessId);
        //    if (business == null)
        //    {
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        responseModel.ErrorMessage = "Business not found";
        //        responseModel.StatusCode = 404;
        //        return responseModel;
        //    }
        //    BusinessFollow follow = await _uow.BusinessFollowRepository.GetFirst(x => x.BusinessId == businessId && x.UserId == uid);
        //    if (follow != null)
        //    {
        //        business.BusinessFollowCount--;
        //        follow.IsDeleted = true;
        //        await _uow.BusinessFollowRepository.Update(follow);
        //        await _uow.BusinessRepository.Update(business);
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        responseModel.ErrorMessage = "Business unfollowed";
        //        responseModel.StatusCode = 200;
        //        return responseModel;
        //    }
        //    follow = new BusinessFollow()
        //    {
        //        BusinessId = businessId,
        //        UserId = uid,
        //        CreatedDate = DateTime.UtcNow,
        //        IsDeleted = false,
        //    };
        //    await _uow.BusinessFollowRepository.Create(follow);
        //    business.BusinessFollowCount++;
        //    await _uow.BusinessRepository.Update(business);
        //    responseModel.Data = new DataModel { Data = "", Message = "" };
        //    responseModel.ErrorMessage = "Business followed";
        //    responseModel.StatusCode = 200;
        //    return responseModel;
        //}

        public async Task<ResponseModel> AddReview(CreateReview_VM review, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessReview businessReview = new BusinessReview()
            {
                BusinessId = review.BusinessId,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                UserId = uid,
                Description = review.Description,
            };
            await _uow.BusinessReviewRepository.Create(businessReview);

            responseModel.Data = new DataModel { Data = "", Message = "Review added succesfully" };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task<ResponseModel> CreateBusiness(string uid, CreateBusiness_VM business)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessCity businessCity;
            BusinessService businessService;
            Business newBusiness = new Business()
            {
                BusinessFollowCount = 0,
                BusinessLikeCount = 0,
                BusinessPhoneClickCount = 0,
                BusinessReviewCount = 0,
                BusinessViewCount = 0,
                BusinessPostCount = 0,
                CreatedDate = DateTime.UtcNow,
                Description = business.BusinessDescription,
                BusinessName = business.BusinessName,
                 BusinessPhone = business.BusinessPhoneNumber,
                UserId = uid,
                IsDeleted= false
            };
            var file = business.ImageFile;
            if (file != null)
            {
                string NewFileName = await Helpers.SaveFile("wwwroot/images", file);
                newBusiness.Image = NewFileName;
            }
            else
            {
                newBusiness.Image = "business-img-placeholder.jpg";
            }
            newBusiness = await _uow.BusinessRepository.Create(newBusiness);
            foreach (var item in business.Cities)
            {
                businessCity = new BusinessCity()
                {
                    CityId = item,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    BusinessId = newBusiness.Id,
                };
                await _uow.BusinessCityRepository.Create(businessCity);
            }

            foreach (var item in business.Services)
            {
                businessService = new BusinessService()
                {
                    BusinessId = newBusiness.Id,
                    IsDeleted = false,
                    ServiceId = item,
                    CreatedDate = DateTime.UtcNow,
                };

                await _uow.BusinessServiceRepository.Create(businessService);
            }
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Business created succesfully" };
            return responseModel;
        }

        public async Task<ResponseModel> UpdateBusiness(CreateBusiness_VM business)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessCity businessCity;
            BusinessService businessService;
            Business currBusiness = await _uow.BusinessRepository.GetByIdWithPredicateAndIncludes(x => x.Id == business.Id, x => x.BusinessServices, x => x.BusinessCities);
            if (currBusiness == null)
            {
                responseModel.ErrorMessage = "Business not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            currBusiness.Description = business.BusinessDescription;
            currBusiness.BusinessName = business.BusinessName;
            currBusiness.BusinessPhone = business.BusinessPhoneNumber;
            var file = business.ImageFile;
            if (file != null)
            {
                string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);
                currBusiness.Image = NewFileName;
            }
            await _uow.BusinessRepository.Update(currBusiness);
            foreach (var item in business.Cities)
            {
                businessCity = new BusinessCity()
                {
                    CityId = item,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    BusinessId = currBusiness.Id,
                };
                await _uow.BusinessCityRepository.Create(businessCity);
            }

            foreach (var item in business.Services)
            {
                businessService = new BusinessService()
                {
                    BusinessId = currBusiness.Id,
                    IsDeleted = false,
                    ServiceId = item,
                    CreatedDate = DateTime.UtcNow,
                };

                await _uow.BusinessServiceRepository.Create(businessService);
            }
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Business updated successfully" };
            return responseModel;
        }

        public async Task<ResponseModel> DeleteBusinessCity(string uid, int businessCityId)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessCity businessCity = await _uow.BusinessCityRepository.GetByIdWithPredicateAndIncludes(x => x.Id == businessCityId && x.Business.UserId == uid && x.IsDeleted == false, x => x.Business);
            if (businessCity == null)
            {
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "Business city not found" };
                return responseModel;
            }
            businessCity.IsDeleted = true;
            await _uow.BusinessCityRepository.Update(businessCity);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Business city succesfully deleted" };
            return responseModel;
        }

        public async Task<ResponseModel> DeleteBusinessService(string uid, int businessServiceId)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessService businessService = await _uow.BusinessServiceRepository.GetByIdWithPredicateAndIncludes(x => x.Id == businessServiceId && x.Business.UserId == uid && x.IsDeleted == false, x => x.Business);
            if (businessService == null)
            {
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "Business service not found" };
                return responseModel;
            }
            businessService.IsDeleted = true;
            await _uow.BusinessServiceRepository.Update(businessService);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Business service succesfully deleted" };
            return responseModel;
        }

        public async Task<ResponseModel> GetBusinessCities(int businessId, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            List<BusinessCity_VM> cities = await _uow.BusinessCityRepository.GetAll(x => x.BusinessId == businessId && x.Business.UserId == uid).Select(x => new BusinessCity_VM
            {
                Id = x.Id,
                Name = x.City.Title
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = cities, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetBusinessServices(int businessId, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            List<BusinessService_VM> cities = await _uow.BusinessServiceRepository.GetAll(x => x.BusinessId == businessId && x.Business.UserId == uid).Select(x => new BusinessService_VM
            {
                Id = x.Id,
                Name = x.Service.Title
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = cities, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> DeleteBusiness(string uid,int businessId)
        {
            ResponseModel responseModel = new ResponseModel();
            Business business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId && x.UserId == uid && x.IsDeleted == false).FirstOrDefaultAsync();
            if (business == null)
            {
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "Business not found" };
                return responseModel;
            }
            business.IsDeleted = true;
            await _uow.BusinessRepository.Update(business);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Business succesfully deleted" };
            return responseModel;
        }

        public async Task<ResponseModel> RegisterNewPhoneClick(string uid,int businessId)
        {
            ResponseModel responseModel = new ResponseModel();
            Business business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId).FirstOrDefaultAsync();
            if (business == null)
            {
                responseModel.ErrorMessage = "Business not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            BusinessPhoneClick newClick = new BusinessPhoneClick()
            {
                BusinessId = businessId,
                UserId = uid,
                CreatedDate = DateTime.UtcNow,

            };
            await _uow.BusinessPhoneClickRepository.Create(newClick);
            business.BusinessPhoneClickCount++;
            await _uow.BusinessRepository.Update(business);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Phone click registered succesfully" };
            return responseModel;
        }

        //public async Task<ResponseModel> CreateBusiness()
    }
}
