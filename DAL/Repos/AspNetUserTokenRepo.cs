using DAL.Data;
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;

namespace DAL.Repos
{
    public class AspNetUserTokenRepo : GenericRepos<AspNetUserToken>, IAspNetUserTokenRepository
    {
         public AspNetUserTokenRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
