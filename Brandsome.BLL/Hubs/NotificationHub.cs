//using Brandsome.DAL.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Brandsome.BLL.Hubs
//{
//    [Authorize]
//    public class NotificationHub : Hub<INotificationHub>
//    {
//        private static readonly Dictionary<string, string> UserIds = new Dictionary<string, string>();
//        private readonly BrandsomeDbContext _context;

//        public NotificationHub(BrandsomeDbContext context)
//        {
//            _context = context;
//        }

//        public void RegisterUser(string id)
//        {

//            UserIds.Add(Context.ConnectionId, id);
//            UpdateUserList();
//        }

//        public override Task OnConnectedAsync()
//        {
//            //string userId = Context.User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
//            string userId = Context.UserIdentifier;
//            var role = _context.AccProfiles.Where(x => x.UserId == userId).Select(x => new { roleName = x.Role.RoleName }).FirstOrDefault();
//            if (!UserIds.ContainsKey(Context.ConnectionId))
//            {
//                UserIds.Add(Context.ConnectionId, userId);
//                UpdateUserList();
//            }

//            //UserIds.Add(userId, userId);
//            Groups.AddToGroupAsync(Context.ConnectionId, role.roleName);
//            UpdateUserList();
//            return base.OnConnectedAsync();
//        }
//        public override Task OnDisconnectedAsync(Exception exception)
//        {
//            if (UserIds.ContainsKey(Context.ConnectionId))
//            {
//                UserIds.Remove(Context.ConnectionId);
//                UpdateUserList();
//            }
//            return base.OnDisconnectedAsync(exception);
//        }

//        private Task UpdateUserList()
//        {
//            //var usersList = _context.AccProfiles.Select(x => new
//            //    Profile_VM
//            //{
//            //    Email = x.Email,
//            //    Id = x.Id,
//            //    Uid = x.UserId,
//            //    Name = x.Name,
//            //    Role = x.Role.RoleName,
//            //    PhoneNumber = x.PhoneNumber
//            //}).ToList();
//            //var usersList = Users.Select(x => new Profile_VM
//            //{
//            //    Id = x.Value.Id,
//            //    Name = x.Value.Name,
//            //    Uid = x.Value.Uid,
//            //    Role = x.Value.Role,
//            //    Email = x.Value.Email,
//            //}).ToList();
//            var usersList = UserIds.Values.ToList();
//            return Clients.All.UpdatedUserList(usersList);

//        }
//    }


//}
