using Data_Access_Layer.Models;
using Data_Access_Layer.DTO;

namespace Data_Access_Layer.Repositories
{
    public interface IGroupMemberRepository
    {
        Task<IEnumerable<GroupMember>> GetAllGroupMembers();
        Task<IEnumerable<GroupMember>> GetGroupMembersByGroupId(int groupId);
        Task<GroupMember> GetGroupMember(int groupId, int userId);
        Task<bool> AddGroupMember(GroupMember groupMember);

        Task<IEnumerable<int>> GetGroupIdsByUserIdAsync(int userId);
        Task<IEnumerable<UserWithBalanceDto>> GetGroupMembersWithBalancesAsync(int groupId);
    }

}
