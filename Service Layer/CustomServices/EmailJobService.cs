using AutoMapper;
using DomainLayer.Common;
using DomainLayer.Data;
using Quartz;
using Quartz.Impl;
using Repository_Layer.IRepository;
using Service_Layer.ICustomServices;
using Service_Layer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service_Layer.CustomServices
{
    public class EmailJobService : IEmailJobService
    {
        private readonly IMailService _mail;
        private readonly RxSplitterContext _context;
        private readonly IMapper _mapper;
        private readonly ISprocRepository _sproc;



        public EmailJobService(IMailService mail, IMapper mapper, RxSplitterContext context, ISprocRepository sproc)
        {
            _mail = mail;
            _mapper = mapper;
            _context = context;
            _sproc = sproc;
        }

        public async Task<bool> SendAsync()
        {
            var usersForNotify = _context.UserAccountSettings.Where(x => x.AccountSettingId == 1 && x.IsActive == true).ToList();
            if (usersForNotify != null && usersForNotify.Count > 0)
            {
                foreach (var user in usersForNotify)
                {
                    var userSetting = _context.DigestiveEmailUserSettings.Where(x => x.UserId == user.UserId && x.IsActive == true).ToList();
                    if (userSetting != null && userSetting.Count > 0)
                    {
                        var notifications = new List<Notification>();
                        foreach (var setting in userSetting)
                        {
                            var toDate = DateTime.Now;
                            var weekLastDate = CommonMethods.GetLastDayOfWeek(toDate);
                            var monthLastDate = CommonMethods.GetLastDayOfMonth(toDate);
                            var quarterLastDate = CommonMethods.GetLastDayOfQuarterMonth(toDate);
                            var yearLastDate = CommonMethods.GetLastDayOfYear(toDate);
                            var toData = _context.UserDetails.Where(x => x.Id == user.UserId && x.IsActive == true && x.IsDeleted == false).ToList();
                            string body = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"960\" align=\"center\" border=\"1\">";
                            switch (setting.EmailTypeId)
                            {
                                case 1:
                                    if (GetDateAccUserSettingEmailPeriod(Convert.ToInt32(setting.EmailPeriodId)) == toDate)
                                    {
                                        notifications = _context.Notifications.Where(x => x.Type == setting.EmailTypeId).ToList();
                                        if (notifications != null && notifications.Count > 0)
                                        {
                                            foreach (var item in notifications)
                                            {
                                                body += " <tr> <td> Member added in the group </td> </tr>";
                                                body += " <tr> <td> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Date + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Title + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"> <tr> <td>" + item.Details + "</td> </tr> </table> </td> </tr>";
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    if (GetDateAccUserSettingEmailPeriod(Convert.ToInt32(setting.EmailPeriodId)) == toDate)
                                    {
                                        notifications = _context.Notifications.Where(x => x.Type == setting.EmailTypeId).ToList();
                                        if (notifications != null && notifications.Count > 0)
                                        {
                                            foreach (var item in notifications)
                                            {
                                                body += " <tr> <td> Expense added in the group </td> </tr>";
                                                body += " <tr> <td> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Date + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Title + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"> <tr> <td>" + item.Details + "</td> </tr> </table> </td> </tr>";
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    if (GetDateAccUserSettingEmailPeriod(Convert.ToInt32(setting.EmailPeriodId)) == toDate)
                                    {
                                        notifications = _context.Notifications.Where(x => x.Type == setting.EmailTypeId).ToList();
                                        if (notifications != null && notifications.Count > 0)
                                        {
                                            foreach (var item in notifications)
                                            {
                                                body += " <tr> <td> Group leave by group member </td> </tr>";
                                                body += " <tr> <td> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Date + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Title + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"> <tr> <td>" + item.Details + "</td> </tr> </table> </td> </tr>";
                                            }
                                        }
                                    }
                                    break;
                                case 4:
                                    if (GetDateAccUserSettingEmailPeriod(Convert.ToInt32(setting.EmailPeriodId)) == toDate)
                                    {
                                        notifications = _context.Notifications.Where(x => x.Type == setting.EmailTypeId).ToList();
                                        if (notifications != null && notifications.Count > 0)
                                        {
                                            foreach (var item in notifications)
                                            {
                                                body += " <tr> <td> Member settleup the payment </td> </tr>";
                                                body += " <tr> <td> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Date + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Title + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"> <tr> <td>" + item.Details + "</td> </tr> </table> </td> </tr>";
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    if (GetDateAccUserSettingEmailPeriod(Convert.ToInt32(setting.EmailPeriodId)) == toDate)
                                    {
                                        notifications = _context.Notifications.Where(x => x.Type == setting.EmailTypeId).ToList();
                                        if (notifications != null && notifications.Count > 0)
                                        {
                                            foreach (var item in notifications)
                                            {
                                                body += " <tr> <td> Member confirm the payment </td> </tr>";
                                                body += " <tr> <td> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Date + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"><tr> <td>" + item.Title + "</td> </tr> </table> <table cellpadding=\"0\" cellspacing=\"0\" width=\"317\" align=\"left\" border=\"1\"> <tr> <td>" + item.Details + "</td> </tr> </table> </td> </tr>";
                                            }
                                        }
                                    }
                                    break;
                            }
                            body += "</table>";
                            SendEmail(toData[0].Email, body, toData[0].Name);
                            //notifications.AddRange(_context.Notifications.Where(x => x.Type == setting.EmailTypeId).ToList());
                        }
                    }
                }
            }
            return true;
        }

        public Task SendEmail(string toMail, string body, string Name)
        {
            UserDetail obj = new UserDetail();
            List<string> to = new List<string>();
            List<string> bcc = new List<string>();
            List<string> cc = new List<string>();
            to.Add(toMail);
            bcc.Add("jaydeep.sahu@radixweb.com");
            cc.Add("prashansa.khandelwal@radixweb.com");
            string subject = "Email Notification";
            string from = "svni7071@gmail.com";
            string displayName = obj.Name;
            MailData mailData = new MailData(to, subject, body, from, displayName);
            _mail.SendAsync(mailData, new CancellationToken());
            return Task.CompletedTask;
        }

        private DateTime GetDateAccUserSettingEmailPeriod(int EmailPeriod)
        {
            var date = DateTime.Now;
            switch (EmailPeriod)
            {
                case 1:
                    date = DateTime.Now;
                    break;
                case 2:
                    date = CommonMethods.GetLastDayOfWeek(date);
                    break;
                case 3:
                    date = CommonMethods.GetLastDayOfQuarterMonth(date);
                    break;
                case 4:
                    date = CommonMethods.GetLastDayOfMonth(date);
                    break;
                case 5:
                    date = CommonMethods.GetLastDayOfYear(date);
                    break;
            }
            return date;
        }
    }
}
