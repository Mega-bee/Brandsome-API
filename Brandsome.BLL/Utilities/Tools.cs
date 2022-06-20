using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Brandsome.DAL.Models;
using Brandsome.DAL.Data;

namespace Brandsome.BLL.Utilities
{
    public static class Tools
    {
        public static async Task<string> SaveImage(this IFormFile formFile, string folderName)
        {
            string path = Path.Combine("wwwroot", "Images", folderName, Guid.NewGuid().ToString() + ".jpg");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fs);
            }
            return path;
        }

        public static async Task<List<string>> SaveImages(List<IFormFile> formFiles, string folderName)
        {
            List<string> result = new List<string>();
            foreach (IFormFile form in formFiles)
            {
                string path = await form.SaveImage(folderName);
                result.Add(path);
            }
            return result;
        }

        public static List<Claim> GenerateClaims(ApplicationUser res, IList<string> roles)
        {
            var claims = new List<Claim>()
                {
                //new Claim(JwtRegisteredClaimNames.Email , res.Email ),
                new Claim(ClaimTypes.Name , res.UserName),
                new Claim("UID",res.Id),
                //new Claim(ClaimTypes.Role , role.Name),
                new Claim(ClaimTypes.NameIdentifier, res.Id),
                };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public static string GenerateJWT(List<Claim> claims)
        {
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fdjfhjehfjhfuehfbhvdbvjjoq8327483rgh"));
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "https://localhost:44310",
               audience: "https://localhost:44310",
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddYears(1),
               signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
               );
            string JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return JwtToken;
        }

        public static bool Between(double? number, double? min, double? max)
        {
            return number >= min && number <= max;
        }

        public static async Task<string> SaveVideo(this IFormFile formFile, string folderName)
        {
            string path = Path.Combine("wwwroot", "Videos", folderName, Guid.NewGuid().ToString() + ".mp4");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fs);
            }
            return path;
        }


        public static async Task<List<string>> SaveVideos(List<IFormFile> formFiles, string folderName)
        {
            List<string> result = new List<string>();
            foreach (IFormFile form in formFiles)
            {
                string path = await form.SaveVideo(folderName);
                result.Add(path);
            }
            return result;
        }

        public static bool CheckIfVideo(IFormFile file)
        {
            string[] extensionArray = VideoTypes.Split(',');
            bool validExtension = extensionArray.Any(x => file.FileName.ToLower().EndsWith(x.ToLower()));
            return validExtension;
        }

        public static bool CHeckIfImage(IFormFile file)
        {
            string[] extensionArray = ImageTypes.Split(',');
            bool validExtension = extensionArray.Any(x => file.FileName.ToLower().EndsWith(x.ToLower()));
            return validExtension;
        }

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

        public static string ImageTypes = ".jpg,.bmp,.PNG,.EPS,.gif,.TIFF,.tif,.jfif";

        public static string VideoTypes = ".mp4,.wmv,.flv";
    }
}
