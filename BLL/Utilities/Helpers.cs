using BLL.ViewModels;
using BLL.ViewModels.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.Utilities
{
    public static class Helpers
    {       
        public static ResponseModelV2 CreateResponseModel(int result, string message, dynamic data)
        {
            ResponseModelV2 responseModel = new ResponseModelV2()
            {
                Result = result,
                Message = message,
                Data = data
            };
            return responseModel;
        }
    }
}
