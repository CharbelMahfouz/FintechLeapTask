using BLL.Utilities.Extensions;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Utilities.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
               

                ResponseModelV2 resp = new ResponseModelV2()
                {
                    Result = 0,
                    Message = "Validation Errors",
                    Errors = context.ModelState.GetValidationErrors(),
                     Data = ""
                };
                string jsonString = JsonSerializer.Serialize(resp);
              
                context.Result = new OkObjectResult(jsonString);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
