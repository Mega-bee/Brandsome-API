using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities
{
    public static class Helpers
    {
        private static Random random = new Random();
        public static async Task<bool> SendEmailAsync(string toEmail, string subject, string content)
        {
            var client = new RestClient("https://api.sendinblue.com/v3/smtp/email");
            var request = new RestRequest(Method.POST);
            request.AddHeader("api-key", "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");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("undefined",
                "{\"tags\":[\"Tileo\"],\"sender\":{\"email\":\"" +
                "YallaJeye@gmail.com" + "\"},\"to\":[{\"email\":\"" + toEmail + "\",\"name\":\"" +
                toEmail + "\"}],\"cc\":[{\"email\":\"YallaJeye@gmail.com\",\"name\":\"Yalla Jeye\"}," +
                "{\"email\":\"YallaJeye@gmail.com \",\"name\":\"Yalla Jeye\"}],\"htmlContent\":\"" +
                content + "\",\"textContent\":\"" + content + "\",\"replyTo\":{\"email\":\"" +
                toEmail + "\"},\"subject\":\"" +
                subject + "\"}", ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);
            return response.IsSuccessful;
        }

        public static string Generate_otp()
        {
            char[] charArr = "0123456789".ToCharArray();
            string strrandom = string.Empty;
            Random objran = new Random();
            for (int i = 0; i < 4; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
                else i--;
            }
            return strrandom;
        }


        public static string RemoveCountryCode(string phoneNumber)
        {
            List<string> countries_list = new List<string>();
            countries_list.Add("+966");
            countries_list.Add("966");
            countries_list.Add("961");
            countries_list.Add("+961");
            countries_list.Add("0");

            foreach (var country in countries_list)
            {
                if (country == "0")
                {
                    if (phoneNumber.Substring(0, 1) == "0")
                    {
                        phoneNumber = phoneNumber.Remove(0, 1);
                    }
                }
                else if (phoneNumber.StartsWith(country))
                {
                    phoneNumber = phoneNumber.Replace(country, "");
                }
            }

            return phoneNumber;
        }

        public static bool SendSMS(string to, string content)
        {
            try
            {
                //var sssss = "https://sms.leblines.com/api.php?username=Fawwil&password=Fawwil9933&action=sendsms&from=Fawwil&to=" + to + "&text=" + content;
                //string res = to.Substring(0, 1);

                //string res = to.Substring(0, 3);

                //if (res == "966")
                //{
                //    to = to.Remove(0, 3);
                //    if (to.Substring(0, 1) == "0")
                //    {
                //        to = to.Remove(0, 1);
                //    }
                //} else if(to.Substring(0, 1) == "0")
                //{
                //    to = to.Remove(0, 1);
                //}
                to = RemoveCountryCode(to);
                //var sssss = "https://sms.leblines.com/api.php?username=Fawwil&password=Fawwil9933&action=sendsms&from=Fawwil&to=961" + to + "&text=Please follow this link to place your payment " + content;
                /////var sssss = "https://sms.leblines.com/api.php?username=Fawwil&password=Fawwil9933&action=sendsms&from=Fawwil&to=961" + to + $"&text={content}";
                //var sssss = "https://api.smsala.com/api/SendSMS?api_id=API898318662588&api_password=P@ssw0rd&sms_type=P&encoding=T&sender_id=HOLOL&phonenumber=966" + to + $"&textmessage={content}" + "&uid=SAU898312805752&callback_url=https://xyz.com/";
                //var sssss = "http://sms.gateway.sa:6005/api/v2/SendSMS?SenderId=CaterMe&Is_Unicode=false&Is_Flash=false&" + $"Message={content}" + "&MobileNumbers=966" + to + "&ApiKey=CG6WXovTOdX12VOP+781SZR27ff6Pd6yCrZHf1UAvGc=&ClientId=d91477cf-98fe-4e2f-9e1e-533ae06ecb29";

                var sssss = " https://www.proxireach.com/bestsmsbulkapi/sendSmsAPI.php?username=megabee&&password=MegaBee123!faq" + $"&&message={content}&&senderid=YJ OTP&&destination=" + to /*+ "&&date=DATE&&time=TIME"*/;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sssss);
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //if (httpResponse.StatusDescription == "OK")
                //{
                //    return true;
                //}
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public static string RandomStringNoCharacters(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static async Task<string> SaveFile(string path, IFormFile file)
        {
            var NewFileName = RandomStringNoCharacters(20) + file.FileName.ToString();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, NewFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return NewFileName;
        }


        public static void DeleteFile(string fileName, string path, IWebHostEnvironment hostEnvironment)
        {
            var filePath = Path.Combine(hostEnvironment.ContentRootPath, path, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
