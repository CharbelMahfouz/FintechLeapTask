using DAL.Data;
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;

namespace DAL.Repos
{
    public class AspNetUserClaimRepo : GenericRepos<AspNetUserClaim>, IAspNetUserClaimRepository
    {
         public AspNetUserClaimRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
