using Business_Layer.Services;
using Data_Access_Layer.DTO;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(Group group)
        {
            try
            {
                var createdGroup = await _groupService.CreateGroup(group);
                return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.GroupId }, createdGroup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            try
            {
                var group = await _groupService.GetGroup(id);
                if (group == null)
                {
                    return NotFound();
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetAllGroups()
        {
            try
            {
                var groups = await _groupService.GetAllGroups();
                if (groups == null || !groups.Any())
                {
                    return NotFound();
                }
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("expenses/{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, Expense updatedExpense)
        {
            if (expenseId != updatedExpense.ExpenseId)
            {
                return BadRequest("Expense ID in the URL must match the ID in the request body.");
            }

            try
            {
                var result = await _groupService.UpdateExpense(expenseId, updatedExpense);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Expense updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("expenses")]
        public async Task<IActionResult> AddExpense(ExpenseDto expenseDto)
        {
            try
            {
                var result = await _groupService.AddExpense(expenseDto);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Expense added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("expenses/{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            try
            {
                var expense = await _groupService.GetExpense(id);
                if (expense == null)
                {
                    return NotFound();
                }
                return Ok(expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("settle-expense/{userId}")]
        public async Task<IActionResult> SettleExpense(int userId)
        {
            try
            {
                var result = await _groupService.SettleExpense(userId);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Expense settled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("expenses")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses()
        {
            var expenses = await _groupService.GetAllExpenses();
            if (expenses == null || !expenses.Any())
            {
                return NotFound();
            }
            return Ok(expenses);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var result = await _groupService.DeleteGroup(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Group deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByUserId(int userId)
        {
            try
            {
                var groups = await _groupService.GetGroupsByUserId(userId);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


