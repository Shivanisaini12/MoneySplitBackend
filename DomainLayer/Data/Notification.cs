using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class Notification
{
    public int Id { get; set; }

    public int? Type { get; set; }

    public string? Details { get; set; }

    public string? Title { get; set; }

    public string? DetailsUrl { get; set; }

    public string? SentTo { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsRead { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsReminder { get; set; }

    public string? Code { get; set; }

    public string? NotificationType { get; set; }
}
