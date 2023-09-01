using DAL.Data;
using System.Collections.Generic;
using DAL.Services;
using DAL.Models;

namespace DAL.Repos
{
    public class ClientProfileRepo : GenericRepos<ClientProfile>, IClientProfileRepository
    {
         public ClientProfileRepo(FintechLeapDbContext context) : base(context)
        {
        }
    }
}
