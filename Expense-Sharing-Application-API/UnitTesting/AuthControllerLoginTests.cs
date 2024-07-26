using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation_Layer.Controllers;
using Xunit;

namespace UnitTesting
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new AuthController(_userServiceMock.Object);
        }

        #region Login Tests

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var model = new UserLoginModel { Email = "test@example.com", Password = "password" };
            _userServiceMock.Setup(x => x.GetUserByEmailAndPasswordAsync(model.Email, model.Password))
                            .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenUserFound()
        {
            // Arrange
            var model = new UserLoginModel { Email = "test@example.com", Password = "password" };
            var user = new User { Email = model.Email, Password = model.Password };
          

            _userServiceMock.Setup(x => x.GetUserByEmailAndPasswordAsync(model.Email, model.Password))
                            .ReturnsAsync(user);
           

            // Act
            var result = await _controller.Login(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        
        }

        #endregion

        #region Register Tests

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenEmailExists()
        {
            // Arrange
            var model = new UserRegisterModel { Email = "test@example.com", Password = "password", Role = "User" };
            _userServiceMock.Setup(x => x.UserExistsAsync(model.Email)).ReturnsAsync(true);

            // Act
            var result = await _controller.Register(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserRegistered()
        {
            // Arrange
            var model = new UserRegisterModel { Email = "test@example.com", Password = "password", Role = "User" };
            var user = new User { Email = model.Email, Password = model.Password, Role = model.Role };
           

            // Act
            var result = await _controller.Register(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            
        }

        #endregion

        #region GetAllUsers Tests
        [Fact]
        public async Task GetAllUsers_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Email = "test1@example.com" },
                new User { UserId = 2, Email = "test2@example.com" }
            };
            _userServiceMock.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(okResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(returnValue.Value);
            Assert.Equal(users.Count, model.Count());
        }
        #endregion

        #region GetUser Tests

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var user = new User { UserId = userId, Email = "test@example.com" };
            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsType<ActionResult<User>>(result);
            Assert.Equal(user, result.Value);
        }

        #endregion

        #region UpdateUserDetails Tests

        [Fact]
        public async Task UpdateUserDetails_ShouldReturnBadRequest_WhenUserIdMismatch()
        {
            // Arrange
            int userId = 1;
            var updatedUser = new User { UserId = 2, Email = "test@example.com", Role = "Admin" };

            // Act
            var result = await _controller.UpdateUserDetails(userId, updatedUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID in the URL must match the ID in the request body.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUserDetails_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            var updatedUser = new User { UserId = userId, Email = "test@example.com", Role = "Admin" };
            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateUserDetails(userId, updatedUser);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateUserDetails_ShouldReturnOk_WhenUserUpdated()
        {
            // Arrange
            int userId = 1;
            var existingUser = new User { UserId = userId, Email = "old@example.com", Role = "User" };
            var updatedUser = new User { UserId = userId, Email = "new@example.com", Role = "Admin" };

            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _userServiceMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUserDetails(userId, updatedUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User details updated successfully.", okResult.Value);
        }

        #endregion

        #region DeleteUser Tests

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task DeleteUser_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            int userIdToDelete = 1;
            var userToDelete = new User { UserId = userIdToDelete, Email = "test@example.com" };

            _userServiceMock.Setup(x => x.GetUserByIdAsync(userIdToDelete))
                            .ReturnsAsync(userToDelete);
            _userServiceMock.Setup(x => x.DeleteUserAsync(userToDelete))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userIdToDelete);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = okResult.Value;
            var messageProperty = messageObject.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(messageObject, null).ToString();

            Assert.Equal("User deleted successfully.", messageValue);
        }
        #endregion
    }
}
