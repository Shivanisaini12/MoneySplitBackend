using DomainLayer.Data;
using DomainLayer.DTO;
using Repository_Layer.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Layer.ICustomServices
{
    public interface IUserSettingService : IRepository<DigestiveEmailUserSetting>
    {
        Task<List<DigestiveEmailPeriod>> GetAllEmailPeriodType();

        Task<List<DigestiveEmailType>> GetAllEmailType();
        Task<List<AccountSetting>> GetAllNotificationsType();
        Task<IList<SP_GetAllDigestiveEmailForAccountSetting>> GetUserAccountSettingEmailChecks(Guid UserId);
        Task<EmailNotification> GetEmailUserSettingByUserId(Guid userId);
        int UpdateUserAccountSetting(Guid userId, EmailNotification entity);
        bool GetUserAccountSettingNotificationChecks(Guid UserId);
    }
}
