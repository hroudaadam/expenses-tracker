using ExpensesTracker.Api.Controllers.V1;
using ExpensesTracker.Api.Dtos;
using ExpensesTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpensesTracker.Api.Test
{
    public class ExpensesControllerV1Tests
    {
        [Fact]
        public async Task GetExpenseNotFound()
        {
            // Arrange
            Guid expenseId = Guid.Empty;
            var mockService = new Mock<IExpensesService>();
            mockService.Setup(s => s.GetAsync(expenseId))
                .ReturnsAsync((ExpenseRes)null);

            var controller = new ExpensesController(mockService.Object);

            // Act
            var response = await controller.Get(expenseId);
            var statusCodeResult = (StatusCodeResult)response.Result;

            // Assert
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteExpenseNotFound()
        {
            // Arrange
            Guid expenseId = Guid.Empty;
            var mockService = new Mock<IExpensesService>();
            mockService.Setup(s => s.DeleteAsync(expenseId))
                .ReturnsAsync(false);

            var controller = new ExpensesController(mockService.Object);

            // Act
            var response = await controller.Delete(expenseId);
            var statusCodeResult = (StatusCodeResult)response;

            // Assert
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task PutExpenseNotFound()
        {
            // Arrange
            Guid expenseId = Guid.Empty;
            var mockService = new Mock<IExpensesService>();
            mockService.Setup(s => s.PutAsync(It.IsAny<ExpenseReq>(), expenseId))
                .ReturnsAsync((ExpenseRes)null);

            var expenseUpdate = new ExpenseReq()
            {
                Amount = decimal.Zero
            };

            var controller = new ExpensesController(mockService.Object);

            // Act
            var response = await controller.Put(expenseId, expenseUpdate);
            var statusCodeResult = (StatusCodeResult)response.Result;

            // Assert
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ListExpenseObjectsCount()
        {
            // Arrange
            var query = new ExpenseListQuery();
            var expenseList = new ExpenseListRes()
            {
                Expenses = new List<ExpenseRes>()
                {
                    new ExpenseRes()
                    {
                        Id = new Guid(),
                        Amount = decimal.Zero,
                        TimeStamp = DateTimeOffset.UtcNow
                    },
                    new ExpenseRes()
                    {
                        Id = new Guid(),
                        Amount = decimal.Zero,
                        TimeStamp = DateTimeOffset.UtcNow.AddDays(-1)
                    },
                }
            };

            var mockService = new Mock<IExpensesService>();
            mockService.Setup(s => s.ListAsync(It.IsAny<int>(), It.IsAny<int>(),
                                               It.IsAny<string>(), It.IsAny<decimal>(),
                                               It.IsAny<decimal>()))
                .ReturnsAsync(expenseList);

            var controller = new ExpensesController(mockService.Object);

            // Act
            var response = await controller.List(query);
            var objResult = response.Result as ObjectResult;

            // Assert
            var expenseListResult = Assert.IsType<ExpenseListRes>(objResult.Value);
            Assert.Equal(2, expenseListResult.Expenses.Count());
        }
    }
}
