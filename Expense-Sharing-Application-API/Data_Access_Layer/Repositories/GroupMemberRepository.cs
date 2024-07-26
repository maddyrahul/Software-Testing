using Data_Access_Layer.Data;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ExpenseSharingDbContext _context;

        public GroupMemberRepository(ExpenseSharingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupMember>> GetAllGroupMembers()
        {
            return await _context.GroupMembers
                .Include(gm => gm.Group)
                .Include(gm => gm.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMembersByGroupId(int groupId)
        {
            return await _context.GroupMembers
                .Include(gm => gm.User)
                .Where(gm => gm.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<GroupMember> GetGroupMember(int groupId, int userId)
        {
            return await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
        }

        public async Task<bool> AddGroupMember(GroupMember groupMember)
        {
           
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<int>> GetGroupIdsByUserIdAsync(int userId)
        {
            return await _context.GroupMembers
                .Where(gm => gm.UserId == userId)
                .Select(gm => gm.GroupId)
                .ToListAsync();
        }


        public async Task<IEnumerable<UserWithBalanceDto>> GetGroupMembersWithBalancesAsync(int groupId)
        {
            return await _context.GroupMembers
                .Where(gm => gm.GroupId == groupId)
                .Include(gm => gm.User)
                .Select(gm => new UserWithBalanceDto
                {
                    UserId = gm.UserId,
                    Email = gm.User.Email,
                    Balance = gm.User.Balance
                })
                .ToListAsync();
        }

    }
}
