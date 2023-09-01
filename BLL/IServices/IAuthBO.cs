using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IAuthBO
    {
        Task CheckRoles();
        Task<ClientProfile> CreateClientProfile(EmailSignUp_VM model, string uid);
        Task<ResponseModelV2> EmailSignIn(EmailSignIn_VM model, HttpRequest Request);
        Task<ResponseModelV2> EmailSignUp(EmailSignUp_VM model);
        Task<Profile_VM> GetProfile(string uid);
        Task<ResponseModelV2> UpdateProfile(UpdateProfile_VM model, string uid, HttpRequest Request);
        Task<ResponseModelV2> DeleteAccount(string uid);
    }
}