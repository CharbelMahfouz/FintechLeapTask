using BLL.IServices;
using BLL.Utilities;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly IAuthBO _auth;

        public AccountsController(IAuthBO auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] EmailSignIn_VM model)
        {
            return Ok(await _auth.EmailSignIn(model,Request));
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] EmailSignUp_VM model)
        {
            return Ok(await _auth.EmailSignUp(model));
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetProfile()
        {
            string uid = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            return Ok(await _auth.GetProfile(uid));
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfile_VM model)
        {
            string uid = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            return Ok(await _auth.UpdateProfile(model, uid, Request));
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAccount()
        {
            string uid = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

            return Ok(await _auth.DeleteAccount(uid));
        }

    }


}
