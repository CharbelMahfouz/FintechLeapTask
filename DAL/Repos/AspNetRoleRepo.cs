
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;
using DAL.Data;
namespace DAL.Repos
{
    public class AspNetRoleRepo : GenericRepos<AspNetRole>, IAspNetRoleRepository
    {
         public AspNetRoleRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
