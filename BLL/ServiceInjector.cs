using AutoMapper;
using BLL.IServices;
using BLL.Service;
using BLL.Utilities;
using BLL.Utilities.ActionFilters;
using BLL.Utilities.Extensions.ServiceExtensions;
using DAL;
using DAL.Models;
using DAL.Repos;
using DAL.Services;
//using DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class ServiceInjector
    {
        private readonly IServiceCollection _services;
        public ServiceInjector(IServiceCollection services)
        {
            _services = services;
        }


       


        public void RenderAPI()
        {
            _services.ConfigureRepositories();
            _services.ConfigureServices();
        }

    }
}
