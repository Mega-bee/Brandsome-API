using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.Utilities.Logging;
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
    public class NotificationBL : BaseBO, INotificationBL
    {
        public NotificationBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, ILoggerManager logger) : base(unit, mapper, notificationHelper, logger)
        {
        }

        public async Task<ResponseModel> GetUserNotifications(HttpRequest request, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Notification_VM> notifications = await _uow.NotificationRepo.GetAll(n => n.UserId == uid).Select(n => new Notification_VM
            {
                BusinessId = n.BusinessId ?? 0,
                BusinessName = n.Business.BusinessName ?? "",
                EventId = n.EventId ?? 0,
                InitiatorId = n.InitiatorId ?? "",
                InitiatorName = n.Initiator.Name ?? "",
                PostId = n.PostId ?? 0,
                PostImage = n.EventId == 1 ? $"{request.Scheme}://{request.Host}/Posts/Media/{n.Post.PostMedia.Select(pm => pm.FilePath).FirstOrDefault()}" : "",
                ReviewId = n.ReviewId ?? 0,
                 CreatedDate = n.CreatedDate,
                  InitiatorImage = $"{request.Scheme}://{request.Host}/Images/{n.User.Image.Trim()}".Trim(),
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = notifications, Message = "" };
            return responseModel;
        }
    }
}
