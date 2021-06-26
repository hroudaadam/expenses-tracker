using ExpensesTracker.Api.Dtos;
using ExpensesTracker.Api.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public ErrorRes Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            string message;

            // BL exception
            if (exception is AppLogicException)
            {
                Response.StatusCode = 400;
                message = exception.Message;
            }
            // server exception
            else
            {
                Response.StatusCode = 500;
                message = "Server error has occured";
            }

            return new ErrorRes() { Message = message };
        }
    }
}
