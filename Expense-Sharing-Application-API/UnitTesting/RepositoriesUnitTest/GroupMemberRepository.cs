using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Access_Layer.Data;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTesting.RepositoriesUnitTest
{
    public class GroupMemberRepositoryTests
    {
        private readonly DbContextOptions<ExpenseSharingDbContext> _options;
        private readonly ExpenseSharingDbContext _context;
        private readonly GroupMemberRepository _repository;

        public GroupMemberRepositoryTests()
        {
            // Configure the DbContext to use an in-memory database
            _options = new DbContextOptionsBuilder<ExpenseSharingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ExpenseSharingDbContext(_options);
            _repository = new GroupMemberRepository(_context);

            // Seed data for testing
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test data to the in-memory database
            var group1 = new Group { GroupId = 1, Name = "Group1" };
            var group2 = new Group { GroupId = 2, Name = "Group2" };
            var user1 = new User { UserId = 1, Email = "user1@example.com", Balance = 100.00m };
            var user2 = new User { UserId = 2, Email = "user2@example.com", Balance = 50.00m };

            _context.Groups.AddRange(group1, group2);
            _context.Users.AddRange(user1, user2);
            _context.GroupMembers.AddRange(
                new GroupMember { GroupId = 1, UserId = 1, Group = group1, User = user1 },
                new GroupMember { GroupId = 1, UserId = 2, Group = group1, User = user2 },
                new GroupMember { GroupId = 2, UserId = 1, Group = group2, User = user1 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllGroupMembers_ShouldReturnAllGroupMembers()
        {
            // Act
            var result = await _repository.GetAllGroupMembers();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetGroupMembersByGroupId_ShouldReturnGroupMembers()
        {
            // Act
            var result = await _repository.GetGroupMembersByGroupId(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, gm => Assert.Equal(1, gm.GroupId));
        }

        [Fact]
        public async Task GetGroupMember_ShouldReturnGroupMember_WhenExists()
        {
            // Act
            var result = await _repository.GetGroupMember(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.GroupId);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetGroupMember_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetGroupMember(1, 3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddGroupMember_ShouldAddMember()
        {
            // Arrange
            var group = new Group { GroupId = 3, Name = "Group3" };
            var user = new User { UserId = 3, Email = "user3@example.com" };
            var groupMember = new GroupMember { GroupId = 3, UserId = 3, Group = group, User = user };

            _context.Groups.Add(group);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.AddGroupMember(groupMember);
            var addedGroupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.GroupId == 3 && gm.UserId == 3);

            // Assert
            Assert.True(result);
            Assert.NotNull(addedGroupMember);
            Assert.Equal(groupMember.GroupId, addedGroupMember.GroupId);
            Assert.Equal(groupMember.UserId, addedGroupMember.UserId);
        }

        [Fact]
        public async Task GetGroupIdsByUserIdAsync_ShouldReturnGroupIds()
        {
            // Act
            var result = await _repository.GetGroupIdsByUserIdAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(1, result);
            Assert.Contains(2, result);
        }

        [Fact]
        public async Task GetGroupMembersWithBalancesAsync_ShouldReturnGroupMembersWithBalances()
        {
            // Act
            var result = await _repository.GetGroupMembersWithBalancesAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.UserId == 1 && r.Email == "user1@example.com" && r.Balance == 100.00m);
            Assert.Contains(result, r => r.UserId == 2 && r.Email == "user2@example.com" && r.Balance == 50.00m);
        }
    }
}
