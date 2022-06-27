using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brandsome.BLL.Utilities.Logging;
using Brandsome.BLL.ViewModels;
using Brandsome.BLL.IServices;
using Brandsome.BLL;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.Utilities.ActionFilters;
using Brandsome.DAL;
using Brandsome.DAL.Services;
using Brandsome.DAL.Models;
using Brandsome.DAL.Repos;
using Brandsome.BLL.Services;

namespace Brandsome.BLL
{
    public class ServiceInjector
    {
        private readonly IServiceCollection _services;
        public ServiceInjector(IServiceCollection services)
        {
            _services = services;
        }

        public void Render()
        {
            _services.AddScoped<BaseBO>();
            _services.AddScoped<IAuthBO, AuthBO>();
            _services.AddScoped<IBusinessBL, BusinessBL>();
            _services.AddScoped<IInterestsBL, InterestsBL>();
            _services.AddScoped<IHomePageService, HomePageService>();
            _services.AddScoped<IPostsBL, PostsBL>();
            _services.AddScoped<IUnitOfWork, UnitOfWork>();
            _services.AddScoped<IGenericRepos<AspNetUser>, GenericRepos<AspNetUser>>();
            _services.AddScoped<IGenericRepos<Business>, GenericRepos<Business>>();
            _services.AddScoped<IGenericRepos<City>, GenericRepos<City>>();
            _services.AddScoped<IGenericRepos<BusinessFollow>, GenericRepos<BusinessFollow>>();
            _services.AddScoped<IGenericRepos<BusinessReview>, GenericRepos<BusinessReview>>();
            _services.AddScoped<IGenericRepos<BusinessCity>, GenericRepos<BusinessCity>>();
            _services.AddScoped<IGenericRepos<PostMedium>, GenericRepos<PostMedium>>();
            _services.AddScoped<IGenericRepos<Category>, GenericRepos<Category>>();
            _services.AddScoped<IGenericRepos<SubCategory>, GenericRepos<SubCategory>>();
            _services.AddScoped<IGenericRepos<Service>, GenericRepos<Service>>();
            _services.AddScoped<IGenericRepos<Post>, GenericRepos<Post>>();
            _services.AddScoped<IGenericRepos<PostLike>, GenericRepos<PostLike>>();
            _services.AddScoped<IGenericRepos<BusinessService>, GenericRepos<BusinessService>>();
            _services.AddScoped<IGenericRepos<BusinessPhoneClick>, GenericRepos<BusinessPhoneClick>>();
            _services.AddScoped<IGenericRepos<PostLikeLog>, GenericRepos<PostLikeLog>>();
            _services.AddScoped<IGenericRepos<PostView>, GenericRepos<PostView>>();
            _services.AddScoped<IGenericRepos<Device>, GenericRepos<Device>>();
            _services.AddScoped<NotificationHelper>();
            _services.AddScoped<IConverters,Converters>();
            _services.AddScoped<ValidationFilterAttribute>();

            var configurationMapper = new MapperConfiguration(option =>
            {
                option.AddProfile(new UserProfile());

            });
            var mapper = configurationMapper.CreateMapper();
            _services.AddSingleton(mapper);

        }
    }
}
