using DomainLayer.Data;
using Repository_Layer.IRepository;
using Repository_Layer.Repository;
using Service_Layer.ICustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Layer.CustomServices
{
    public class AccountSettingEmailNotificationService : Repository<UserAccountSetting>, IAccountSettingEmailNotificationService
    {
        private readonly RxSplitterContext _context;
        
        public AccountSettingEmailNotificationService(RxSplitterContext context) : base(context)
        {
            _context = context;
            
        }
    }
}
