using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Access_Layer.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public ICollection<GroupMember>? Members { get; set; }

        [JsonIgnore]
        public ICollection<Expense>? Expenses { get; set; }
    }
}
