using BLL.ViewModels;
using DAL;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Utilities.ActionFilters
{
    public class AccountValidityFilterAttribute: IActionFilter
    {
        private readonly IUnitOfWork _unit;

        public AccountValidityFilterAttribute(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
           
            string uid = Tools.GetClaimValue(context.HttpContext, ClaimTypes.NameIdentifier);

         
            if (!string.IsNullOrEmpty(uid))
            {
                bool isDeleted = _unit.UserRepos.GetAll(u => u.Id == uid).Select(u => u.IsDeleted).FirstOrDefault() ?? false;

                if (isDeleted)
                {
                    ResponseModelV2 responseModel = new ResponseModelV2();
                    responseModel.Data = "";
                    responseModel.Message = "Account is deleted";
                    responseModel.Result = 0;
                    context.Result = new UnauthorizedObjectResult(responseModel);
                }


            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
