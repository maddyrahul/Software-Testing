using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repositories
{
    public interface IGroupRepository
    {
        Task<Group> CreateGroup(Group group);
        Task<Group> GetGroup(int id);
        Task<IEnumerable<Group>> GetAllGroups();
        Task<bool> DeleteGroup(int id);
        Task<bool> UpdateExpense(Expense updatedExpense);
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task<Expense> GetExpense(int id);
        Task<bool> AddExpense(ExpenseDto expenseDto);
        Task<bool> SettleExpense(int userId);

        Task<IEnumerable<Group>> GetGroupsByUserId(int userId);
    }

}
