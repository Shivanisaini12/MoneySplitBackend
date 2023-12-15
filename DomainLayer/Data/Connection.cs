using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class Connection
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public string? SignalrId { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
