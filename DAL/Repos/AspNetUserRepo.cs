using DAL.Data;
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;

namespace DAL.Repos
{
    public class AspNetUserRepo : GenericRepos<AspNetUser>, IAspNetUserRepository
    {
         public AspNetUserRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
