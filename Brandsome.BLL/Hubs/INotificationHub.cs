using Brandsome.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Brandsome.BLL.Hubs
{
    public interface INotificationHub
    {
        Task MessageToUser(object outgoingMessage);
        Task UpdatedUserList(object onlineUsers);
        Task UpdatedDashboard(dynamic patients);
        Task UpdateSearchList(Search_VM search);
    }
}
