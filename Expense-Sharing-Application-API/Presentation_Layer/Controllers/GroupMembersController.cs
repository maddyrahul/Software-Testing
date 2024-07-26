using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Layer.Services;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Business_Layer.Services.IGroupMemberService;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        private readonly IGroupMemberService _groupMemberService;

        public GroupMembersController(IGroupMemberService groupMemberService)
        {
            _groupMemberService = groupMemberService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupMember>>> GetAllGroupMembers()
        {
            try
            {
                var groupMembers = await _groupMemberService.GetAllGroupMembers();
                if (groupMembers == null || !groupMembers.Any())
                {
                    return NotFound();
                }
                return Ok(groupMembers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IEnumerable<GroupMember>>> GetGroupMembersByGroupId(int groupId)
        {
            try
            {
                var groupMembers = await _groupMemberService.GetGroupMembersByGroupId(groupId);
                if (groupMembers == null || !groupMembers.Any())
                {
                    return NotFound();
                }
                return Ok(groupMembers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMemberByGroupIdAndEmail([FromBody] AddMemberRequest model)
        {
            try
            {
                var result = await _groupMemberService.AddMemberByGroupIdAndEmail(model);
                if (!result)
                {
                    return BadRequest("Failed to add member to the group.");
                }
                return Ok(new { Message = "User added to the group successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("group/user/{userId}/groups-with-balances")]
        public async Task<ActionResult<IEnumerable<GroupWithMembersDto>>> GetGroupsWithBalancesByUserId(int userId)
        {
            try
            {
                var groupsWithBalances = await _groupMemberService.GetGroupsWithBalancesByUserIdAsync(userId);
                if (groupsWithBalances == null || !groupsWithBalances.Any())
                {
                    return NotFound("The user is not a member of any groups.");
                }

                return Ok(groupsWithBalances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("WithBalances/{groupId}")]
        public async Task<ActionResult<IEnumerable<UserWithBalanceDto>>> GetGroupMembersWithBalances(int groupId)
        {
            try
            {
                var membersWithBalances = await _groupMemberService.GetGroupMembersWithBalancesAsync(groupId);
                if (membersWithBalances == null)
                {
                    return NotFound();
                }

                return Ok(membersWithBalances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
