using ExpensesTracker.Api.Data;
using ExpensesTracker.Api.Dtos;
using ExpensesTracker.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Services
{
    public interface IExpensesService
    {
        Task<ExpenseListRes> ListAsync(int size, int page, string sort, decimal maxAmount, decimal minAmount);
        Task<ExpenseRes> GetAsync(Guid expenseId);
        Task<ExpenseRes> CreateAsync(ExpenseReq model);
        Task<ExpenseRes> PutAsync(ExpenseReq model, Guid expenseId);
        Task<bool> DeleteAsync(Guid expenseId);
    }

    public class ExpensesService : IExpensesService
    {
        private readonly DataContext _context;

        public ExpensesService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// List expenses
        /// </summary>
        public async Task<ExpenseListRes> ListAsync(int size, int page, string sort, decimal maxAmount, decimal minAmount)
        {
            int countOfItems = await _context.Expenses.CountAsync();
            int countOfPages = (int)Math.Ceiling((countOfItems / (double)size));

            // check if page is out of bounds
            if (page > countOfPages)
            {
                throw new AppLogicException("Page out of bounds");
            };

            int skip = (page - 1) * size;
            // get order query string
            string orderQuery = Sorting.GetDynamicFormat<Expense>(sort);

            var expenses = await _context.Expenses
                // filter, retype decimal to bool because of inconsistent precision in SQL and .NET
                .Where(ex => ((double)ex.Amount <= (double)maxAmount) && ((double)ex.Amount >= (double)minAmount)) 
                // apply sorting
                .OrderBy(orderQuery)
                // paging
                .Skip(skip).Take(size)
                .Select(ex => ex.MapToExpenseRes())
                .ToListAsync();
            
            var response = expenses.MapExpenseListRes(size, page, countOfItems, countOfPages);
            return response;
        }

        /// <summary>
        /// Get one expense
        /// </summary>
        public async Task<ExpenseRes> GetAsync(Guid expenseId)
        {
            var expense = await _context.Expenses.SingleOrDefaultAsync(ex => ex.Id == expenseId);
            
            // not found
            if (expense == null)
            {
                return null;
            }

            return expense.MapToExpenseRes();
        }

        /// <summary>
        /// Create expense
        /// </summary>
        public async Task<ExpenseRes> CreateAsync(ExpenseReq model)
        {
            Expense newExpense = model.MapToExpense();

            newExpense = _context.Expenses.Add(newExpense).Entity;
            await _context.SaveChangesAsync();

            return newExpense.MapToExpenseRes();
        }

        /// <summary>
        /// Delete expense
        /// </summary>
        public async Task<bool> DeleteAsync(Guid expenseId)
        {
            var expenseToDelete = await _context.Expenses
                .Where(ex => ex.Id == expenseId)
                .SingleOrDefaultAsync();
            // not found
            if (expenseToDelete == null)
            {
                return false;
            }

            _context.Expenses.Remove(expenseToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Update expense
        /// </summary>
        public async Task<ExpenseRes> PutAsync(ExpenseReq model, Guid expenseId)
        {
            var expenseToUpdate = await _context.Expenses
                .SingleOrDefaultAsync(ex => ex.Id == expenseId);
            
            // not found
            if (expenseToUpdate == null)
            {
                return null;
            }

            // update
            expenseToUpdate.Amount = model.Amount;
            expenseToUpdate.Note = model.Note;
            await _context.SaveChangesAsync();

            return expenseToUpdate.MapToExpenseRes();
        }
    }
}
