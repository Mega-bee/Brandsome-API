using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandsome.BLL.Hubs
{
    [Authorize]
    [AllowAnonymous]
    public class NotificationHub : Hub<INotificationHub>
    {
        private static readonly Dictionary<string, string> UserIds = new Dictionary<string, string>();
        private readonly IUnitOfWork _uow;

        public NotificationHub(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void RegisterUser(string id)
        {

            UserIds.Add(Context.ConnectionId, id);
            //UpdateUserList();
        }

        public override Task OnConnectedAsync()
        {
            //string userId = Context.User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
            string userId = Context.UserIdentifier;
            if (!UserIds.ContainsKey(Context.ConnectionId))
            {
                UserIds.Add(Context.ConnectionId, userId);
                //UpdateUserList();
            }

            //UserIds.Add(userId, userId);
            //Groups.AddToGroupAsync(Context.ConnectionId, role.roleName);
            //UpdateUserList();
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (UserIds.ContainsKey(Context.ConnectionId))
            {
                UserIds.Remove(Context.ConnectionId);
                UpdateUserList();
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Search(string searchTerm)
        {
            Search_VM search = new Search_VM();
            search.Businesses = await _uow.BusinessRepository.GetAll(b=> b.IsDeleted == false && b.BusinessName.Contains(searchTerm)).Take(8).Select(b=> new FollowedBusiness_VM
            {
                 Id = b.Id,
                  Image = b.Image,
                   Name = b.BusinessName,
                    Services = b.BusinessServices.Where(bs=> bs.IsDeleted == false).Select(bs=> new BusinessService_VM
                    {
                         Id=bs.Id,
                          Name = bs.Service.Title
                    }).ToList(),
                     Type = b.BusinessServices.Where(bs => bs.IsDeleted == false).Select(bs=> bs.Service.SubCategory.Category.Title).FirstOrDefault(),
            }).ToListAsync();
            search.Services = await _uow.ServiceRepository.GetAll(s => s.IsDeleted == false && s.Title.Contains(searchTerm)).Take(6).Select(s => new Service_VM
            {
                Id = s.Id,
                Image = s.Image,
                Name = s.Title
            }).ToListAsync();
             await Clients.Client(Context.ConnectionId).UpdateSearchList(search);
        }


        private Task UpdateUserList()
        {
            //var usersList = _context.AccProfiles.Select(x => new
            //    Profile_VM
            //{
            //    Email = x.Email,
            //    Id = x.Id,
            //    Uid = x.UserId,
            //    Name = x.Name,
            //    Role = x.Role.RoleName,
            //    PhoneNumber = x.PhoneNumber
            //}).ToList();
            //var usersList = Users.Select(x => new Profile_VM
            //{
            //    Id = x.Value.Id,
            //    Name = x.Value.Name,
            //    Uid = x.Value.Uid,
            //    Role = x.Value.Role,
            //    Email = x.Value.Email,
            //}).ToList();
            var usersList = UserIds.Values.ToList();
            return Clients.All.UpdatedUserList(usersList);

        }
    }


}
