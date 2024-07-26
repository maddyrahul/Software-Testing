using Data_Access_Layer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.DTO
{
    public class GroupWithMembersDtoTests
    {
        [Fact]
        public void GroupWithMembersDto_Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var groupId = 1;
            var members = new List<UserWithBalanceDto>
            {
                new UserWithBalanceDto { UserId = 1, Email = "user1@example.com", Balance = 100 },
                new UserWithBalanceDto { UserId = 2, Email = "user2@example.com", Balance = 200 }
            };

            // Act
            var groupWithMembersDto = new GroupWithMembersDto
            {
                GroupId = groupId,
                Members = members
            };

            // Assert
            Assert.Equal(groupId, groupWithMembersDto.GroupId);
            Assert.Equal(members, groupWithMembersDto.Members);
        }
    }
}
