using ExpensesTracker.Api.Data;
using ExpensesTracker.Api.Services;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExpensesTracker.Api.Test
{
    public class ExpensesServiceTests
    {
        [Fact]
        public async Task GetNotExistingExpense()
        {
            // Arrange
            var mockSet = GetExpenses().AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(m => m.Expenses).Returns(mockSet.Object);

            var service = new ExpensesService(mockContext.Object);
            Guid expenseId = Guid.Empty;

            // Act
            var expense = await service.GetAsync(expenseId);

            // Assert
            Assert.Null(expense);
        }

        private List<Expense> GetExpenses()
        {
            return new List<Expense>()
            {
                new Expense()
                {
                    Id = Guid.NewGuid(),
                    Amount = decimal.Zero,
                    TimeStamp = DateTimeOffset.UtcNow
                },
                new Expense()
                {
                    Id = Guid.NewGuid(),
                    Amount = decimal.Zero,
                    TimeStamp = DateTimeOffset.UtcNow
                },
            };
        }
    }
}
