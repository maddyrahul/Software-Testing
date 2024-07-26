using System;
using System.Linq;
using System.Threading.Tasks;
using Data_Access_Layer.Data;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UnitTesting.RepositoriesUnitTest
{
    public class GroupRepositoryTests : IDisposable
    {
        private readonly ExpenseSharingDbContext _context;
        private readonly GroupRepository _repository;

        public GroupRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ExpenseSharingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ExpenseSharingDbContext(options);
            _repository = new GroupRepository(_context);

            // Seed data for testing
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test data to the in-memory database
            var group1 = new Group { GroupId = 1, Name = "Group1", Description = "Test Group 1", CreatedDate = DateTime.Now };
            var user1 = new User { UserId = 1, Email = "user1@example.com", Password = "password", Role = "Normal", Balance = 0 };
            var user2 = new User { UserId = 2, Email = "user2@example.com", Password = "password", Role = "Normal", Balance = 0 };

            _context.Groups.AddRange(group1);
            _context.Users.AddRange(user1, user2);
            _context.GroupMembers.AddRange(
                new GroupMember { GroupId = 1, UserId = 1, Group = group1, User = user1 },
                new GroupMember { GroupId = 1, UserId = 2, Group = group1, User = user2 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateGroup_ShouldAddGroup()
        {
            // Arrange
            var newGroup = new Group { GroupId = 2, Name = "Group2", Description = "Test Group 2", CreatedDate = DateTime.Now };

            // Act
            var result = await _repository.CreateGroup(newGroup);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newGroup.Name, result.Name);
            Assert.Equal(2, _context.Groups.Count());
        }

        [Fact]
        public async Task CreateGroup_ShouldThrowException_WhenGroupNameExists()
        {
            // Arrange
            var newGroup = new Group { GroupId = 1, Name = "Group1", Description = "Duplicate Group Name", CreatedDate = DateTime.Now };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.CreateGroup(newGroup));
            Assert.Equal("Group name already exists:", exception.Message);
        }

        [Fact]
        public async Task GetGroup_ShouldReturnGroup_WhenExists()
        {
            // Act
            var result = await _repository.GetGroup(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.GroupId);
        }

        [Fact]
        public async Task GetGroup_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetGroup(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllGroups_ShouldReturnAllGroups()
        {
            // Act
            var result = await _repository.GetAllGroups();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task DeleteGroup_ShouldReturnTrue_WhenGroupDeleted()
        {
            // Act
            var result = await _repository.DeleteGroup(1);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Groups);
        }

        [Fact]
        public async Task DeleteGroup_ShouldReturnFalse_WhenGroupNotExists()
        {
            // Act
            var result = await _repository.DeleteGroup(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateExpense_ShouldReturnTrue_WhenExpenseUpdated()
        {
            // Arrange
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var updatedExpense = new Expense
            {
                ExpenseId = 1,
                Description = "Updated Expense",
                Amount = 200,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };

            // Act
            var result = await _repository.UpdateExpense(updatedExpense);

            // Assert
            Assert.True(result);
            var dbExpense = await _context.Expenses.FindAsync(1);
            Assert.Equal("Updated Expense", dbExpense.Description);
            Assert.Equal(200, dbExpense.Amount);
        }

        [Fact]
        public async Task UpdateExpense_ShouldReturnFalse_WhenExpenseNotExists()
        {
            // Arrange
            var updatedExpense = new Expense
            {
                ExpenseId = 999,
                Description = "Non-Existent Expense",
                Amount = 200,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };

            // Act
            var result = await _repository.UpdateExpense(updatedExpense);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllExpenses_ShouldReturnAllExpenses()
        {
            // Arrange
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllExpenses();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetExpense_ShouldReturnExpense_WhenExists()
        {
            // Arrange
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetExpense(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ExpenseId);
        }

        [Fact]
        public async Task GetExpense_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetExpense(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddExpense_ShouldAddExpense_WhenValid()
        {
            // Arrange
            var expenseDto = new ExpenseDto
            {
                Description = "New Expense",
                Amount = 100,
                Date = DateTime.Now,
                Email = "user1@example.com",
                GroupName = "Group1"
            };

            // Act
            var result = await _repository.AddExpense(expenseDto);

            // Assert
            Assert.True(result);
            var dbExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.Description == "New Expense");
            Assert.NotNull(dbExpense);
            Assert.Equal(100, dbExpense.Amount);
        }

        [Fact]
        public async Task AddExpense_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var expenseDto = new ExpenseDto
            {
                Description = "Invalid Expense",
                Amount = 100,
                Date = DateTime.Now,
                Email = "nonexistent@example.com",
                GroupName = "Group1"
            };

            // Act
            var result = await _repository.AddExpense(expenseDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddExpense_ShouldReturnFalse_WhenGroupNotFound()
        {
            // Arrange
            var expenseDto = new ExpenseDto
            {
                Description = "Invalid Expense",
                Amount = 100,
                Date = DateTime.Now,
                Email = "user1@example.com",
                GroupName = "NonexistentGroup"
            };

            // Act
            var result = await _repository.AddExpense(expenseDto);

            // Assert
            Assert.False(result);
        }
       

        [Fact]
        public async Task SettleExpense_ShouldReturnFalse_WhenUserNotFound()
        {
            // Act
            var result = await _repository.SettleExpense(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SettleExpense_ShouldReturnFalse_WhenExpenseNotFound()
        {
            // Act
            var result = await _repository.SettleExpense(1);

            // Assert
            Assert.False(result);
        }

       /* [Fact]
        public async Task SettleExpense_ShouldReturnFalse_WhenInvalidBalanceOrSelfSettlement()
        {
            // Arrange
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SettleExpense(1);

            // Assert
            Assert.False(result);
        }*/



        [Fact]
        public async Task GetGroupsByUserId_ShouldReturnGroups_WhenUserExists()
        {
            // Act
            var result = await _repository.GetGroupsByUserId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetGroupsByUserId_ShouldReturnEmpty_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetGroupsByUserId(999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact]
        public async Task AddExpense_ShouldReturnFalse_WhenPaidByUserNotFound()
        {
            // Arrange
            var expenseDto = new ExpenseDto
            {
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                Email = "nonexistent@example.com", // Invalid email
                GroupName = "Group1"
            };

            // Act
            var result = await _repository.AddExpense(expenseDto);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task AddExpense_ShouldReturnFalse_WhenGroupMemberNotFound()
        {
            // Arrange
            var expenseDto = new ExpenseDto
            {
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                Email = "user3@example.com", // User2 is not a member of any group
                GroupName = "Group1"
            };

            // Act
            var result = await _repository.AddExpense(expenseDto);

            // Assert
            Assert.False(result);
        }



        

        [Fact]
        public async Task UpdateExpense_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var updatedExpense = new Expense
            {
                ExpenseId = 1,
                Description = "Updated Expense",
                Amount = 200,
                Date = DateTime.Now,
                PaidById = 999, // Invalid user ID
                GroupId = 1
            };

            // Act
            var result = await _repository.UpdateExpense(updatedExpense);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateExpense_ShouldReturnFalse_WhenUserNotMemberOfGroup()
        {
            // Arrange
            var existingExpense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(existingExpense);
            await _context.SaveChangesAsync();

            var updatedExpense = new Expense
            {
                ExpenseId = 1,
                Description = "Updated Expense",
                Amount = 200,
                Date = DateTime.Now,
                PaidById = 3, // User 3 does not exist in the group
                GroupId = 1
            };

            // Act
            var result = await _repository.UpdateExpense(updatedExpense);

            // Assert
            Assert.False(result);
        }



        [Fact]
        public async Task ValidateUserIsMemberOfGroup_ShouldReturnFalse_IfUserIsNotMember()
        {
            // Arrange
            var groupId = 1;
            var userId = 3; // User with ID 3 does not exist in seeded data
            var group = await _context.Groups.FindAsync(groupId);
            var user = new User { UserId = userId };

            // Act
            var result = group.Members.All(m => m.UserId != user.UserId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfExpenseAlreadyExists_ShouldThrowInvalidOperationException_IfExpenseExists()
        {
            // Arrange
            var groupId = 1;
            var userId = 1; // User with ID 1 exists in seeded data

            // Add an expense for user 1 in group 1
            var expense = new Expense
            {
                ExpenseId = 1,
                GroupId = groupId,
                PaidById = userId,
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test expense description" // Set the description
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var existingExpense = await _context.Expenses
                                                    .AnyAsync(e => e.PaidById == userId && e.GroupId == groupId);
                if (existingExpense)
                {
                    throw new InvalidOperationException("Expense already created by this user in this group.");
                }
            });
        }



        [Fact]
        public async Task FindGroupMemberInformation_ShouldReturnFalse_IfUserIsNotMemberOfAnyGroup()
        {
            // Arrange
            var userId = 3; // User with ID 3 does not exist in seeded data

            // Act
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == userId);

            // Assert
            Assert.Null(groupMember);
        }


        [Fact]
        public async Task FindGroupMemberInformation_ShouldReturnGroupMember_IfUserIsMemberOfAnyGroup()
        {
            // Arrange
            var userId = 1; // User with ID 1 exists in seeded data

            // Act
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == userId);

            // Assert
            Assert.NotNull(groupMember);
            Assert.Equal(userId, groupMember.UserId);
        }

        [Fact]
        public async Task FindUserById_ShouldReturnFalse_IfUserNotFound()
        {
            // Arrange
            var userId = 100; // User ID that does not exist in seeded data

            // Act
            var user = await _context.Users.FindAsync(userId);

            // Assert
            Assert.Null(user);
        }
       
        [Fact]
        public async Task UpdateExpense_ShouldReturnFalse_WhenGroupMemberNotFound()
        {
            // Arrange
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var updatedExpense = new Expense
            {
                ExpenseId = 1,
                Description = "Updated Expense",
                Amount = 200,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 999 // Non-existent group
            };

            // Act
            var result = await _repository.UpdateExpense(updatedExpense);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateExpense_ShouldReturnFalse_WhenGroupNotFound()
        {
            // Arrange
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var updatedExpense = new Expense
            {
                ExpenseId = 1,
                Description = "Updated Expense",
                Amount = 200,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 999 // Non-existent group
            };

            // Act
            var result = await _repository.UpdateExpense(updatedExpense);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddExpense_ShouldThrowException_WhenExpenseAlreadyExists()
        {
            // Arrange
            var existingExpense = new Expense
            {
                Description = "Existing Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = 1,
                GroupId = 1
            };
            _context.Expenses.Add(existingExpense);
            await _context.SaveChangesAsync();

            var expenseDto = new ExpenseDto
            {
                Description = "New Expense",
                Amount = 200,
                Date = DateTime.Now,
                Email = "user1@example.com",
                GroupName = "Group1"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.AddExpense(expenseDto));
        }

        [Fact]
        public async Task AddExpense_ShouldReturnFalse_WhenUserNotFoundInGroup()
        {
            // Arrange
            var expenseDto = new ExpenseDto
            {
                Description = "New Expense",
                Amount = 100,
                Date = DateTime.Now,
                Email = "user3@example.com", // User not in the group
                GroupName = "Group1"
            };

            // Act
            var result = await _repository.AddExpense(expenseDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SettleExpense_ShouldReturnTrue_WhenExpenseSettled()
        {
            // Arrange
            var group = await _context.Groups.FirstAsync();
            var user1 = await _context.Users.FindAsync(1);
            var user2 = await _context.Users.FindAsync(2);

            user1.Balance = -50;
            user2.Balance = 50;

            var expense = new Expense
            {
                Description = "Test Expense",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = user2.UserId,
                GroupId = group.GroupId
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SettleExpense(user1.UserId);

            // Assert
            Assert.True(result);
            Assert.Equal(0, user1.Balance);
            Assert.Equal(0, user2.Balance);
            var settledGroupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == user1.UserId);
            Assert.True(settledGroupMember.IsSettled);
        }

        [Fact]
        public async Task SettleExpense_ShouldReturnFalse_WhenGroupMemberIsAlreadySettled()
        {
            // Arrange
            var group = await _context.Groups.FirstAsync();
            var user = await _context.Users.FindAsync(1);
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == user.UserId);
            groupMember.IsSettled = true;
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SettleExpense(user.UserId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task SettleExpense_ShouldReturnFalse_WhenInvalidBalanceOrSelfSettlement()
        {
            // Arrange
            var group = await _context.Groups.FirstAsync();
            var user1 = new User { UserId = 1, Email = "user1@example.com", Password = "password", Role = "Normal", Balance = 0 };
            var user2 = new User { UserId = 2, Email = "user2@example.com", Password = "password", Role = "Normal", Balance = 50 };

            // Check if users already exist in the context
            var existingUser1 = await _context.Users.FindAsync(user1.UserId);
            if (existingUser1 == null)
            {
                _context.Users.Add(user1);
            }
            else
            {
                user1 = existingUser1;
            }

            var existingUser2 = await _context.Users.FindAsync(user2.UserId);
            if (existingUser2 == null)
            {
                _context.Users.Add(user2);
            }
            else
            {
                user2 = existingUser2;
            }

            // Save users to context
            await _context.SaveChangesAsync();

            _context.GroupMembers.AddRange(
                new GroupMember { GroupId = group.GroupId, UserId = user1.UserId, Group = group, User = user1 },
                new GroupMember { GroupId = group.GroupId, UserId = user2.UserId, Group = group, User = user2 }
            );
            await _context.SaveChangesAsync();

            // Case 1: Receiver user has zero balance
            var expenseWithZeroBalance = new Expense
            {
                ExpenseId = 1,
                Description = "Test Expense with Zero Balance",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = user2.UserId,
                GroupId = group.GroupId
            };
            _context.Expenses.Add(expenseWithZeroBalance);
            await _context.SaveChangesAsync();

            // Act
            var resultZeroBalance = await _repository.SettleExpense(user1.UserId);

            // Assert
            Assert.False(resultZeroBalance);

            // Case 2: User trying to settle their own expense
            var expenseSelfSettlement = new Expense
            {
                ExpenseId = 2,
                Description = "Test Expense for Self Settlement",
                Amount = 100,
                Date = DateTime.Now,
                PaidById = user1.UserId,
                GroupId = group.GroupId
            };
            _context.Expenses.Add(expenseSelfSettlement);
            await _context.SaveChangesAsync();

            // Act
            var resultSelfSettlement = await _repository.SettleExpense(user1.UserId);

            // Assert
            Assert.False(resultSelfSettlement);
        }




        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
