using DAL.Data;
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;

namespace DAL.Repos
{
    public class AspNetRoleClaimRepo : GenericRepos<AspNetRoleClaim>, IAspNetRoleClaimRepository
    {
         public AspNetRoleClaimRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
