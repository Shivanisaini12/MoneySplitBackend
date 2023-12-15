using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class SP_GetAllDigestiveEmailForAccountSetting
    {
        public int EmailPeriodId { get; set; }
        public int EmailTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
