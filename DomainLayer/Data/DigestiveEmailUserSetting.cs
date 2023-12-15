using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class DigestiveEmailUserSetting
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public int? EmailPeriodId { get; set; }

    public int? EmailTypeId { get; set; }

    public bool? IsActive { get; set; }

    public virtual DigestiveEmailPeriod? EmailPeriod { get; set; }

    public virtual DigestiveEmailType? EmailType { get; set; }

    public virtual UserDetail? User { get; set; }
}
