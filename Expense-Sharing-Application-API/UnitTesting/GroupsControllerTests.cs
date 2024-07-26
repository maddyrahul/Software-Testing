using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation_Layer.Controllers;
using Xunit;

namespace UnitTesting
{
    public class GroupsControllerTests
    {
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly GroupsController _controller;

        public GroupsControllerTests()
        {
            _mockGroupService = new Mock<IGroupService>();
            _controller = new GroupsController(_mockGroupService.Object);
        }

        [Fact]
        public async Task CreateGroup_ReturnsCreatedAtAction_WithCreatedGroup()
        {
            // Arrange
            var group = new Group { GroupId = 1, Name = "Test Group", Description = "Test Description" };
            _mockGroupService.Setup(service => service.CreateGroup(group))
                .ReturnsAsync(group);

            // Act
            var result = await _controller.CreateGroup(group);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Group>(createdAtActionResult.Value);
            Assert.Equal(group.GroupId, returnValue.GroupId);
        }


        [Fact]
        public async Task CreateGroup_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var group = new Group { GroupId = 1, Name = "Test Group", Description = "Test Description" };
            _mockGroupService.Setup(service => service.CreateGroup(group))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.CreateGroup(group);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }


        [Fact]
        public async Task GetGroup_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int groupId = 1;
            _mockGroupService.Setup(service => service.GetGroup(groupId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetGroup(groupId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }
        [Fact]
        public async Task GetAllGroups_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockGroupService.Setup(service => service.GetAllGroups())
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetAllGroups();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }

        [Fact]
        public async Task DeleteGroup_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int groupId = 1;
            _mockGroupService.Setup(service => service.DeleteGroup(groupId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.DeleteGroup(groupId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }

        [Fact]
        public async Task GetGroup_ReturnsOkResult_WithGroup()
        {
            // Arrange
            int groupId = 1;
            var group = new Group { GroupId = groupId, Name = "Test Group", Description = "Test Description" };
            _mockGroupService.Setup(service => service.GetGroup(groupId))
                .ReturnsAsync(group);

            // Act
            var result = await _controller.GetGroup(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Group>(okResult.Value);
            Assert.Equal(groupId, returnValue.GroupId);
        }

        [Fact]
        public async Task GetGroup_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            int groupId = 1;
            _mockGroupService.Setup(service => service.GetGroup(groupId))
                .ReturnsAsync((Group)null);

            // Act
            var result = await _controller.GetGroup(groupId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllGroups_ReturnsOkResult_WithListOfGroups()
        {
            // Arrange
            var groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "Test Group 1", Description = "Test Description 1" },
                new Group { GroupId = 2, Name = "Test Group 2", Description = "Test Description 2" }
            };
            _mockGroupService.Setup(service => service.GetAllGroups())
                .ReturnsAsync(groups);

            // Act
            var result = await _controller.GetAllGroups();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Group>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllGroups_ReturnsNotFound_WhenNoGroupsExist()
        {
            // Arrange
            _mockGroupService.Setup(service => service.GetAllGroups())
                .ReturnsAsync((List<Group>)null);

            // Act
            var result = await _controller.GetAllGroups();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateExpense_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            int expenseId = 1;
            var updatedExpense = new Expense { ExpenseId = expenseId, Description = "Updated Expense", Amount = 100m };
            _mockGroupService.Setup(service => service.UpdateExpense(expenseId, updatedExpense))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateExpense(expenseId, updatedExpense);

         
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = okResult.Value;
            var messageProperty = messageObject.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(messageObject, null).ToString();


            Assert.Equal("Expense updated successfully.", messageValue);
        }

        [Fact]
        public async Task UpdateExpense_ReturnsNotFound_WhenExpenseDoesNotExist()
        {
            // Arrange
            int expenseId = 1;
            var updatedExpense = new Expense { ExpenseId = expenseId, Description = "Updated Expense", Amount = 100m };
            _mockGroupService.Setup(service => service.UpdateExpense(expenseId, updatedExpense))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateExpense(expenseId, updatedExpense);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddExpense_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var expenseDto = new ExpenseDto { Email = "test@example.com", GroupName = "Test Group", Description = "New Expense", Amount = 100, Date = DateTime.Now };
            _mockGroupService.Setup(service => service.AddExpense(expenseDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.AddExpense(expenseDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = okResult.Value;
            var messageProperty = messageObject.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(messageObject, null).ToString();
            Assert.Equal("Expense added successfully.", messageValue);
        }

        [Fact]
        public async Task AddExpense_ReturnsNotFound_WhenFailed()
        {
            // Arrange
            var expenseDto = new ExpenseDto { Email = "test@example.com", GroupName = "Test Group", Description = "New Expense", Amount = 100m, Date = DateTime.Now };
            _mockGroupService.Setup(service => service.AddExpense(expenseDto))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.AddExpense(expenseDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetExpense_ReturnsOkResult_WithExpense()
        {
            // Arrange
            int expenseId = 1;
            var expense = new Expense { ExpenseId = expenseId, Description = "Test Expense", Amount = 100m };
            _mockGroupService.Setup(service => service.GetExpense(expenseId))
                .ReturnsAsync(expense);

            // Act
            var result = await _controller.GetExpense(expenseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Expense>(okResult.Value);
            Assert.Equal(expenseId, returnValue.ExpenseId);
        }

        [Fact]
        public async Task GetExpense_ReturnsNotFound_WhenExpenseDoesNotExist()
        {
            // Arrange
            int expenseId = 1;
            _mockGroupService.Setup(service => service.GetExpense(expenseId))
                .ReturnsAsync((Expense)null);

            // Act
            var result = await _controller.GetExpense(expenseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task SettleExpense_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            int userId = 1;
            _mockGroupService.Setup(service => service.SettleExpense(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.SettleExpense(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = okResult.Value;
            var messageProperty = messageObject.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(messageObject, null).ToString();
            Assert.Equal("Expense settled successfully.", messageValue);
           
        }

        [Fact]
        public async Task SettleExpense_ReturnsNotFound_WhenFailed()
        {
            // Arrange
            int userId = 1;
            _mockGroupService.Setup(service => service.SettleExpense(userId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.SettleExpense(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllExpenses_ReturnsOkResult_WithListOfExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense { ExpenseId = 1, Description = "Expense 1", Amount = 100m },
                new Expense { ExpenseId = 2, Description = "Expense 2", Amount = 200m }
            };
            _mockGroupService.Setup(service => service.GetAllExpenses())
                .ReturnsAsync(expenses);

            // Act
            var result = await _controller.GetAllExpenses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Expense>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllExpenses_ReturnsNotFound_WhenNoExpensesExist()
        {
            // Arrange
            _mockGroupService.Setup(service => service.GetAllExpenses())
                .ReturnsAsync((List<Expense>)null);

            // Act
            var result = await _controller.GetAllExpenses();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteGroup_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            int groupId = 1;
            _mockGroupService.Setup(service => service.DeleteGroup(groupId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteGroup(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = okResult.Value;
            var messageProperty = messageObject.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(messageObject, null).ToString();
            Assert.Equal("Group deleted successfully.", messageValue);
        }

        [Fact]
        public async Task DeleteGroup_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            int groupId = 1;
            _mockGroupService.Setup(service => service.DeleteGroup(groupId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteGroup(groupId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task GetGroupsByUserId_ReturnsOkResult_WithListOfGroups()
        {
            // Arrange
            int userId = 1;
            var groups = new List<Group>
    {
        new Group { GroupId = 1, Name = "Test Group 1", Description = "Test Description 1" },
        new Group { GroupId = 2, Name = "Test Group 2", Description = "Test Description 2" }
    };
            _mockGroupService.Setup(service => service.GetGroupsByUserId(userId))
                .ReturnsAsync(groups);

            // Act
            var result = await _controller.GetGroupsByUserId(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Group>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<Group>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal(groups[0].GroupId, returnValue[0].GroupId); // Example assertion for group ID
                                                                     // Add more assertions as needed for group properties or count
        }

        [Fact]
        public async Task GetGroupsByUserId_ReturnsOkResult_WhenGroupsFound()
        {
            // Arrange
            int userId = 1;
            var groups = new List<Group>
    {
        new Group { GroupId = 1, Name = "Group1" },
        new Group { GroupId = 2, Name = "Group2" }
    };
            _mockGroupService.Setup(service => service.GetGroupsByUserId(userId))
                .ReturnsAsync(groups);

            // Act
            var result = await _controller.GetGroupsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Group>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetGroupsByUserId_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            int userId = 1;
            _mockGroupService.Setup(service => service.GetGroupsByUserId(userId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetGroupsByUserId(userId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }

        [Fact]
        public async Task UpdateExpense_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int expenseId = 1;
            var updatedExpense = new Expense { ExpenseId = expenseId, Description = "Updated Expense", Amount = 100m };
            _mockGroupService.Setup(service => service.UpdateExpense(expenseId, updatedExpense))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.UpdateExpense(expenseId, updatedExpense);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }

        [Fact]
        public async Task AddExpense_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var expenseDto = new ExpenseDto { Email = "test@example.com", GroupName = "Test Group", Description = "New Expense", Amount = 100m, Date = DateTime.Now };
            _mockGroupService.Setup(service => service.AddExpense(expenseDto))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.AddExpense(expenseDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }


        [Fact]
        public async Task GetExpense_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int expenseId = 1;
            _mockGroupService.Setup(service => service.GetExpense(expenseId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetExpense(expenseId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", objectResult.Value);
        }

        [Fact]
        public async Task SettleExpense_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            int userId = 1;
            var exceptionMessage = "Test exception";
            _mockGroupService.Setup(service => service.SettleExpense(userId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.SettleExpense(userId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal($"Internal server error: {exceptionMessage}", objectResult.Value);
        }


    }
}
