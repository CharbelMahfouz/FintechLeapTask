using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BLL.ViewModels;
using BLL.Utilities.Logging;

namespace BLL.Utilities.CustomMiddleWare
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionHandler(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext,ex.Message);
            }


        }

        private Task HandleExceptionAsync(HttpContext context,string exceptionMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            //ResponseModel resp = new ResponseModel()
            //{
            //    Data = new DataModel
            //    {
            //        Data = "",
            //        Message = ""
            //    },
            //    ErrorMessage = "Something Went Wrong",
            //    StatusCode = 500
            //};
            ResponseModelV2 resp = new ResponseModelV2()
            {
                Data = "",
                Message = exceptionMessage,
                Result = 0,
                 
            };
            string jsonString = JsonSerializer.Serialize(resp);
            return context.Response.WriteAsync(jsonString);
        }



    }
}
