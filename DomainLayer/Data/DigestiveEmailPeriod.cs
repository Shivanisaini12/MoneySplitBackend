using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class DigestiveEmailPeriod
{
    public int Id { get; set; }

    public string? EmailTypeName { get; set; }

    public virtual ICollection<DigestiveEmailUserSetting> DigestiveEmailUserSettings { get; } = new List<DigestiveEmailUserSetting>();
}
