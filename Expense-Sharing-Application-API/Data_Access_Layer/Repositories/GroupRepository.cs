using Data_Access_Layer.Data;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Data_Access_Layer.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ExpenseSharingDbContext _context;

        public GroupRepository(ExpenseSharingDbContext context)
        {
            _context = context;
        }

        public async Task<Group> CreateGroup(Group group)
        {
            try
            {
                // Check if the group name already exists
                if (await _context.Groups.AnyAsync(g => g.Name == group.Name))
                {
                    throw new InvalidOperationException("Group name already exists:");
                }

                _context.Groups.Add(group);
                await _context.SaveChangesAsync();

                return group;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{ex.Message}");
            }
        }

        public async Task<Group> GetGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            return group;
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            var groups = await _context.Groups.ToListAsync();
            return groups;
        }

        public async Task<bool> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return false;
            }


            // Find the group based on the provided group ID
            var group1 = await _context.Groups.Include(g => g.Members)
                                             .FirstOrDefaultAsync(g => g.GroupId == id);
          
            foreach (var member in group1.Members)
            {
                var user = await _context.Users.FindAsync(member.UserId);
                user.Balance = 0;
            }

                _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateExpense(Expense updatedExpense)
        {
            var existingExpense = await _context.Expenses.FindAsync(updatedExpense.ExpenseId);
            if (existingExpense == null)
            {
                return false; // Existing expense not found
            }

            // Find the user who paid for the updated expense
            var paidByUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == updatedExpense.PaidById);
            if (paidByUser == null)
            {
                return false; // User not found
            }

            // Find the group member information for the user
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == paidByUser.UserId);
            if (groupMember == null)
            {
                return false; // User is not a member of any group
            }

            // Find the group based on the provided group ID
            var group = await _context.Groups.Include(g => g.Members)
                                             .FirstOrDefaultAsync(g => g.GroupId == updatedExpense.GroupId);
            if (group == null)
            {
                return false; // Group not found
            }

            // Validate that the user is a member of the group
            if (group.Members.All(m => m.UserId != paidByUser.UserId))
            {
                return false; // User is not a member of the specified group
            }

            // Calculate the old and new share amounts
            var oldShareAmount = existingExpense.Amount / group.Members.Count;
            var newShareAmount = updatedExpense.Amount / group.Members.Count;

            // Update balances for all group members
            foreach (var member in group.Members)
            {
                var user = await _context.Users.FindAsync(member.UserId);

                if (user.UserId == existingExpense.PaidById)
                {
                    user.Balance += oldShareAmount * (group.Members.Count - 1);
                    user.Balance -= newShareAmount * (group.Members.Count - 1);
                }
                else
                {
                    user.Balance -= oldShareAmount;
                    user.Balance += newShareAmount;
                }
            }

            // Update the existing expense with new details
            existingExpense.Description = updatedExpense.Description;
            existingExpense.Amount = updatedExpense.Amount;
            existingExpense.Date = updatedExpense.Date;
            existingExpense.PaidById = updatedExpense.PaidById;
            existingExpense.GroupId = updatedExpense.GroupId;

            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return expenses;
        }

        public async Task<Expense> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            return expense;
        }

        public async Task<bool> AddExpense(ExpenseDto expenseDto)
        {
            // Find the user who paid for the expense
            var paidByUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == expenseDto.Email);
            if (paidByUser == null)
            {
                return false; // User not found
            }

            // Find the group member information for the user
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == paidByUser.UserId);
            if (groupMember == null)
            {
                return false; // User is not a member of any group
            }

            // Find the group based on the provided group name
            var group = await _context.Groups.Include(g => g.Members)
                                             .FirstOrDefaultAsync(g => g.Name == expenseDto.GroupName);
            if (group == null)
            {
                return false; // Group not found
            }

           

            // Check if an expense already exists for this user in this group
            var existingExpense = await _context.Expenses
                                               .AnyAsync(e => e.PaidById == paidByUser.UserId && e.GroupId == group.GroupId);
            if (existingExpense)
            {
                throw new InvalidOperationException("Expense already created by this user in this group.");
            }

            // Create a new expense object
            var expense = new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                Date = expenseDto.Date,
                PaidById = paidByUser.UserId,
                GroupId = group.GroupId,
            };

            // Calculate the share amount
            var shareAmount = expense.Amount / group.Members.Count;

            // Update balances for all group members
            foreach (var member in group.Members)
            {
                var user = await _context.Users.FindAsync(member.UserId);
               

                if (user.UserId == expense.PaidById)
                {
                    user.Balance -= shareAmount * (group.Members.Count - 1);
                }
                else
                {
                    user.Balance += shareAmount;
                }
            }

            // Add the expense to the context and save changes
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return true;
        }





        public async Task<bool> SettleExpense(int userId)
        {
            // Find the group member entry for the specified user
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == userId);
            if (groupMember == null)
            {
                return false; // Group member not found
            }

            // Ensure the group ID is valid
            var groupId = groupMember.GroupId;
            if (groupId == null)
            {
                return false; // Invalid group ID
            }

            // Find an expense paid within the group
            var userExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.GroupId == groupId);
           
            if (userExpense == null)
            {
                return false; // Expense not found
            }

            // Get the ID of the user who paid the expense
            var paidById = userExpense.PaidById;

            // Check if the expense is already settled
            if (groupMember.IsSettled)
            {
                return false; // Expense already settled
            }

            // Find the users involved in the settlement
            var user = await _context.Users.FindAsync(userId);
            var receiverUser = await _context.Users.FindAsync(paidById);
            if (user == null || receiverUser == null)
            {
                return false; // User or receiver not found
            }

            // Ensure the receiver's balance is non-zero and the user is not settling their own balance
            if (receiverUser.Balance == 0 || user.UserId == receiverUser.UserId)
            {
                return false; // Invalid balance or self-settlement attempt
            }

            // Adjust balances
            receiverUser.Balance += user.Balance;
            user.Balance = 0;

            // Mark the expense as settled
            groupMember.IsSettled = true;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true; // Successfully settled the expense
        }


        public async Task<IEnumerable<Group>> GetGroupsByUserId(int userId)
        {
            var groups = await _context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();

            return groups;
        }


       

    }
}