using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class EmailNotification
    {
        public bool Email { get; set; }
        public bool Notification { get; set; }
        public List<DigestiveEmailUserSettingDTO> UserSettings { get; set; }
    }
}
