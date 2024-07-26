using Data_Access_Layer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.DTO
{
    public class UserWithBalanceDtoTests
    {
        [Fact]
        public void UserWithBalanceDto_ConstructsCorrectly()
        {
            // Arrange
            int userId = 1;
            string email = "test@example.com";
            decimal balance = 100.0m;

            // Act
            var user = new UserWithBalanceDto
            {
                UserId = userId,
                Email = email,
                Balance = balance
            };

            // Assert
            Assert.Equal(userId, user.UserId);
            Assert.Equal(email, user.Email);
            Assert.Equal(balance, user.Balance);
        }
    }

}
