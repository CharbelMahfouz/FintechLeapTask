//using DAL.Models;
using DAL.Data;
using DAL.Repos;
using DAL.Services;
using System;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        protected readonly FintechLeapDbContext _context;

        public UnitOfWork(FintechLeapDbContext context)
        {
            _context = context;
        }



        #region private 
        private IUserRepos userRepos;
        private IResetPasswordRepo resetPasswordRepo;
        
        private IClientProfileRepository _clientprofileRepository;
      

        #endregion



        #region public 
        //public IProfileRepos ProfileRepos => profileRepos ?? new ProfileRepos(_context);
        public IUserRepos UserRepos => userRepos ?? new UserRepos(_context);
        
        public IResetPasswordRepo ResetPasswordRepo => resetPasswordRepo ?? new ResetPasswordRepo(_context);

        
        public IClientProfileRepository ClientProfileRepository => _clientprofileRepository ??= new ClientProfileRepo(_context);

      
        #endregion





        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async virtual Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
