using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTO
{
    public class ActivityLogDTO
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? ResponseMessage { get; set; }

        public string? Ipaddress { get; set; }

        public string? RequestUrl { get; set; }

        public string? RequestBody { get; set; }

        public string? RequestHost { get; set; }

        public string? RequestScheme { get; set; }

        public string? RequestMethod { get; set; }

        public string? ResponseBody { get; set; }

        public string? ResponseStatus { get; set; }

        public DateTime? LogDatetime { get; set; }


    }
}
