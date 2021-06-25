using ExpensesTracker.Api.Dtos;
using ExpensesTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Controllers.V1
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/expenses")]
    public class ExpensesController : ControllerBase
    {
        private IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        // GET /api/expenses
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<ExpenseListRes>> List([FromQuery] ExpenseListQuery query)
        {
            var result = await _expensesService.ListAsync(query.Size, query.Page, query.Sort, query.MaxAmount, query.MinAmount);
            return StatusCode(200, result);
        }

        // GET /api/expenses/{expenseId}
        [HttpGet("{expenseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<ExpenseRes>> Get(Guid expenseId)
        {
            var result = await _expensesService.GetAsync(expenseId);
            if (result == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, result);
        }

        // DELETE /api/expenses/{expenseId}
        [HttpDelete("{expenseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(Guid expenseId)
        {
            bool found = await _expensesService.DeleteAsync(expenseId);
            if (!found)
            {
                return StatusCode(404);
            }

            return StatusCode(204);
        }

        // POST /api/expenses
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<ExpenseRes>> Post([FromBody] ExpenseReq body)
        {
            var response = await _expensesService.CreateAsync(body);
            return StatusCode(201, response);
        }

        // PUT /api/expenses/{expenseId}
        [HttpPut("{expenseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<ExpenseRes>> Put(Guid expenseId, [FromBody] ExpenseReq body)
        {
            var response = await _expensesService.PutAsync(body, expenseId);
            if (response == null)
            {
                return StatusCode(404);
            }
            return StatusCode(200, response);
        }

        // TODO: PATCH
    }
}
