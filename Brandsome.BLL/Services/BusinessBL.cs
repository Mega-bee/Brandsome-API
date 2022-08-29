using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.Utilities.Enums;
using Brandsome.BLL.Utilities.Logging;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Brandsome.DAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Services
{
    public class BusinessBL : BaseBO, IBusinessBL
    {

        public BusinessBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, ILoggerManager logger) : base(unit, mapper, notificationHelper, logger)
        {
        }

        public async Task<ResponseModel> GetBusinsses(List<int> services, string sortBy, HttpRequest request, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Business_VM> businesses = await _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false && x.User.IsDeleted == false).Select(business => new Business_VM
            {
                Id = business.Id,
                Cities = business.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = bc.Id,
                    Name = bc.City.Title
                }).ToList(),
                IsFollowed = business.BusinessFollows.Where(bf => bf.UserId == uid).FirstOrDefault() != null,
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
                if (services.Count > 0)
                {
                    businesses = businesses.Where(x => x.Services.Any(s => services.Contains((int)s.Id))).ToList();
                }

            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "A-Z")
                {
                    businesses = businesses.OrderBy(x => x.Name).ToList();
                }
                else
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

        public async Task<ResponseModel> GetUserBusinesses(string uid, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Business_VM> businesses = await _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false && x.UserId == uid && x.User.IsDeleted == false).Select(business => new Business_VM
            {
                Id = business.Id,
                Cities = business.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = (int)bc.CityId,
                    Name = bc.City.Title ?? "",
                    BusinessCityId = (int)bc.Id,
                }).ToList(),
                IsFollowed = business.BusinessFollows.Where(bf => bf.UserId == uid).FirstOrDefault() != null,
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
                    Name = bs.Service.Title ?? "",
                    BusinessServiceId = bs.Id,
                }).ToList()
            }).OrderByDescending(x => x.Id).ToListAsync();
            responseModel.Data = new DataModel { Data = businesses, Message = "" };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            return responseModel;
        }

        public async Task<ResponseModel> GetBusiness(string uid, int businessId, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            BusinessInfo_VM business = await _uow.BusinessRepository.GetAll().Where(x => x.Id == businessId && x.User.IsDeleted == false).Select(b => new BusinessInfo_VM
            {
                Id = b.Id,
                Cities = b.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = (int)bc.CityId,
                    Name = bc.City.Title ?? "",
                    BusinessCityId = (int)bc.Id,
                }).ToList(),
                IsFollowed = b.BusinessFollows.Where(bf => bf.UserId == uid).FirstOrDefault() != null,
                Description = b.Description ?? "",
                FollowCount = b.BusinessFollowCount ?? 0,
                PhoneNumber = b.BusinessPhone ?? "",
                Image = $"{request.Scheme}://{request.Host}/Images/{b.Image}",
                PostCount = b.BusinessPostCount ?? 0,
                Name = b.BusinessName ?? "",
                Posts = b.BusinessServices.SelectMany(bs => bs.Posts.Where(p => p.IsDeleted == false)).Select(p => new Post_VM
                {
                    Name = b.BusinessName ?? "",
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
                    //Cities = p.busi
                }).ToList(),
                ReviewCount = b.BusinessReviewCount ?? 0,
                Reviews = b.BusinessReviews.Where(br => br.IsDeleted == false && br.User.IsDeleted == false).Select(br => new ReviewBase_VM
                {
                    Description = br.Description ?? "",
                    Id = br.Id,
                    Name = br.User.Name ?? "",
                    Image = $"{request.Scheme}://{request.Host}/Uploads/{br.User.Image}",
                    CreatedDate = br.CreatedDate
                }).OrderByDescending(br => br.CreatedDate).ToList(),
                Services = b.BusinessServices.Where(bs => bs.IsDeleted == false).Select(bs => new BusinessService_VM
                {
                    Id = (int)bs.ServiceId,
                    Name = bs.Service.Title ?? "",
                    BusinessServiceId = bs.Id,
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

        public async Task<ResponseModel> FollowBusiness(string uid, int businessId, bool IsFollow)
        {

            ResponseModel responseModel = new ResponseModel();
            NotificationModel notification = new NotificationModel();

            BusinessFollow follow = null;
            bool businessExists = _uow.BusinessRepository.CheckIfExists(x => x.IsDeleted == false && x.Id == businessId && x.User.IsDeleted == false);
            if (!businessExists)
            {
                responseModel.Data = new DataModel { Data = "", Message = "" };
                responseModel.ErrorMessage = "Business not found";
                responseModel.StatusCode = 404;
                return responseModel;
            }
            var businessInfo = await _uow.BusinessRepository.GetAll(x => x.Id == businessId).Select(x => new { fcmToken = x.User.FcmToken, UserId = x.UserId }).FirstOrDefaultAsync();


            BusinessFollowLog followLog = new BusinessFollowLog();
            followLog.BusinessId = businessId;
            followLog.UserId = uid;
            followLog.IsFollow = IsFollow;
            followLog.CreatedDate = DateTime.UtcNow;

            await _uow.BusinessFollowLogRepository.Create(followLog);
            follow = await _uow.BusinessFollowRepository.GetFirst(f => f.BusinessId == businessId && f.UserId == uid && f.IsDeleted == false);
            if (follow == null)
            {
                follow = new BusinessFollow();
                follow.IsDeleted = false;
                follow.CreatedDate = DateTime.UtcNow;
                follow.UserId = uid;
                follow.BusinessId = businessId;
                await _uow.BusinessFollowRepository.Create(follow);
                string userName = _uow.UserRepository.GetAll(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(businessInfo.fcmToken))
                {
                    notification.DeviceId = businessInfo.fcmToken;
                    notification.BusinessId = businessId;
                    notification.UserId = businessInfo.UserId;
                    notification.EventId = (int)NotificationEvents.Follow;
                    notification.Title = Constants.BusinessFollowNotificationTitle;
                    notification.Body = userName + Constants.BusinessFollowNotificationBody;
                    notification.InitiatorId = uid;
                    await _notificationHelper.SendNotification(notification);
                }
            }
            else
            {
                await _uow.BusinessFollowRepository.Delete(follow.Id);
            }


            responseModel.Data = new DataModel { Data = "", Message = $"Business {(IsFollow ? "followed" : "unfollowed")} successfully" };
            responseModel.ErrorMessage = "";
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
                BusinessPhone = business.BusinessPhoneNumber,
                UserId = uid,
                IsDeleted = false
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
            Business currBusiness = await _uow.BusinessRepository.GetByIdWithPredicateAndIncludes(x => x.Id == business.Id && x.User.IsDeleted == false && x.IsDeleted == false, x => x.BusinessServices.Where(bs => bs.IsDeleted == false), x => x.BusinessCities.Where(bc => bc.IsDeleted == false));
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
            foreach (var item in currBusiness.BusinessCities)
            {
                item.IsDeleted = true;
                await _uow.BusinessCityRepository.Update(item);
            }

            foreach (var item in currBusiness.BusinessServices)
            {
                item.IsDeleted = true;
                await _uow.BusinessServiceRepository.Update(item);
            }
            if (business.Cities != null)
            {

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
            }

            if (business.Services != null)
            {
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

        public async Task<ResponseModel> DeleteBusiness(string uid, int businessId)
        {
            ResponseModel responseModel = new ResponseModel();
            Business business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId && x.UserId == uid && x.IsDeleted == false && x.User.IsDeleted == false).FirstOrDefaultAsync();
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

        public async Task<ResponseModel> RegisterNewPhoneClick(string uid, int businessId)
        {
            ResponseModel responseModel = new ResponseModel();
            Business business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId && x.IsDeleted == false && x.User.IsDeleted == false).FirstOrDefaultAsync();
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
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Phone click registered succesfully" };
            return responseModel;
        }

        public async Task<ResponseModel> RegisterNewBusinesView(string uid, string imei, int businessId)
        {
            ResponseModel responseModel = new ResponseModel();
            Business business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId && x.IsDeleted == false && x.User.IsDeleted == false).FirstOrDefaultAsync();
            if (business == null)
            {
                responseModel.ErrorMessage = "Business not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            BusinessView newView = new BusinessView()
            {
                BusinessId = businessId,
                UserId = uid,
                CreatedDate = DateTime.UtcNow,
                Imei = imei
            };
            await _uow.BusinessViewRepository.Create(newView);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Business view registered succesfully" };
            return responseModel;
        }

        //public async Task<ResponseModel> RegisterNewPhoneClick(string uid, int businessId,string)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    Business business = await _uow.BusinessRepository.GetAll(x => x.Id == businessId).FirstOrDefaultAsync();
        //    if (business == null)
        //    {
        //        responseModel.ErrorMessage = "Business not found";
        //        responseModel.StatusCode = 404;
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }
        //    BusinessPhoneClick newClick = new BusinessPhoneClick()
        //    {
        //        BusinessId = businessId,
        //        UserId = uid,
        //        CreatedDate = DateTime.UtcNow,


        //    };
        //    await _uow.BusinessPhoneClickRepository.Create(newClick);
        //    responseModel.ErrorMessage = "";
        //    responseModel.StatusCode = 200;
        //    responseModel.Data = new DataModel { Data = "", Message = "Phone click registered succesfully" };
        //    return responseModel;
        //}
    }
}
