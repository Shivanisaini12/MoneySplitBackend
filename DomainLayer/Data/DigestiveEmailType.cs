using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class DigestiveEmailType
{
    public int Id { get; set; }

    public string? EmailType { get; set; }

    public virtual ICollection<DigestiveEmailUserSetting> DigestiveEmailUserSettings { get; } = new List<DigestiveEmailUserSetting>();
}
