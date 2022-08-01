using AutoMapper;
using Brandsome.BLL.IServices;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.Utilities.Logging;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL;
using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Services
{
    public class InterestsBL : BaseBO, IInterestsBL
    {
        private readonly BrandsomeDbContext _context;
        private string ConnectionString;
        public InterestsBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, ILoggerManager logger,IConfiguration configuration,BrandsomeDbContext context) : base(unit, mapper, notificationHelper, logger)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        public async Task<ResponseModel> GetCategories(HttpRequest request, string uid)
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
                        Name = s.Title,
                        Image = $"{request.Scheme}://{request.Host}/Images/{s.Image}",
                        IsUserInterest = s.Interests.ToList().Any(i => i.UserId == uid)
                    }).ToList(),
                }).ToList(),
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

        public async Task<ResponseModel> SetInterests(string uid, List<int> services)
        {
            ResponseModel responseModel = new ResponseModel();
            bool userExists = _uow.UserRepository.CheckIfExists(x => x.IsDeleted == false && x.Id == uid);
            if (!userExists)
            {
                responseModel.ErrorMessage = "User not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            //List<Interest> userInterests = await _uow.InterestRepository.GetAll(i => i.UserId == uid && i.IsDeleted == false).ToListAsync();

            //Interest newInterest = null;
            //foreach (var interest in model.Interests)
            //{
            //    if (userInterests.Count > 0)
            //    {
            //        foreach (var userInterest in userInterests)
            //        {
            //            if (interest.ServiceId == userInterest.ServiceId)
            //            {
            //                userInterest.IsDeleted = !interest.IsSelected;
            //                await _uow.InterestRepository.Update(userInterest);
            //            }
            //            else if(interest.IsSelected == false)
            //            {
            //                newInterest = new Interest()
            //                {
            //                    UserId = uid,
            //                    ServiceId = interest.ServiceId,
            //                    CreatedDate = DateTime.UtcNow,
            //                     IsDeleted = false,
            //                };
            //                await _uow.InterestRepository.Create(newInterest);
            //            }
            //        }
            //    } else if(interest.IsSelected == false)
            //    {
            //        newInterest = new Interest()
            //        {
            //            UserId = uid,
            //            ServiceId = interest.ServiceId,
            //            CreatedDate = DateTime.UtcNow,
            //            IsDeleted = false,
            //        };
            //        await _uow.InterestRepository.Create(newInterest);
            //    }
            //}

            ////DELETE ALL USER INTERESTS
            //using(SqlConnection conn = new SqlConnection(ConnectionString))
            //{
            //    conn.Open();
            //    using(SqlCommand command = new SqlCommand("DeleteAllInterestsFromUser",conn))
            //    {
            //        command.CommandType = System.Data.CommandType.StoredProcedure;
            //        SqlParameter UserId = new SqlParameter("@UserId ", uid);
            //        command.Parameters.Add(UserId);
            //        conn.Open();
            //        SqlDataAdapter da = new SqlDataAdapter(command);
            //        conn.Close();

            //    }
            //}

            var conneciton = (SqlConnection)_context.Database.GetDbConnection();
            string procedure1Name = AppSetting.DeleteInterestsFromUserProcedure;
            string procedure2Name = AppSetting.InsertUpdatedInterestsIntoUserProcedure;
            SqlCommand command1 = new SqlCommand(procedure1Name, conneciton);
            SqlCommand command2 = new SqlCommand(procedure2Name, conneciton);
            command1.CommandType = CommandType.StoredProcedure;
            command2.CommandType = CommandType.StoredProcedure;
            SqlParameter UserId = new SqlParameter("@UserId ", uid);
            var dt = new DataTable();
            dt.Columns.Add("ServiceId",typeof(int));
            foreach (var item in services)
            {
                dt.Rows.Add(item);
            }
            //SqlParameter serviceIds = new SqlParameter("@ServiceIdsList", dt);
        
            command1.Parameters.Add(UserId);
            conneciton.Open();
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            var command2Param = command2.Parameters.AddWithValue("@ServiceIdsList", dt);
            command2Param.SqlDbType = SqlDbType.Structured;
            command2.Parameters.Add(UserId);
            command2.ExecuteNonQuery();
            //DataTable dt = new DataTable();
            conneciton.Close();

           
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Interests set succesfully" };
            return responseModel;
        }


     

        private void ExecWithStoreProcedure(string query, params object[] parameters)
        {
            _context.Database.ExecuteSqlRaw("EXEC " + query, parameters);
        }
    }
}
