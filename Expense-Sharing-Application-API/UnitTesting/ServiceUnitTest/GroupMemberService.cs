using System.Collections.Generic;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using Moq;
using Xunit;
using static Business_Layer.Services.IGroupMemberService;

namespace UnitTesting
{
    public class GroupMemberServiceTests
    {
        private readonly Mock<IGroupMemberRepository> _groupMemberRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly GroupMemberService _service;

        public GroupMemberServiceTests()
        {
            _groupMemberRepositoryMock = new Mock<IGroupMemberRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _service = new GroupMemberService(
                _groupMemberRepositoryMock.Object,
                _userRepositoryMock.Object,
                _groupRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllGroupMembers_ShouldReturnAllGroupMembers()
        {
            // Arrange
            var groupMembers = new List<GroupMember>
            {
                new GroupMember { GroupId = 1, UserId = 1 },
                new GroupMember { GroupId = 2, UserId = 2 }
            };

            _groupMemberRepositoryMock.Setup(x => x.GetAllGroupMembers())
                                      .ReturnsAsync(groupMembers);

            // Act
            var result = await _service.GetAllGroupMembers();

            // Assert
            Assert.Equal(groupMembers.Count, result.Count());
        }

        [Fact]
        public async Task GetGroupMembersByGroupId_ShouldReturnGroupMembersOfSpecificGroup()
        {
            // Arrange
            var groupId = 1;
            var groupMembers = new List<GroupMember>
            {
                new GroupMember { GroupId = groupId, UserId = 1 },
                new GroupMember { GroupId = groupId, UserId = 2 }
            };

            _groupMemberRepositoryMock.Setup(x => x.GetGroupMembersByGroupId(groupId))
                                      .ReturnsAsync(groupMembers);

            // Act
            var result = await _service.GetGroupMembersByGroupId(groupId);

            // Assert
            Assert.Equal(groupMembers.Count, result.Count());
        }

        [Fact]
        public async Task GetGroupIdsByUserIdAsync_ShouldReturnGroupIds()
        {
            // Arrange
            var userId = 1;
            var groupIds = new List<int> { 1, 2 };

            _groupMemberRepositoryMock.Setup(x => x.GetGroupIdsByUserIdAsync(userId))
                                      .ReturnsAsync(groupIds);

            // Act
            var result = await _service.GetGroupIdsByUserIdAsync(userId);

            // Assert
            Assert.Equal(groupIds.Count, result.Count());
            Assert.Equal(groupIds, result);
        }

        [Fact]
        public async Task GetGroupMembersWithBalancesAsync_ShouldReturnGroupMembersWithBalances()
        {
            // Arrange
            var groupId = 1;
            var groupMembersWithBalances = new List<UserWithBalanceDto>
            {
                new UserWithBalanceDto { UserId = 1, Email = "user1@example.com", Balance = 100 },
                new UserWithBalanceDto { UserId = 2, Email = "user2@example.com", Balance = 200 }
            };

            _groupMemberRepositoryMock.Setup(x => x.GetGroupMembersWithBalancesAsync(groupId))
                                      .ReturnsAsync(groupMembersWithBalances);

            // Act
            var result = await _service.GetGroupMembersWithBalancesAsync(groupId);

            // Assert
            Assert.Equal(groupMembersWithBalances.Count, result.Count());
            Assert.Equal(groupMembersWithBalances, result);
        }




        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ShouldReturnFalse_WhenGroupDoesNotExist()
        {
            // Arrange
            var model = new AddMemberRequest { GroupId = 1, Email = "test@example.com" };
            _groupRepositoryMock.Setup(x => x.GetGroup(model.GroupId))
                                .ReturnsAsync((Group)null);

            // Act
            var result = await _service.AddMemberByGroupIdAndEmail(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var model = new AddMemberRequest { GroupId = 1, Email = "test@example.com" };
            _groupRepositoryMock.Setup(x => x.GetGroup(model.GroupId))
                                .ReturnsAsync(new Group { GroupId = model.GroupId });

            _userRepositoryMock.Setup(x => x.GetUserByEmail(model.Email))
                               .ReturnsAsync((User)null);

            // Act
            var result = await _service.AddMemberByGroupIdAndEmail(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ShouldReturnFalse_WhenUserIsAlreadyMember()
        {
            // Arrange
            var model = new AddMemberRequest { GroupId = 1, Email = "test@example.com" };
            var user = new User { UserId = 1, Email = model.Email };

            _groupRepositoryMock.Setup(x => x.GetGroup(model.GroupId))
                                .ReturnsAsync(new Group { GroupId = model.GroupId });

            _userRepositoryMock.Setup(x => x.GetUserByEmail(model.Email))
                               .ReturnsAsync(user);

            _groupMemberRepositoryMock.Setup(x => x.GetGroupMember(model.GroupId, user.UserId))
                                      .ReturnsAsync(new GroupMember { GroupId = model.GroupId, UserId = user.UserId });

            // Act
            var result = await _service.AddMemberByGroupIdAndEmail(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddMemberByGroupIdAndEmail_ShouldReturnTrue_WhenUserIsAdded()
        {
            // Arrange
            var model = new AddMemberRequest { GroupId = 1, Email = "test@example.com" };
            var user = new User { UserId = 1, Email = model.Email };

            _groupRepositoryMock.Setup(x => x.GetGroup(model.GroupId))
                                .ReturnsAsync(new Group { GroupId = model.GroupId });

            _userRepositoryMock.Setup(x => x.GetUserByEmail(model.Email))
                               .ReturnsAsync(user);

            _groupMemberRepositoryMock.Setup(x => x.GetGroupMember(model.GroupId, user.UserId))
                                      .ReturnsAsync((GroupMember)null);

            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()))
                                      .ReturnsAsync(true);

            // Act
            var result = await _service.AddMemberByGroupIdAndEmail(model);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task GetGroupsWithBalancesByUserIdAsync_ShouldReturnGroupsWithBalances()
        {
            // Arrange
            var userId = 1;
            var groupIds = new List<int> { 1, 2 };
            var groupMembersWithBalances1 = new List<UserWithBalanceDto>
            {
                new UserWithBalanceDto { UserId = 1, Email = "user1@example.com", Balance = 100 },
                new UserWithBalanceDto { UserId = 2, Email = "user2@example.com", Balance = 200 }
            };
            var groupMembersWithBalances2 = new List<UserWithBalanceDto>
            {
                new UserWithBalanceDto { UserId = 3, Email = "user3@example.com", Balance = 300 },
                new UserWithBalanceDto { UserId = 4, Email = "user4@example.com", Balance = 400 }
            };

            _groupMemberRepositoryMock.Setup(x => x.GetGroupIdsByUserIdAsync(userId))
                                      .ReturnsAsync(groupIds);

            _groupMemberRepositoryMock.Setup(x => x.GetGroupMembersWithBalancesAsync(1))
                                      .ReturnsAsync(groupMembersWithBalances1);

            _groupMemberRepositoryMock.Setup(x => x.GetGroupMembersWithBalancesAsync(2))
                                      .ReturnsAsync(groupMembersWithBalances2);

            // Act
            var result = await _service.GetGroupsWithBalancesByUserIdAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(groupIds[0], result.First().GroupId);
            Assert.Equal(groupMembersWithBalances1.Count, result.First().Members.Count);
            Assert.Equal(groupIds[1], result.Last().GroupId);
            Assert.Equal(groupMembersWithBalances2.Count, result.Last().Members.Count);
        }
    }
}
