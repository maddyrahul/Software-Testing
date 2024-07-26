using System.Collections.Generic;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using Moq;
using Xunit;

namespace UnitTesting
{
    public class GroupServiceTests
    {
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly GroupService _service;

        public GroupServiceTests()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _service = new GroupService(_groupRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateGroup_ShouldReturnCreatedGroup()
        {
            // Arrange
            var group = new Group { GroupId = 1, Name = "Test Group" };
            _groupRepositoryMock.Setup(x => x.CreateGroup(group)).ReturnsAsync(group);

            // Act
            var result = await _service.CreateGroup(group);

            // Assert
            Assert.Equal(group, result);
        }

        [Fact]
        public async Task GetGroup_ShouldReturnGroup_WhenGroupExists()
        {
            // Arrange
            var groupId = 1;
            var group = new Group { GroupId = groupId, Name = "Test Group" };
            _groupRepositoryMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(group);

            // Act
            var result = await _service.GetGroup(groupId);

            // Assert
            Assert.Equal(group, result);
        }

        [Fact]
        public async Task GetGroup_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            // Arrange
            var groupId = 1;
            _groupRepositoryMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync((Group)null);

            // Act
            var result = await _service.GetGroup(groupId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllGroups_ShouldReturnAllGroups()
        {
            // Arrange
            var groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "Group 1" },
                new Group { GroupId = 2, Name = "Group 2" }
            };
            _groupRepositoryMock.Setup(x => x.GetAllGroups()).ReturnsAsync(groups);

            // Act
            var result = await _service.GetAllGroups();

            // Assert
            Assert.Equal(groups.Count, result.Count());
        }

        [Fact]
        public async Task DeleteGroup_ShouldReturnTrue_WhenGroupDeleted()
        {
            // Arrange
            var groupId = 1;
            _groupRepositoryMock.Setup(x => x.DeleteGroup(groupId)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteGroup(groupId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteGroup_ShouldReturnFalse_WhenGroupNotDeleted()
        {
            // Arrange
            var groupId = 1;
            _groupRepositoryMock.Setup(x => x.DeleteGroup(groupId)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteGroup(groupId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateExpense_ShouldReturnTrue_WhenExpenseUpdated()
        {
            // Arrange
            var expenseId = 1;
            var updatedExpense = new Expense { ExpenseId = expenseId, Description = "Updated Expense", Amount = 100 };
            _groupRepositoryMock.Setup(x => x.UpdateExpense(updatedExpense)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateExpense(expenseId, updatedExpense);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateExpense_ShouldReturnFalse_WhenExpenseNotUpdated()
        {
            // Arrange
            var expenseId = 1;
            var updatedExpense = new Expense { ExpenseId = expenseId, Description = "Updated Expense", Amount = 100 };
            _groupRepositoryMock.Setup(x => x.UpdateExpense(updatedExpense)).ReturnsAsync(false);

            // Act
            var result = await _service.UpdateExpense(expenseId, updatedExpense);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllExpenses_ShouldReturnAllExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense { ExpenseId = 1, Description = "Expense 1", Amount = 50 },
                new Expense { ExpenseId = 2, Description = "Expense 2", Amount = 100 }
            };
            _groupRepositoryMock.Setup(x => x.GetAllExpenses()).ReturnsAsync(expenses);

            // Act
            var result = await _service.GetAllExpenses();

            // Assert
            Assert.Equal(expenses.Count, result.Count());
        }

        [Fact]
        public async Task GetExpense_ShouldReturnExpense_WhenExpenseExists()
        {
            // Arrange
            var expenseId = 1;
            var expense = new Expense { ExpenseId = expenseId, Description = "Expense 1", Amount = 50 };
            _groupRepositoryMock.Setup(x => x.GetExpense(expenseId)).ReturnsAsync(expense);

            // Act
            var result = await _service.GetExpense(expenseId);

            // Assert
            Assert.Equal(expense, result);
        }

        [Fact]
        public async Task GetExpense_ShouldReturnNull_WhenExpenseDoesNotExist()
        {
            // Arrange
            var expenseId = 1;
            _groupRepositoryMock.Setup(x => x.GetExpense(expenseId)).ReturnsAsync((Expense)null);

            // Act
            var result = await _service.GetExpense(expenseId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddExpense_ShouldReturnTrue_WhenExpenseAdded()
        {
            // Arrange
            var expenseDto = new ExpenseDto { Description = "New Expense", Amount = 100 };
            _groupRepositoryMock.Setup(x => x.AddExpense(expenseDto)).ReturnsAsync(true);

            // Act
            var result = await _service.AddExpense(expenseDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddExpense_ShouldReturnFalse_WhenExpenseNotAdded()
        {
            // Arrange
            var expenseDto = new ExpenseDto { Description = "New Expense", Amount = 100 };
            _groupRepositoryMock.Setup(x => x.AddExpense(expenseDto)).ReturnsAsync(false);

            // Act
            var result = await _service.AddExpense(expenseDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SettleExpense_ShouldReturnTrue_WhenExpenseSettled()
        {
            // Arrange
            var userId = 1;
            _groupRepositoryMock.Setup(x => x.SettleExpense(userId)).ReturnsAsync(true);

            // Act
            var result = await _service.SettleExpense(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SettleExpense_ShouldReturnFalse_WhenExpenseNotSettled()
        {
            // Arrange
            var userId = 1;
            _groupRepositoryMock.Setup(x => x.SettleExpense(userId)).ReturnsAsync(false);

            // Act
            var result = await _service.SettleExpense(userId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task GetGroupsByUserId_ShouldReturnGroups_WhenGroupsExist()
        {
            // Arrange
            var userId = 1;
            var groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "Group 1" },
                new Group { GroupId = 2, Name = "Group 2" }
            };
            _groupRepositoryMock.Setup(x => x.GetGroupsByUserId(userId)).ReturnsAsync(groups);

            // Act
            var result = await _service.GetGroupsByUserId(userId);

            // Assert
            Assert.Equal(groups.Count, result.Count());
            Assert.Contains(result, g => g.GroupId == 1 && g.Name == "Group 1");
            Assert.Contains(result, g => g.GroupId == 2 && g.Name == "Group 2");
        }

        [Fact]
        public async Task GetGroupsByUserId_ShouldReturnEmptyList_WhenNoGroupsExist()
        {
            // Arrange
            var userId = 1;
            var groups = new List<Group>();
            _groupRepositoryMock.Setup(x => x.GetGroupsByUserId(userId)).ReturnsAsync(groups);

            // Act
            var result = await _service.GetGroupsByUserId(userId);

            // Assert
            Assert.Empty(result);
        }
    }
}
