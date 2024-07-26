using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // "Admin" or "Normal"
        public decimal Balance { get; set; }

        [JsonIgnore]
        public ICollection<GroupMember>? GroupMembers { get; set; }

    }
}
