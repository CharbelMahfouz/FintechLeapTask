using DAL.Data;
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;

namespace DAL.Repos
{
    public class AspNetUserLoginRepo : GenericRepos<AspNetUserLogin>, IAspNetUserLoginRepository
    {
         public AspNetUserLoginRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
