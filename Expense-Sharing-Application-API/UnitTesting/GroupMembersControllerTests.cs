using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation_Layer.Controllers;
using Xunit;
using static Business_Layer.Services.IGroupMemberService;

namespace UnitTesting
{
    public class GroupMembersControllerTests
    {
        private readonly Mock<IGroupMemberService> _mockGroupMemberService;
        private readonly GroupMembersController _controller;

        public GroupMembersControllerTests()
        {
            _mockGroupMemberService = new Mock<IGroupMemberService>();
            _controller = new GroupMembersController(_mockGroupMemberService.Object);
        }

        [Fact]
        public async Task GetAllGroupMembers_ReturnsOkResult_WithAListOfGroupMembers()
        {
            // Arrange
            var groupMembers = new List<GroupMember>
            {
                new GroupMember { GroupMemberId = 1, GroupId = 1, UserId = 1, IsSettled = false },
                new GroupMember { GroupMemberId = 2, GroupId = 1, UserId = 2, IsSettled = true }
            };

            _mockGroupMemberService.Setup(service => service.GetAllGroupMembers())
                .ReturnsAsync(groupMembers);

            // Act
            var result = await _controller.GetAllGroupMembers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<GroupMember>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllGroupMembers_ReturnsNotFound_WhenNoGroupMembersExist()
        {
            // Arrange
            _mockGroupMemberService.Setup(service => service.GetAllGroupMembers())
                .ReturnsAsync((List<GroupMember>)null);

            // Act
            var result = await _controller.GetAllGroupMembers();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task GetAllGroupMembers_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockGroupMemberService.Setup(service => service.GetAllGroupMembers())
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetAllGroupMembers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", statusCodeResult.Value);
        }


        [Fact]
        public async Task GetGroupMembersByGroupId_ReturnsOkResult_WithAListOfGroupMembers()
        {
            // Arrange
            int groupId = 1;
            var groupMembers = new List<GroupMember>
            {
                new GroupMember { GroupMemberId = 1, GroupId = groupId, UserId = 1, IsSettled = false },
                new GroupMember { GroupMemberId = 2, GroupId = groupId, UserId = 2, IsSettled = true }
            };

            _mockGroupMemberService.Setup(service => service.GetGroupMembersByGroupId(groupId))
                .ReturnsAsync(groupMembers);

            // Act
            var result = await _controller.GetGroupMembersByGroupId(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<GroupMember>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetGroupMembersByGroupId_ReturnsNotFound_WhenNoGroupMembersExist()
        {
            // Arrange
            int groupId = 1;
            _mockGroupMemberService.Setup(service => service.GetGroupMembersByGroupId(groupId))
                .ReturnsAsync((List<GroupMember>)null);

            // Act
            var result = await _controller.GetGroupMembersByGroupId(groupId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetGroupMembersByGroupId_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            int groupId = 1;
            _mockGroupMemberService.Setup(service => service.GetGroupMembersByGroupId(groupId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetGroupMembersByGroupId(groupId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", statusCodeResult.Value);
        }


        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var model = new AddMemberRequest
            {
                GroupId = 1,
                Email = "test@example.com"
            };

            _mockGroupMemberService.Setup(service => service.AddMemberByGroupIdAndEmail(model))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.AddMemberByGroupIdAndEmail(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = okResult.Value;
            var messageProperty = messageObject.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(messageObject, null).ToString();

            Assert.Equal("User added to the group successfully.", messageValue);
        }

        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ReturnsBadRequest_WhenFailed()
        {
            // Arrange
            var model = new AddMemberRequest
            {
                GroupId = 1,
                Email = "test@example.com"
            };

            _mockGroupMemberService.Setup(service => service.AddMemberByGroupIdAndEmail(model))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.AddMemberByGroupIdAndEmail(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add member to the group.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var model = new AddMemberRequest
            {
                GroupId = 1,
                Email = "test@example.com"
            };

            _mockGroupMemberService.Setup(service => service.AddMemberByGroupIdAndEmail(model))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.AddMemberByGroupIdAndEmail(model);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", statusCodeResult.Value);
        }


        [Fact]
        public async Task GetGroupsWithBalancesByUserId_ReturnsOkResult_WithAListOfGroupsWithBalances()
        {
            // Arrange
            int userId = 1;
            var groupsWithBalances = new List<GroupWithMembersDto>
            {
                new GroupWithMembersDto
                {
                    GroupId = 1,
                    Members = new List<UserWithBalanceDto>
                    {
                        new UserWithBalanceDto { UserId = 1, Email = "user1@example.com", Balance = 100.0m },
                        new UserWithBalanceDto { UserId = 2, Email = "user2@example.com", Balance = 50.0m }
                    }
                },
                new GroupWithMembersDto
                {
                    GroupId = 2,
                    Members = new List<UserWithBalanceDto>
                    {
                        new UserWithBalanceDto { UserId = 1, Email = "user1@example.com", Balance = 200.0m }
                    }
                }
            };

            _mockGroupMemberService.Setup(service => service.GetGroupsWithBalancesByUserIdAsync(userId))
                .ReturnsAsync(groupsWithBalances);

            // Act
            var result = await _controller.GetGroupsWithBalancesByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<GroupWithMembersDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetGroupsWithBalancesByUserId_ReturnsNotFound_WhenNoGroupsWithBalancesExist()
        {
            // Arrange
            int userId = 1;
            _mockGroupMemberService.Setup(service => service.GetGroupsWithBalancesByUserIdAsync(userId))
                .ReturnsAsync((List<GroupWithMembersDto>)null);

            // Act
            var result = await _controller.GetGroupsWithBalancesByUserId(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("The user is not a member of any groups.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetGroupsWithBalancesByUserId_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            int userId = 1;
            _mockGroupMemberService.Setup(service => service.GetGroupsWithBalancesByUserIdAsync(userId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetGroupsWithBalancesByUserId(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", statusCodeResult.Value);
        }


        [Fact]
        public async Task GetGroupMembersWithBalances_ReturnsOkResult_WithAListOfUsersWithBalances()
        {
            // Arrange
            int groupId = 1;
            var membersWithBalances = new List<UserWithBalanceDto>
            {
                new UserWithBalanceDto { UserId = 1, Email = "user1@example.com", Balance = 100.0m },
                new UserWithBalanceDto { UserId = 2, Email = "user2@example.com", Balance = 50.0m }
            };

            _mockGroupMemberService.Setup(service => service.GetGroupMembersWithBalancesAsync(groupId))
                .ReturnsAsync(membersWithBalances);

            // Act
            var result = await _controller.GetGroupMembersWithBalances(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<UserWithBalanceDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetGroupMembersWithBalances_ReturnsNotFound_WhenNoMembersWithBalancesExist()
        {
            // Arrange
            int groupId = 1;
            _mockGroupMemberService.Setup(service => service.GetGroupMembersWithBalancesAsync(groupId))
                .ReturnsAsync((List<UserWithBalanceDto>)null);

            // Act
            var result = await _controller.GetGroupMembersWithBalances(groupId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetGroupMembersWithBalances_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            int groupId = 1;
            _mockGroupMemberService.Setup(service => service.GetGroupMembersWithBalancesAsync(groupId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.GetGroupMembersWithBalances(groupId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error: Simulated exception", statusCodeResult.Value);
        }

    }
}
