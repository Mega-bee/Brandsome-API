using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Brandsome.DAL.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Service
{
    public class BusinessBL : BaseBO, IBusinessBL
    {
        public BusinessBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public ResponseModel GetBusinsses(int serviceId, string sortBy)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Business_VM> businesses = _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false).Select(Converters.ConvertToBusinessVM).OrderByDescending(x => x.Id).ToList();


            if (serviceId > 0)
            {
                businesses = businesses.Where(x => x.Services.Any(s => s.Id == serviceId)).ToList();
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                var propertyInfo = typeof(Business_VM).GetProperty(sortBy);
                businesses = businesses.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
            }
            responseModel.Data = new DataModel { Data = businesses, Message = "" };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            return responseModel;

        }

        public async Task<ResponseModel> GetBusiness(string uid, int businessId)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessInfo_VM business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId).Select(b => new BusinessInfo_VM
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
                Image = b.Image ?? "",
                PostCount = b.BusinessPostCount ?? 0,
                Name = b.BusinessName ?? "",
                Posts = b.BusinessServices.SelectMany(bs => bs.Posts.Where(p => p.IsDeleted == false)).Select(p => new Post_VM
                {
                    Name = b.BusinessName ?? "",
                    Description = p.Descrption ?? "",
                    LikeCount = p.PostLikeCount ?? 0,
                    Id = p.Id,
                    IsLiked = p.PostLikes.Any(pl => pl.IsDeleted == false && pl.UserId == uid),
                    Type = p.BusinessService.Service.SubCategory.Category.Title + "/" + p.BusinessService.Service.SubCategory.Title + "/" + p.BusinessService.Service.Title,
                    City = p.BusinessCity.City.Title,
                    PostMedia = p.PostMedia.Select(pm => new PostMedia_VM
                    {
                        Id = pm.Id,
                        MediaTypeId = pm.PostTypeId ?? 0,
                        MediaTypeName = pm.PostType.Title ?? ""
                    }).ToList(),
                    //Cities = p.busi
                }).ToList(),
                ReviewCount = b.BusinessReviewCount ?? 0,
                Reviews = b.BusinessReviews.Select(br => new ReviewBase_VM
                {
                    Description = br.Description ?? "",
                    Id = br.Id,
                    Name = br.User.UserName ?? "",
                }).ToList(),

            }).FirstOrDefaultAsync();
            responseModel.Data = new DataModel { Data = business, Message = "" };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task<ResponseModel> FollowBusiness(string uid, int businessId)
        {

            ResponseModel responseModel = new ResponseModel();
            Business business = await _uow.BusinessRepository.GetFirst(x => x.IsDeleted == false && x.Id == businessId);
            if (business == null)
            {
                responseModel.Data = new DataModel { Data = "", Message = "" };
                responseModel.ErrorMessage = "Business not found";
                responseModel.StatusCode = 404;
                return responseModel;
            }
            BusinessFollow follow = await _uow.BusinessFollowRepository.GetFirst(x => x.BusinessId == businessId && x.UserId == uid);
            if (follow != null)
            {
                business.BusinessFollowCount--;
                follow.IsDeleted = true;
                await _uow.BusinessFollowRepository.Update(follow);
                await _uow.BusinessRepository.Update(business);
                responseModel.Data = new DataModel { Data = "", Message = "" };
                responseModel.ErrorMessage = "Business unfollowed";
                responseModel.StatusCode = 200;
                return responseModel;
            }
            follow = new BusinessFollow()
            {
                BusinessId = businessId,
                UserId = uid,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
            };
            await _uow.BusinessFollowRepository.Create(follow);
            business.BusinessFollowCount++;
            await _uow.BusinessRepository.Update(business);
            responseModel.Data = new DataModel { Data = "", Message = "" };
            responseModel.ErrorMessage = "Business followed";
            responseModel.StatusCode = 200;
            return responseModel;
        }

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
                UserId = uid,
            };
            var file = business.ImageFile;
            if (file != null)
            {
                string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);
                newBusiness.Image = NewFileName;
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

        public async Task<ResponseModel> CreatePost(CreatePost_VM post)
        {
            ResponseModel responseModel = new ResponseModel();
            Post newPost = new Post()
            {
                CreatedDate = DateTime.UtcNow,
                BusinessCityId = post.CityId,
                BusinessServiceId = post.ServiceId,
                Descrption = post.Description,
                IsDeleted = false,
                PostViewCount = 0,
                PostLikeCount = 0,
            };
            newPost = await _uow.PostRepository.Create(newPost);
            if (post.Media.Count > 0)
            {
                foreach (var item in post.Media)
                {
                    PostMedium media = new PostMedium();
                    media.PostId = newPost.Id;
                    bool isImage = Tools.CHeckIfImage(item);
                    bool isVideo = Tools.CheckIfVideo(item);
                    if (isImage)
                    {
                        media.PostTypeId = 1;
                    }
                    else if (isVideo)
                    {
                        media.PostTypeId = 2;
                    }

                    string NewFileName = await Helpers.SaveFile("wwwroot/Posts/Media", item);
                    media.FilePath = NewFileName;
                    await _uow.PostMediaRepository.Create(media);
                }
            }
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Post updated succesfully" };
            return responseModel;

        }

        public async Task<ResponseModel> UpdatePost(string uid, CreatePost_VM post)
        {
            ResponseModel responseModel = new ResponseModel();
            Post currPost = await _uow.PostRepository.GetFirst(x => x.Id == post.Id && x.IsDeleted == false && x.BusinessCity.Business.UserId == uid);
            if (currPost == null)
            {
                responseModel.ErrorMessage = "Post not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            currPost.Descrption = post.Description;
            currPost.BusinessCityId = post.CityId;
            currPost.BusinessServiceId = post.ServiceId;
            await _uow.PostRepository.Update(currPost);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Post updated successfully" };
            return responseModel;
        }
        //public async Task<ResponseModel> CreateBusiness()
    }
}
