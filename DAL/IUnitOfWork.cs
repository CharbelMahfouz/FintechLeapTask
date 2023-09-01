using DAL.Services;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork
    {
        IUserRepos UserRepos { get; }
        IResetPasswordRepo ResetPasswordRepo { get; }
      
        IClientProfileRepository ClientProfileRepository { get; }
       
        void Save();
        Task SaveAsync();
    }
}
