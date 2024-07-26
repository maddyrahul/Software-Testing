using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Model
{
    public class ExpenseTests
    {
        [Fact]
        public void Expense_AllProperties_CanBeSetAndGet()
        {
            // Arrange
            var expense = new Expense();
            var user = new User { UserId = 1, Email = "rahul@gmail.com" };
            var group = new Group { GroupId = 1, Name = "Test Group" };

            // Act
            expense.ExpenseId = 1;
            expense.Description = "Test Expense";
            expense.Amount = 100.50m;
            expense.Date = new DateTime(2023, 1, 1);
            expense.PaidById = 1;
            expense.PaidBy = user;
            expense.GroupId = 1;
            expense.Group = group;

            // Assert
            Assert.Equal(1, expense.ExpenseId);
            Assert.Equal("Test Expense", expense.Description);
            Assert.Equal(100.50m, expense.Amount);
            Assert.Equal(new DateTime(2023, 1, 1), expense.Date);
            Assert.Equal(1, expense.PaidById);
            Assert.Equal(user, expense.PaidBy);
            Assert.Equal(1, expense.GroupId);
            Assert.Equal(group, expense.Group);
        }
    }
}
