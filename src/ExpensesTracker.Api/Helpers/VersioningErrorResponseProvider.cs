using ExpensesTracker.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Helpers
{
    /// <summary>
    /// API versioning error handler
    /// </summary>
    public class VersioningErrorResponseProvider : IErrorResponseProvider
    {
        public IActionResult CreateResponse(ErrorResponseContext context)
        {
            return new NotFoundObjectResult(new ErrorRes() { Message = context.Message });
        }
    }
}
