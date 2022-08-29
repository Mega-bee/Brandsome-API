using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class NotificationModel
    {
        public string DeviceId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string InitiatorId { get; set; }
        public int? EventId { get; set; }
        public int? BusinessId { get; set; }
        public int? PostId { get; set; }
        public int? ReviewId { get; set; }

    }

    public partial class Notification_VM
    {
        public string InitiatorId { get; set; }
        public string InitiatorName { get; set; }
        public string InitiatorImage { get; set; }
        public int? EventId { get; set; }
        public int? BusinessId { get; set; }
        public string BusinessName { get; set; }
        public int? ReviewId { get; set; }
        public int? PostId { get; set; }
        public string PostImage { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
