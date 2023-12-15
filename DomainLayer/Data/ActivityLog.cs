using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class ActivityLog
{
    public int Id { get; set; }

    public string? UserId { get; set; }

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
