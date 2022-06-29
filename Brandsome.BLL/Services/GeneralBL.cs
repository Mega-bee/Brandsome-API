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
    public class GeneralBL : BaseBO, IGeneralBL
    {
        public GeneralBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetCities()
        {
            ResponseModel responseModel = new ResponseModel();
            List<City_VM> cities = await _uow.CityRepository.GetAll(x => x.IsDeleted == false).Select(c => new City_VM
            {
                Id = c.Id,
                Name = c.Title
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = cities, Message = "" };
            return responseModel;
        }
    }
}
