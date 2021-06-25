using ExpensesTracker.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Helpers
{
    /// <summary>
    /// HATEOAS generation
    /// </summary>
    public static class LinksGenerator
    {
        /// <summary>
        /// Genereate HATEOAS for single expense
        /// </summary>
        public static ExpenseRes GenerateLinks(this ExpenseRes expense)
        {
            List<Link> links = new();

            links.Add(new Link()
            {
                Href = $"/api/expenses/{expense.Id}",
                Rel = "self",
                Method = "GET"
            });

            links.Add(new Link()
            {
                Href = $"/api/expenses",
                Rel = "create",
                Method = "POST"
            });

            links.Add(new Link()
            {
                Href = $"/api/expenses/{expense.Id}",
                Rel = "delete",
                Method = "DELETE"
            });

            links.Add(new Link()
            {
                Href = $"/api/expenses/{expense.Id}",
                Rel = "update",
                Method = "PUT"
            });

            expense.Links = links;
            return expense;
        }

        /// <summary>
        /// Generate HATEOAS for expense list
        /// </summary>
        public static ExpenseListRes GenerateLinks(this ExpenseListRes expenseList)
        {
            List<Link> links = new();

            links.Add(new Link()
            {
                Href = $"/api/expenses?size={expenseList.Page.Size}&page={expenseList.Page.Number}",
                Rel = "self",
                Method = "GET"
            });

            // previons page link
            bool isFirst = expenseList.Page.Number == 1;
            if (!isFirst)
            {
                links.Add(new Link()
                {
                    Href = $"/api/expenses?size={expenseList.Page.Size}&page={expenseList.Page.Number-1}",
                    Rel = "prev",
                    Method = "GET"
                });
            }

            // next page link
            bool isLast = expenseList.Page.Number == expenseList.Page.TotalPages;
            if (!isLast)
            {
                links.Add(new Link()
                {
                    Href = $"/api/expenses?size={expenseList.Page.Size}&page={expenseList.Page.Number+1}",
                    Rel = "next",
                    Method = "GET"
                });
            }

            expenseList.Links = links;
            return expenseList;
        }
    }
}
