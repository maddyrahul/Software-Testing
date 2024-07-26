using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public interface IGroupService
    {
        Task<Group> CreateGroup(Group group);
        Task<Group> GetGroup(int id);
        Task<IEnumerable<Group>> GetAllGroups();
        Task<bool> DeleteGroup(int id);
        Task<bool> UpdateExpense(int expenseId, Expense updatedExpense);
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task<Expense> GetExpense(int id);
        Task<bool> AddExpense(ExpenseDto expenseDto);
        Task<bool> SettleExpense(int userId);

        Task<IEnumerable<Group>> GetGroupsByUserId(int userId);
    }
}
