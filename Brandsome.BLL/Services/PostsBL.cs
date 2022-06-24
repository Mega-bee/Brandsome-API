using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Services
{
    public class PostsBL : BaseBO, IPostsBL
    {
        public PostsBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
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

        public async Task<ResponseModel> LikePost(string uid,int postId,bool isLike)
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
            PostLikeLog postLike = new PostLikeLog()
            {
                CreatedDate = DateTime.UtcNow,
                IsLike = isLike,
                PostId = postId,
                UserId = uid
            };
            await _uow.PostLikeLogRepository.Create(postLike);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = $"Post {(isLike ? "liked" : "dislike")} successfully" };
            return responseModel;
        }
    }
}
