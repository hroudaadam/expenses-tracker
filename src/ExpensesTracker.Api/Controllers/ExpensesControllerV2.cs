using ExpensesTracker.Api.Dtos;
using ExpensesTracker.Api.Helpers;
using ExpensesTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Controllers.V2
{
    [ApiVersion("2")]
    [ApiController]
    [Route("api/v{version:apiVersion}/expenses")]
    public class ExpensesController : ControllerBase
    {
        private IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        // GET /api/expenses/{expenseId}
        [HttpGet("{expenseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<ExpenseResV2>> Get(Guid expenseId)
        {
            var result = await _expensesService.GetAsync(expenseId);
            if (result == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, result.MapToExpenseResV2());
        }
    }
}
