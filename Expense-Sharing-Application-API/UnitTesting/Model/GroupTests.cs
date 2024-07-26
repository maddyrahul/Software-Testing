using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Model
{
    public class GroupTests
    {
        [Fact]
        public void Group_AllProperties_CanBeSetAndGet()
        {
            // Arrange
            var group = new Group();
            var members = new List<GroupMember>
        {
            new GroupMember { GroupMemberId = 1, UserId = 1, GroupId = 1 },
            new GroupMember { GroupMemberId = 2, UserId = 2, GroupId = 1 }
        };
            var expenses = new List<Expense>
        {
            new Expense { ExpenseId = 1, Description = "Test Expense 1", Amount = 50m, Date = new DateTime(2023, 1, 1), PaidById = 1, GroupId = 1 },
            new Expense { ExpenseId = 2, Description = "Test Expense 2", Amount = 75m, Date = new DateTime(2023, 1, 2), PaidById = 2, GroupId = 1 }
        };

            // Act
            group.GroupId = 1;
            group.Name = "Test Group";
            group.Description = "This is a test group.";
            group.CreatedDate = new DateTime(2023, 1, 1);
            group.Members = members;
            group.Expenses = expenses;

            // Assert
            Assert.Equal(1, group.GroupId);
            Assert.Equal("Test Group", group.Name);
            Assert.Equal("This is a test group.", group.Description);
            Assert.Equal(new DateTime(2023, 1, 1), group.CreatedDate);
            Assert.Equal(members, group.Members);
            Assert.Equal(expenses, group.Expenses);
        }
    }
}
