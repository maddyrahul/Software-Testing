using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data_Access_Layer.Data;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTesting.RepositoriesUnitTest
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly ExpenseSharingDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ExpenseSharingDbContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseSharingTestDb")
                .Options;
            _context = new ExpenseSharingDbContext(options);
            _repository = new UserRepository(_context);

            // Seed data for testing
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var users = new List<User>
            {
                new User { UserId = 1, Email = "user1@example.com", Password = "password1", Role = "normal", Balance = 100 },
                new User { UserId = 2, Email = "user2@example.com", Password = "password2", Role = "normal", Balance = 200 },
                new User { UserId = 3, Email = "user3@example.com", Password = "password3", Role = "admin", Balance = 300 }
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Act
            var user = await _repository.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal("user1@example.com", user.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var user = await _repository.GetUserByIdAsync(999);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Act
            var users = await _repository.GetAllUsersAsync();

            // Assert
            Assert.NotNull(users);
            Assert.Equal(3, users.Count());
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            // Act
            var user = await _repository.GetUserByEmailAndPasswordAsync("user1@example.com", "password1");

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.UserId);
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Act
            var user = await _repository.GetUserByEmailAndPasswordAsync("user1@example.com", "wrongpassword");

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUser()
        {
            // Arrange
            var newUser = new User { UserId = 4, Email = "user4@example.com", Password = "password4", Role = "normal", Balance = 400 };

            // Act
            await _repository.AddUserAsync(newUser);

            // Assert
            var user = await _context.Users.FindAsync(4);
            Assert.NotNull(user);
            Assert.Equal("user4@example.com", user.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = await _context.Users.FindAsync(1);
            user.Balance = 500;

            // Act
            await _repository.UpdateUserAsync(user);

            // Assert
            var updatedUser = await _context.Users.FindAsync(1);
            Assert.Equal(500, updatedUser.Balance);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var user = await _context.Users.FindAsync(1);

            // Act
            await _repository.DeleteUserAsync(user);

            // Assert
            var deletedUser = await _context.Users.FindAsync(1);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Act
            var exists = await _repository.UserExistsAsync("user1@example.com");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Act
            var exists = await _repository.UserExistsAsync("nonexistent@example.com");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUser_WhenEmailExists()
        {
            // Act
            var user = await _repository.GetUserByEmail("user1@example.com");

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.UserId);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Act
            var user = await _repository.GetUserByEmail("nonexistent@example.com");

            // Assert
            Assert.Null(user);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
