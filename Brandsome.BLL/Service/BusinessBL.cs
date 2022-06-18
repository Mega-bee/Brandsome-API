using AutoMapper;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Service
{
    public class BusinessBL : BaseBO
    {
        public BusinessBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public ResponseModel GetBusinsses(int serviceId, string sortBy)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Business_VM> businesses =  _uow.BusinessRepository.GetAll().Where(x => x.IsDeleted == false).Select(Converters.ConvertToBusinessVM).OrderByDescending(x=> x.Id).ToList();
            //if (serviceId == 0)
            //{
            //    businesses = businesses.OrderByDescending(x=> x.Id).ToList();
            //    responseModel.Data = new DataModel { Data = businesses, Message = "" };
            //    responseModel.ErrorMessage = "";
            //    responseModel.StatusCode = 200;
            //    return responseModel;
            //}

            if(serviceId > 0)
            {
                businesses = businesses.Where(x=> x.Services.Any(s => s.Id == serviceId)).ToList();
            }

            if(!string.IsNullOrEmpty(sortBy))
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
                    Name = bc.City.Title
                }).ToList(),
                Description = b.Description ?? "",
                FollowCount = b.BusinessFollowCount ?? 0,
                PhoneNumber = b.BusinessPhone ?? "",
                Image = b.Image ?? "",
                PostCount = b.BusinessPostCount ?? 0,
                Name = b.BusinessName ?? "",
                Posts = b.BusinessServices.SelectMany(bs => bs.Posts.Where(p => p.IsDeleted == false)).Select(p => new Post_VM
                {
                    Name = b.BusinessName,
                    Description = p.Descrption,
                    LikeCount = p.PostLikeCount ?? 0,
                    Id = p.Id,
                    IsLiked = p.PostLikes.Any(pl => pl.IsDeleted == false && pl.UserId == uid),
                    Type = p.BusinessService.Service.SubCategory.Category.Title + "/" + p.BusinessService.Service.SubCategory.Title + "/" + p.BusinessService.Service.Title,

                    //Cities = p.busi
                }).ToList(),
                 ReviewCount = b.BusinessReviewCount ?? 0,
                  Reviews = b.BusinessReviews.Select(br=> new ReviewBase_VM
                  {
                       Description = br.Description ?? "",
                        Id = br.Id,
                         Name = br.User.UserName,
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
            if(follow != null)
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

        public async Task<ResponseModel> AddReview(CreateReview_VM review,string uid)
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

        //public async Task<ResponseModel> CreateBusiness()
    }
}
