using AutoMapper;
using BLL.IServices;
using BLL.Utilities;
using BLL.Utilities.Extensions;
using BLL.ViewModels;
using DAL;
using DAL.Data;
using DAL.Models;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
//using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AuthBO : BaseBO, IAuthBO
    {
        private readonly CustomUserStore _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public AuthBO(IUnitOfWork unit, CustomUserStore userManager, SignInManager<ApplicationUser> signInManager, FintechLeapDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration = null) : base(unit)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task CheckRoles()
        {
            if (!await _roleManager.RoleExistsAsync(AppSetting.UserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = AppSetting.UserRole, NormalizedName = AppSetting.UserRoleNormalized });
            }

        }

        public async Task<ResponseModelV2> EmailSignIn(EmailSignIn_VM model, HttpRequest Request)
        {
            await CheckRoles();
            ResponseModelV2 responseModel = new ResponseModelV2();
            ApplicationUser res = await _userManager.FindByEmailAsync(model.Email);
            if (res == null)
            {
                responseModel.Result = 0;
                responseModel.Message = "Incorrect email/password combination";
                responseModel.Data = "";
                return responseModel;
            }

            var pass = await _userManager.CheckPasswordAsync(res, model.Password);
            if (!pass)
            {
                responseModel.Result = 0;
                responseModel.Message = "Incorrect email/password combination";
                responseModel.Data = "";
                return responseModel;
            }
            var signIn = await _signInManager.PasswordSignInAsync(res, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (!signIn.Succeeded)
            {
                responseModel.Result = 0;
                responseModel.Message = "Sign In Failed";
                responseModel.Data = "";
                return responseModel;
            }



            var roles = await _userManager.GetRolesAsync(res);
            Profile_VM profile = await _uow.ClientProfileRepository.GetAll(x => x.UserId == res.Id).Select(x => new Profile_VM
            {
                Email = x.Email,
                Id = x.Id,
                CountryCode = x.CountryCode,
                FullName = x.Name,
                PhoneNumber = x.PhoneNumber,
                Role = roles.FirstOrDefault(),
            }).FirstOrDefaultAsync();
            //var roles = await _userManager.GetRolesAsync(res);
            var claims = Tools.GenerateClaims(res, roles, profile.Id);
            string jwtSecret = _configuration["Secret"];
            string JwtToken = Tools.GenerateJWT(claims,jwtSecret);


            profile.Token = JwtToken;
            responseModel.Result = 1;
            responseModel.Message = "Sign in successful";
            responseModel.Data = profile;
            return responseModel;
        }
        public async Task<ResponseModelV2> EmailSignUp(EmailSignUp_VM model)
        {
            await CheckRoles();

            ApplicationUser oldUser;
            oldUser = await _userManager.FindByEmailAsync(model.Email);
            if (oldUser != null)
            {
                return CreateResponseModel(0, "Email is already taken", string.Empty);

            }
            IdentityResult_VM identityResult = await CreateUser(model);
            if (!identityResult.Result.Succeeded)
            {
                return CreateResponseModel(0, identityResult.Result.Errors.Select(x => x.Description).FirstOrDefault(), string.Empty);
            }
            ClientProfile newProfile = await CreateClientProfile(model, identityResult.User.Id);
            return CreateResponseModel(1, "Account created successfully", string.Empty);

        }
        public async Task<ClientProfile> CreateClientProfile(EmailSignUp_VM model, string uid)
        {
            ClientProfile profile = new ClientProfile()
            {
                CountryCode = model.CountryCode,
                CreatedDate = DateTime.UtcNow,
                Email = model.Email,
                IsDeleted = false,
                Name = model.FullName,
                PhoneNumber = model.PhoneNumber,
                UserId = uid,

            };

            await _uow.ClientProfileRepository.Create(profile);
            return profile;
        }
        public async Task<Profile_VM> GetProfile(string uid)
        {
            Profile_VM profile = await _uow.ClientProfileRepository.GetAll(x => x.UserId == uid).Select(x => new Profile_VM
            {
                Email = x.Email,
                Id = x.Id,
                CountryCode = x.CountryCode,
                FullName = x.Name,
                PhoneNumber = x.PhoneNumber,
            }).FirstOrDefaultAsync();
            return profile;
        }
        public async Task<ResponseModelV2> UpdateProfile(UpdateProfile_VM model, string uid, HttpRequest Request)
        {
            ResponseModelV2 responseModel = new ResponseModelV2();
            bool userExists = await _uow.UserRepos.CheckIfExists(x => x.Id == uid);
            if (!userExists)
            {
                return CreateResponseModel(0, "User not found", string.Empty);
            }

            ApplicationUser user = await _userManager.FindByIdAsync(uid);

            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                bool emailUserExists = await _uow.UserRepos.CheckIfExists(x => x.Email == model.Email && x.Id != uid);
                if (emailUserExists)
                {
                    return CreateResponseModel(0, "New email is taken", string.Empty);
                }
                user.Email = model.Email;
                user.UserName = model.Email;
                user.NormalizedUserName = model.Email.ToUpper();
                user.NormalizedEmail = model.Email.ToUpper();
                user.EmailConfirmed = false;
            }
            //if you want to change the phone number 
            user.PhoneNumber = model.PhoneNumber;
            user.CountryCode = model.CountryCode;
            user.Name = model.FullName;

            ClientProfile profile = await _uow.ClientProfileRepository.GetAllWithTracking(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefaultAsync();


            profile.Name = user.Name;
            profile.PhoneNumber = user.PhoneNumber;


            await _userManager.UpdateAsync(user);
            await _uow.SaveAsync();

            Profile_VM updatedProfile = new Profile_VM()
            {
                CountryCode = profile.CountryCode,
                Email = profile.Email,
                FullName = profile.Name,
                Id = profile.Id,
                PhoneNumber = profile.PhoneNumber,
            };

            //await _uow.UserRepos.Update(user);
            return CreateResponseModel(1, "Profile updated successfully", updatedProfile);



        }
        private async Task<IdentityResult_VM> CreateUser(EmailSignUp_VM model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                PhoneNumberConfirmed = true,
                IsDeleted = false,
                CountryCode = model.CountryCode,
                CreatedDate = DateTime.UtcNow,
                Name = model.FullName
            };

            IdentityResult res = await _userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
                //await _userManager.AddToRoleAsync(user, AppSetting.UserRole);
                await _userManager.AddToRoleAsync(user, AppSetting.UserRole);
            // check if user creation succeeded
            IdentityResult_VM result = new IdentityResult_VM
            {
                Result = res,
                User = user
            };
            return result;

        }
        public async Task<ResponseModelV2> DeleteAccount(string uid)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                return CreateResponseModel(0, "User not found", string.Empty);
            }
            bool isUserDeleted = user.IsDeleted ?? false;
            if (isUserDeleted)
            {
                
                return CreateResponseModel(0, "User not found", string.Empty);
            }

            ClientProfile profile = await _uow.ClientProfileRepository.GetAllWithTracking(x => x.UserId == uid).FirstOrDefaultAsync();

            user.IsDeleted = true;
            user.PhoneNumber = user.PhoneNumber + "_DELETED";
            user.UserName = user.PhoneNumber + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmm");
            user.Email = user.Email + "_DELETED";
            //profile
            profile.IsDeleted = true;
            profile.Email = profile.Email + "_DEL";
            profile.PhoneNumber = profile.PhoneNumber + "_DEL";

            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return CreateResponseModel(0, updateResult.Errors.Select(e => e.Description).FirstOrDefault(), string.Empty);
            }

            await _uow.ClientProfileRepository.Update(profile);
            return CreateResponseModel(1, "Account deleted", string.Empty);
        }


    }


}

