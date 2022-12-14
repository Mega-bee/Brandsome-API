using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.Utilities.Logging;
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

namespace Brandsome.BLL.Services
{
    public class PostsBL : BaseBO, IPostsBL
    {
        public PostsBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, ILoggerManager logger) : base(unit, mapper, notificationHelper, logger)
        {

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
            responseModel.Data = new DataModel { Data = "", Message = "Post created succesfully" };
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

        public async Task<ResponseModel> DeletePost(string uid, int postId)
        {
            ResponseModel responseModel = new ResponseModel();
            Post currPost = await _uow.PostRepository.GetFirst(p => p.Id == postId && p.BusinessCity.Business.UserId == uid && p.IsDeleted == false);
            if (currPost == null)
            {
                responseModel.ErrorMessage = "Post not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            currPost.IsDeleted = true;
            await _uow.PostRepository.Update(currPost);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Post deleted successfully" };
            return responseModel;
        }

        public async Task<ResponseModel> LikePost(string uid, int postId, bool isLike)
        {
            ResponseModel responseModel = new ResponseModel();
            NotificationModel notification = new NotificationModel();

            bool postExists =  _uow.PostRepository.CheckIfExists(p => p.Id == postId && p.IsDeleted == false);
            PostLike postLike = null;
            if (!postExists)
            {
                responseModel.ErrorMessage = "Post not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            var postUserInfo = await _uow.PostRepository.GetAll(p=> p.Id == postId && p.IsDeleted == false).Select(p=> new {FcmToken= p.BusinessCity.Business.User.FcmToken ,UserId = p.BusinessCity.Business.UserId }).FirstOrDefaultAsync();
            PostLikeLog postLikeLog = new PostLikeLog()
            {
                CreatedDate = DateTime.UtcNow,
                IsLike = isLike,
                PostId = postId,
                UserId = uid
            };
            await _uow.PostLikeLogRepository.Create(postLikeLog);

            postLike = await _uow.PostLikeRepository.GetFirst(p => p.PostId == postId && p.IsDeleted == false && p.UserId == uid);


            if (postLike == null)
            {
                postLike = new PostLike();
                postLike.PostId = postId;
                postLike.IsDeleted = false;
                postLike.UserId = uid;
                postLike.CreatedDate = DateTime.UtcNow;
                await _uow.PostLikeRepository.Create(postLike);
                string userName = _uow.UserRepository.GetAll(x => x.Id == uid).Select(x => x.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(postUserInfo.FcmToken))
                {
                    notification.DeviceId = postUserInfo.FcmToken;
                    notification.Title = Constants.PostLikeNotificationTitle;
                    notification.Body = userName + Constants.PostLikeNotificationBody;
                    notification.UserId = postUserInfo.UserId;
                    notification.InitiatorId = uid;
                    notification.PostId = postId;
                    await _notificationHelper.SendNotification(notification);
                }
            }
            else
            {
                await _uow.PostLikeRepository.Delete(postLike.Id);
            }
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = $"Post {(isLike ? "liked" : "disliked")} successfully" };
            return responseModel;
        }

        public async Task<ResponseModel> LikeList(string uid, int postId, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();

            Post currPost = await _uow.PostRepository.GetFirst(p => p.Id == postId /*&& p.BusinessCity.Business.UserId == uid */&& p.IsDeleted == false);
            List<PostLike_VM> postLike = null;
            if (currPost == null)
            {
                responseModel.ErrorMessage = "Post not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            postLike = await _uow.PostLikeRepository.GetAll(pl => pl.PostId == postId && pl.IsDeleted == false && pl.User.IsDeleted == false).Select(x => new PostLike_VM
            {
                Id = x.Id,
                Name = x.User.Name,
                Image = $"{request.Scheme}://{request.Host}/Images/{x.User.Image}".Trim()
            }).ToListAsync();

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = postLike, Message = "" };
            return responseModel;
        }
    }
}
