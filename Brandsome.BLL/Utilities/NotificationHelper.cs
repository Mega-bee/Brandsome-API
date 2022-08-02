using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL.Models;

namespace Brandsome.BLL.Utilities
{
    public class NotificationHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _fcmSettings;
        private readonly BrandsomeDbContext _context;

        public NotificationHelper(IConfiguration configuration, BrandsomeDbContext context)
        {
            _configuration = configuration;
            _fcmSettings = _configuration.GetSection("FcmNotification");
            _context = context;
        }
        public bool SendNotification(NotificationModel notificationModel)
        {
            try
            {

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var objNotification = new
                {
                    to = notificationModel.DeviceId,
                    notification = new
                    {
                        title = "Anomaly Detected",
                        body = notificationModel.Body,
                 
                        sound = "default",
                        //android_channel_id= "easyapproach"
                    },
                    android = new
                    {
                        priority = "HIGH",
                        notification = new
                        {
                            notification_priority = "PRIORITY_MAX",
                            sound = "default",
                            default_sound = true,
                            default_vibrate_timing = true,
                            default_light_settings = true
                        }
                    },
                    data = new
                    {
                        type = "order",
                        title = "Anomaly Detected",
                        body = notificationModel.Body,
                        //routePage= "order",
                        click_action = "FLUTTER_NOTIFICATION_CLICK"
                    }
                };

                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

                byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", _fcmSettings.GetSection("serverKey").Value));
                tRequest.Headers.Add(string.Format("Sender: id={0}", _fcmSettings.GetSection("senderId").Value));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string responseFromFirebaseServer = tReader.ReadToEnd();

                                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(responseFromFirebaseServer);
                                if (response != null)
                                {
                                    //var prof = _context.Orders.Where(x => x.Id == notificationModel.OrderId)
                                    //           .Select(x => new
                                    //           {
                                    //               x.ProfileId
                                    //           }).FirstOrDefault();

                                    return true;
                                }
                                else
                                {
                                    return false;

                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }




        }


    }
}
