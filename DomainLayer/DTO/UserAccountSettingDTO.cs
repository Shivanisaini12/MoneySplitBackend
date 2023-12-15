using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class UserAccountSettingDTO
    {
        public int AccountSettingId { get; set; }

        public Guid? UserId { get; set; }

        public bool IsActive { get; set; }
    }
}
