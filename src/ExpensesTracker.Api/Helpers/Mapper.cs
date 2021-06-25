using ExpensesTracker.Api.Data;
using ExpensesTracker.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Helpers
{
    /// <summary>
    /// Custome objects mapper
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Map Expense to ExpenseRes
        /// </summary>
        public static ExpenseRes MapToExpenseRes(this Expense expense)
        {
            var output = new ExpenseRes()
            { 
                Id = expense.Id,
                Amount = expense.Amount,
                Note = expense.Note,
                TimeStamp = expense.TimeStamp
            };
            // HATEOAS
            output.GenerateLinks();

            return output;
        }

        /// <summary>
        /// Map ExpenseRes to ExpenseResV2
        /// </summary>
        public static ExpenseResV2 MapToExpenseResV2(this ExpenseRes expense)
        {
            // HATEOAS
            expense.GenerateLinks();

            var output = new ExpenseResV2()
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Note = expense.Note,
                TimeStamp = expense.TimeStamp,
                Links = expense.Links
            };

            return output;
        }

        /// <summary>
        /// Map ExpenseReq to Expense
        /// </summary>
        public static Expense MapToExpense(this ExpenseReq expenseReq)
        {
            return new Expense()
            {
                Amount = expenseReq.Amount,
                Note = expenseReq.Note
            };
        }

        /// <summary>
        /// Map ExpenseRes list to ExpenseListRes
        /// </summary>
        public static ExpenseListRes MapExpenseListRes(this IEnumerable<ExpenseRes> expenses, int size, int number, int totalItems, int totalPages)
        {
            Page page = new()
            {
                Size = size,
                Number = number,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            ExpenseListRes expenseList = new()
            {
                Expenses = expenses,
                Page = page
            };
            // HATEOAS
            expenseList.GenerateLinks();

            return expenseList;
        }

    }
}
