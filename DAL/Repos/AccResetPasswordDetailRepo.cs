
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;
using DAL.Data;

namespace DAL.Repos
{
    public class AccResetPasswordDetailRepo : GenericRepos<AccResetPasswordDetail>, IAccResetPasswordDetailRepository
    {
         public AccResetPasswordDetailRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
