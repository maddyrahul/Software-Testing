using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Model
{
    public class UserTests
    {
        [Fact]
        public void User_AllProperties_CanBeSetAndGet()
        {
            // Arrange
            var user = new User();
            var groupMembers = new List<GroupMember>
        {
            new GroupMember { GroupMemberId = 1, UserId = 1, GroupId = 1 },
            new GroupMember { GroupMemberId = 2, UserId = 1, GroupId = 2 }
        };

            // Act
            user.UserId = 1;
            user.Email = "test@example.com";
            user.Password = "password";
            user.Role = "Admin";
            user.Balance = 100.50m;
            user.GroupMembers = groupMembers;

            // Assert
            Assert.Equal(1, user.UserId);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("password", user.Password);
            Assert.Equal("Admin", user.Role);
            Assert.Equal(100.50m, user.Balance);
            Assert.Equal(groupMembers, user.GroupMembers);
        }
    }
}
