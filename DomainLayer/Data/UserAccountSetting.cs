using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class UserAccountSetting
{
    public int Id { get; set; }

    public int AccountSettingId { get; set; }

    public Guid? UserId { get; set; }

    public bool IsActive { get; set; }

    public virtual AccountSetting AccountSetting { get; set; } = null!;

    public virtual UserDetail? User { get; set; }
}
