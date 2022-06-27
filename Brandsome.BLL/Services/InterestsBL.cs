using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Services
{
    public class InterestsBL : BaseBO, IInterestsBL
    {
        public InterestsBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetCategories()
        {
            ResponseModel responseModel = new ResponseModel();
            List<Category_VM> categories = await _uow.CategoryRepository.GetAll(x => x.IsDeleted == false).Select(x => new Category_VM
            {
                Id = x.Id,
                Name = x.Title
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = categories, Message = "" };
            return responseModel;
        } 
        
        public async Task<ResponseModel> GetSearchCategories()
        {
            ResponseModel responseModel = new ResponseModel();
           List<Category_VM> categories = await _uow.CategoryRepository.GetAll(x => x.IsDeleted == false).Select(c => new Category_VM
           {
               Id = c.Id,
               Name = c.Title,
               SubCategories = c.SubCategories.Where(sc => sc.IsDeleted == false).Select(sc => new SubCategory_VM
               {
                   Id = sc.Id,
                   Name = sc.Title,
                   Services = sc.Services.Where(s => s.IsDeleted == false).Select(s => new Service_VM
                   {
                       Id = s.Id,
                       Name = s.Title
                   }).ToList(),
               }).ToList(),
           }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = categories, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetSubCategories(int categoryId)
        {
            ResponseModel responseModel = new ResponseModel();
            List<SubCategory_VM> subCats = await _uow.SubcategoryRepository.GetAll(x => x.IsDeleted == false && x.CategoryId == categoryId).Select(x => new SubCategory_VM
            {
                Id = x.Id,
                Name = x.Title
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = subCats, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetServices(int subcategoryId)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Service_VM> services = await _uow.ServiceRepository.GetAll(x => x.IsDeleted == false && x.SubCategoryId == subcategoryId).Select(x => new Service_VM
            {
                Id = x.Id,
                Name = x.Title
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = services, Message = "" };
            return responseModel;
        }

        //public async Task<ResponseModel> GetMainLists()
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    SplashScreen_VM mainLists = new SplashScreen_VM();
        //    mainLists.Categories = await _uow.CategoryRepository.GetAll(x => x.IsDeleted == false).Select(c => new Category_VM
        //    {
        //        Id = c.Id,
        //        Name = c.Title,
        //        SubCategories = c.SubCategories.Where(sc => sc.IsDeleted == false).Select(sc => new SubCategory_VM
        //        {
        //            Id = sc.Id,
        //            Name = sc.Title,
        //            Services = sc.Services.Where(s => s.IsDeleted == false).Select(s => new Service_VM
        //            {
        //                Id = s.Id,
        //                Name = s.Title
        //            }).ToList(),
        //        }).ToList(),
        //    }).ToListAsync();
        //    mainLists.Cities = await _uow.CityRepository.GetAll(x => x.IsDeleted == false).Select(c => new City_VM
        //    {
        //        Id = c.Id,
        //        Name = c.Title,
        //    }).ToListAsync();
        //    responseModel.ErrorMessage = "";
        //    responseModel.StatusCode = 200;
        //    responseModel.Data = new DataModel { Data = mainLists, Message = "" };
        //    return responseModel;
        //}
    }
}
