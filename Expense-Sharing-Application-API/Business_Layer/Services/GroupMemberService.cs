using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business_Layer.Services.IGroupMemberService;

namespace Business_Layer.Services
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public GroupMemberService(
            IGroupMemberRepository groupMemberRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<GroupMember>> GetAllGroupMembers()
        {
            return await _groupMemberRepository.GetAllGroupMembers();
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMembersByGroupId(int groupId)
        {
            return await _groupMemberRepository.GetGroupMembersByGroupId(groupId);
        }

        public async Task<bool> AddMemberByGroupIdAndEmail(AddMemberRequest model)
        {
            // Check if the group exists
            var group = await _groupRepository.GetGroup(model.GroupId);
            if (group == null)
            {
                return false;
            }

            // Check if the user exists
            var user = await _userRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                return false;
            }

            // Check if the user is already a member of the group
            var existingMember = await _groupMemberRepository.GetGroupMember(model.GroupId, user.UserId);
            if (existingMember != null)
            {
                return false;
            }

            // Add the user to the group
            var groupMember = new GroupMember
            {
                GroupId = model.GroupId,
                UserId = user.UserId
            };

            return await _groupMemberRepository.AddGroupMember(groupMember);
        }

        public async Task<IEnumerable<UserWithBalanceDto>> GetGroupMembersWithBalancesAsync(int groupId)
        {
            return await _groupMemberRepository.GetGroupMembersWithBalancesAsync(groupId);
        }

        public async Task<IEnumerable<int>> GetGroupIdsByUserIdAsync(int userId)
        {
            return await _groupMemberRepository.GetGroupIdsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<GroupWithMembersDto>> GetGroupsWithBalancesByUserIdAsync(int userId)
        {
            var groupIds = await _groupMemberRepository.GetGroupIdsByUserIdAsync(userId);

            var groupsWithBalances = new List<GroupWithMembersDto>();

            foreach (var groupId in groupIds)
            {
                var membersWithBalances = await _groupMemberRepository.GetGroupMembersWithBalancesAsync(groupId);

                groupsWithBalances.Add(new GroupWithMembersDto
                {
                    GroupId = groupId,
                    Members = membersWithBalances.ToList()
                });
            }

            return groupsWithBalances;
        }
    }
}