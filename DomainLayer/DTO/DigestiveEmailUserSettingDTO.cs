using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class DigestiveEmailUserSettingDTO
    {
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        public int? EmailPeriodId { get; set; }

        public int EmailTypeId { get; set; }

        public bool IsActive { get; set; }

        public string EmailTypeName { get; set; }
    }
}
