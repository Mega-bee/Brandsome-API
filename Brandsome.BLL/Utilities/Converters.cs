using Brandsome.BLL.ViewModels;
using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities
{
    public static class Converters
    {
        public static Business_VM ConvertToBusinessVM(Business business, HttpRequest request)
        {
            var businessVm = new Business_VM
            {
                Id = business.Id,
                Cities = business.BusinessCities.Where(bc => bc.IsDeleted == false).Select(bc => new BusinessCity_VM
                {
                    Id = bc.Id,
                    Name = bc.City.Title
                }).ToList(),
                Name = business.BusinessName ??"",
                Image = $"{request.Scheme}://{request.Host}/Images/{business.Image}",
                PostCount = business.BusinessPostCount ?? 0,
                ReviewCount = business.BusinessReviewCount ?? 0,
                ViewCount = business.BusinessViewCount ?? 0,
                 FollowCount = business.BusinessFollowCount ?? 0,
                Services = business.BusinessServices.Where(bs => bs.IsDeleted == false).Select(bs => new BusinessService_VM
                {
                    Id = (int)bs.ServiceId,
                    Name = bs.Service.Title
                }).ToList()
            };
            return businessVm;
           } 
        }
    }

