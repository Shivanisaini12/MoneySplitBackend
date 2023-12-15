using System;
using System.Collections.Generic;

namespace DomainLayer.Data;

public partial class AccountSetting
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserAccountSetting> UserAccountSettings { get; } = new List<UserAccountSetting>();
}
