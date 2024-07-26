using Data_Access_Layer.Models;
using Data_Access_Layer.DTO;

namespace Business_Layer.Services
{
    public interface IGroupMemberService
    {
        Task<IEnumerable<GroupMember>> GetAllGroupMembers();
        Task<IEnumerable<GroupMember>> GetGroupMembersByGroupId(int groupId);
        Task<IEnumerable<UserWithBalanceDto>> GetGroupMembersWithBalancesAsync(int groupId);


        Task<bool> AddMemberByGroupIdAndEmail(AddMemberRequest model);

        Task<IEnumerable<int>> GetGroupIdsByUserIdAsync(int userId);
       

        public class AddMemberRequest
        {
            public int GroupId { get; set; }
            public string Email { get; set; }
        }

        Task<IEnumerable<GroupWithMembersDto>> GetGroupsWithBalancesByUserIdAsync(int userId);

    }
}
