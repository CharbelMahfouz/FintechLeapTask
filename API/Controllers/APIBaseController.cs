using BLL.Utilities.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(AccountValidityFilterAttribute))]
    [ServiceFilter(typeof(ValidationFilterAttribute))]

    public class APIBaseController : ControllerBase
    {

    }
}
