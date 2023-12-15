using AutoMapper;
using AutoMapper.Execution;
using DomainLayer.Data;
using DomainLayer.DTO;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.IRepository;
using Repository_Layer.Repository;
using Service_Layer.ICustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static DomainLayer.Common.Enum;

namespace Service_Layer.CustomServices
{
    public class UserSettingService : Repository<DigestiveEmailUserSetting>, IUserSettingService
    {
        private readonly RxSplitterContext _context;
        private readonly IMapper _mapper;
        private readonly ISprocRepository _sprocRepo;
        public UserSettingService(RxSplitterContext context, IMapper mapper, ISprocRepository sprocRepo) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _sprocRepo = sprocRepo;
        }

        public async Task<List<DigestiveEmailPeriod>> GetAllEmailPeriodType()
        {
            return _context.DigestiveEmailPeriods.ToList();
        }

        public async Task<EmailNotification> GetEmailUserSettingByUserId(Guid userId)
        {
            var emailNotification = new EmailNotification();
            var accountSetting = _context.UserAccountSettings.Where(x => x.UserId == userId);
            if (accountSetting.Any())
            {
                emailNotification.Email = accountSetting.FirstOrDefault(x => x.AccountSettingId == ((int)NotificationType.Email)).IsActive;
                emailNotification.Notification = accountSetting.FirstOrDefault(x => x.AccountSettingId == ((int)NotificationType.Notification)).IsActive;
            }

            var query = await _context.DigestiveEmailUserSettings.Include(e => e.EmailType).Where(x => x.UserId == userId).ToListAsync();
            var mapped = _mapper.Map<List<DigestiveEmailUserSettingDTO>>(query);
            if (query.Count == 0)
            {
                query = new List<DigestiveEmailUserSetting>();
                var emailtype = _context.DigestiveEmailTypes.ToList();
                emailtype.ForEach(x =>
                {
                    mapped.Add(new DigestiveEmailUserSettingDTO
                    {
                        Id = 0,
                        UserId = userId,
                        EmailTypeId = x.Id,
                        IsActive = false,
                        EmailTypeName = x.EmailType
                    });
                });
            }
            else
            {
                foreach (var emailtype in query)
                {
                    var map = mapped.FirstOrDefault(x => x.Id == emailtype.Id);
                    if(map != null)
                    {
                        map.EmailTypeName = emailtype.EmailType.EmailType;
                    }
                }
            }


            emailNotification.UserSettings = mapped;

            return emailNotification;
        }

        public async Task<List<DigestiveEmailType>> GetAllEmailType()
        {
            return _context.DigestiveEmailTypes.ToList();
        }

        public async Task<List<AccountSetting>> GetAllNotificationsType()
        {
            return _context.AccountSettings.ToList();
        }

        public Task<IList<SP_GetAllDigestiveEmailForAccountSetting>> GetUserAccountSettingEmailChecks(Guid UserId)
        {
            var obj = _sprocRepo.GetStoredProcedure("SP_GetAllDigestiveEmailForAccountSetting")
                .WithSqlParams(
                (nameof(UserId), UserId))
                .ExecuteStoredProcedureAsync<SP_GetAllDigestiveEmailForAccountSetting>();

            return obj;
        }

        public bool GetUserAccountSettingNotificationChecks(Guid UserId)
        {
            return _context.UserAccountSettings.Any(x => x.UserId == UserId && x.AccountSettingId == 2);
        }

        public int UpdateUserAccountSetting(Guid userId, EmailNotification entity)
        {
            var querysetting = _context.UserAccountSettings.Where(x => x.UserId == userId).ToList();
            if (querysetting.Count > 0)
            {
                foreach (var item in querysetting)
                {
                    item.IsActive = item.AccountSettingId == ((int)NotificationType.Email) ? entity.Email : entity.Notification;
                }
            }
            else
            {
                for (int i = 1; i <= Enum.GetNames(typeof(NotificationType)).Length; i++)
                {
                    _context.UserAccountSettings.Add(new UserAccountSetting
                    {
                        UserId = userId,
                        AccountSettingId = i,
                        IsActive = i == 1 ? entity.Email : entity.Notification
                    });
                }
            }

            return _context.SaveChanges();
        }
    }
}
