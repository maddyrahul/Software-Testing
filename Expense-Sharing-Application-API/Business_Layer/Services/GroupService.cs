using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Group> CreateGroup(Group group)
        {
            return await _groupRepository.CreateGroup(group);
        }

        public async Task<Group> GetGroup(int id)
        {
            return await _groupRepository.GetGroup(id);
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            return await _groupRepository.GetAllGroups();
        }

        public async Task<bool> DeleteGroup(int id)
        {
            return await _groupRepository.DeleteGroup(id);
        }

        public async Task<bool> UpdateExpense(int expenseId, Expense updatedExpense)
        {
            updatedExpense.ExpenseId = expenseId;
            return await _groupRepository.UpdateExpense(updatedExpense);
        }

        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            return await _groupRepository.GetAllExpenses();
        }

        public async Task<Expense> GetExpense(int id)
        {
            return await _groupRepository.GetExpense(id);
        }

        public async Task<bool> AddExpense(ExpenseDto expenseDto)
        {
            return await _groupRepository.AddExpense(expenseDto);
        }

        public async Task<bool> SettleExpense(int userId)
        {
            return await _groupRepository.SettleExpense(userId);
        }

        public async Task<IEnumerable<Group>> GetGroupsByUserId(int userId)
        {
            return await _groupRepository.GetGroupsByUserId(userId);
        }
    }
}
