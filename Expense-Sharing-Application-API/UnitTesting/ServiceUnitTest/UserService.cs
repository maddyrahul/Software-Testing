using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace UnitTesting
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();
            _service = new UserService(_userRepositoryMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Email = "test@example.com" };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Email = "user1@example.com" },
                new User { UserId = 2, Email = "user2@example.com" }
            };
            _userRepositoryMock.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordAsync_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var user = new User { Email = email, Password = password };
            _userRepositoryMock.Setup(x => x.GetUserByEmailAndPasswordAsync(email, password)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByEmailAndPasswordAsync(email, password);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordAsync_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            _userRepositoryMock.Setup(x => x.GetUserByEmailAndPasswordAsync(email, password)).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetUserByEmailAndPasswordAsync(email, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUser()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(x => x.AddUserAsync(user)).Returns(Task.CompletedTask);

            // Act
            await _service.AddUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = new User { UserId = 1, Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(x => x.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.UpdateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var user = new User { UserId = 1, Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(x => x.DeleteUserAsync(user)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.DeleteUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            _userRepositoryMock.Setup(x => x.UserExistsAsync(email)).ReturnsAsync(true);

            // Act
            var result = await _service.UserExistsAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            _userRepositoryMock.Setup(x => x.UserExistsAsync(email)).ReturnsAsync(false);

            // Act
            var result = await _service.UserExistsAsync(email);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void GenerateJwtToken_ShouldReturnToken_WhenUserIsValid()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Role = "User"
            };

            _configMock.Setup(c => c["Jwt:SecretKey"]).Returns("a_very_long_superSecretKey@345678901234567890");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            // Act
            var token = _service.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            Assert.Equal(user.UserId.ToString(), userIdClaim);
            Assert.Equal(user.Email, emailClaim);
            Assert.Equal(user.Role, roleClaim);
        }

        [Fact]
        public void GenerateJwtToken_ShouldContainCorrectIssuerAndAudience()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Role = "User"
            };

            _configMock.Setup(c => c["Jwt:SecretKey"]).Returns("a_very_long_superSecretKey@345678901234567890");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            // Act
            var token = _service.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal("TestAudience", jwtToken.Audiences.First());
        }

        [Fact]
        public void GenerateJwtToken_ShouldHaveCorrectExpiration()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Role = "User"
            };

            _configMock.Setup(c => c["Jwt:SecretKey"]).Returns("a_very_long_superSecretKey@345678901234567890");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            // Act
            var token = _service.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
            Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddDays(1));
        }
    }
}
