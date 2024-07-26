using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.DTO
{
    public class GroupWithMembersDto
    {
        public int GroupId { get; set; }
        public List<UserWithBalanceDto> Members { get; set; }
    }
}
