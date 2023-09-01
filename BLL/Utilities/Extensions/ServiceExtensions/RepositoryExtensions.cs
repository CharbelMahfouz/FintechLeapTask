
using BLL.IServices;
using BLL.Service;
using BLL.Utilities.ActionFilters;
using DAL;
using DAL.Models;
using DAL.Repos;
using DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Utilities.Extensions.ServiceExtensions
{
    public static class RepositoryExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepos<AccResetPasswordDetail>, GenericRepos<AccResetPasswordDetail>>();
            services.AddScoped<IGenericRepos<ApiDataLogging>, GenericRepos<ApiDataLogging>>();
            services.AddScoped<IGenericRepos<AspNetUser>, GenericRepos<AspNetUser>>();
            services.AddScoped<IGenericRepos<AspNetRoleClaimRepo>, GenericRepos<AspNetRoleClaimRepo>>();
            services.AddScoped<IGenericRepos<AspNetRoleRepo>, GenericRepos<AspNetRoleRepo>>();
            services.AddScoped<IGenericRepos<AspNetUserClaimRepo>, GenericRepos<AspNetUserClaimRepo>>();
            services.AddScoped<IGenericRepos<AspNetUserLoginRepo>, GenericRepos<AspNetUserLoginRepo>>();
            services.AddScoped<IGenericRepos<AspNetUserTokenRepo>, GenericRepos<AspNetUserTokenRepo>>();
            services.AddScoped<IGenericRepos<ClientProfile>, GenericRepos<ClientProfile>>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<AccountValidityFilterAttribute>();
            services.AddScoped<BaseBO>();
            services.AddScoped<IAuthBO, AuthBO>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ValidationFilterAttribute>();

        }
    }
}
