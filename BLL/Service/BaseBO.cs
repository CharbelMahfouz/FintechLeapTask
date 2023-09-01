using AutoMapper;
using BLL.IServices;
using BLL.Utilities;
using BLL.ViewModels;
using DAL;

namespace BLL.Service
{
    public class BaseBO
    {
        protected readonly IUnitOfWork _uow;
        public BaseBO(IUnitOfWork unit)
        {
            _uow = unit;
           
         
        }

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
