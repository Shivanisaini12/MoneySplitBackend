using AutoMapper;
using AutoMapper.Execution;
using DomainLayer.Data;
using DomainLayer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.IRepository;
using Repository_Layer.Repository;
using Service_Layer.ICustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service_Layer.CustomServices
{
    public class NotificationService : Repository<Notification>, INotificationService
    {
        private readonly RxSplitterContext _context;
        private readonly ISprocRepository _sprocRepo;
        private readonly IMapper _mapper;

        public NotificationService(RxSplitterContext context, IMapper mapper, ISprocRepository sprocRepo) : base(context)
        {

            _context = context;
            _sprocRepo = sprocRepo;
            _mapper = mapper;
        }

        #region ExpenseNotification
        public List<Notification> ExpenseNotification(ExpensesDTO expenseDTO)
        {
            List<Notification> notificationsList = new List<Notification>();
            for (int i=0; i < expenseDTO.lstExpenseTransaction.Count; i++)
            {
                Notification notification = new Notification();
                notification.Title = "Expense";
                notification.Date = DateTime.UtcNow;
                notification.IsRead = false;
                notification.IsDeleted = false;
                notification.Type = 2;
                notification.SentTo = (_context.GroupMembers.FirstOrDefault(c=>c.Id==expenseDTO.lstExpenseTransaction[i].ParticipantMemberId).UserId).ToString();
                //string name = _context.UserDetails.FirstOrDefault(x => x.Email == (_context.GroupMembers.Include(t => t.User).FirstOrDefault(x=>x.Id==expenseDTO.PaidBy).Email)).Name;
                string name = _context.GroupMembers.Include(t => t.User).FirstOrDefault(x => x.Id == expenseDTO.PaidBy).User.Name;
                notification.Details = name + " Added an Expense of " + expenseDTO.Amount + " and your share is " + expenseDTO.lstExpenseTransaction[i].Amount;
                notificationsList.Add(notification);
            }
            _context.Notifications.AddRange(notificationsList);
            _context.SaveChanges();
           
            return notificationsList;
        }
        #endregion


        #region LeaveGroupNotification
        public List<Notification> LeaveGroupNotification(int memberId, Guid userId)
        {
            List<Notification> notificationsList = new List<Notification>();
            Notification notification = new Notification();
                notification.Title = "Leave Group";
                notification.Date = DateTime.UtcNow;
                notification.IsRead = false;
                notification.IsDeleted = false;
                notification.Type = 3;
                notification.SentTo = _context.Groups.FirstOrDefault(x=>x.Id== (_context.GroupMembers.FirstOrDefault(c => c.Id == memberId)).GroupId).AddedBy.ToString();
           
            string GroupName = _context.Groups.FirstOrDefault(x => x.Id == (_context.GroupMembers.FirstOrDefault(c => c.Id == memberId)).GroupId).GroupName;

        
            string MemberName =  _context.UserDetails.FirstOrDefault(m => m.Id == userId).Name;


            notification.Details = MemberName + ' ' + "Leave " + GroupName + " group"  ;
                notificationsList.Add(notification);
           
            _context.Notifications.AddRange(notificationsList);
            _context.SaveChanges();
           
            return notificationsList;
        }
        #endregion

        #region AddMemberNotification
        public List<Notification> AddMemberNotification(int GroupId, string mEmail)
        {
            List<Notification> notificationsList = new List<Notification>();
            Notification notification = new Notification();
            notification.Title = "Add Member";
            notification.Date = DateTime.UtcNow;
            notification.IsRead = false;
            notification.IsDeleted = false;
            notification.Type = 1;


            notification.SentTo = _context.GroupMembers.FirstOrDefault(x => x.Email == mEmail && x.GroupId== GroupId).UserId.ToString();

            string GroupName = _context.Groups.FirstOrDefault(x => x.Id == GroupId).GroupName;
            string MemberName = _context.UserDetails.FirstOrDefault(m => m.Email == mEmail).Name;

            notification.Details = MemberName + "member of this "  + GroupName + " group" ;
            notificationsList.Add(notification);


            _context.Notifications.AddRange(notificationsList);
            _context.SaveChanges();
           
            return notificationsList;
        }
        #endregion

        #region SettleUpNotification
        public List<Notification> SettleUpNotification(int GroupId, int memberId, string mEmail, decimal PaidToAmount, string PaidToName,string PaidByName)
        {
            List<Notification> notificationsList = new List<Notification>();
            Notification notification = new Notification();
            notification.Title = "Settle Up";
            notification.Date = DateTime.UtcNow;
            notification.IsRead = false;
            notification.IsDeleted = false;
            notification.Type = 4;
            notification.SentTo =_context.GroupMembers.FirstOrDefault(c => c.Id == memberId).UserId.ToString();

            string GroupName = _context.Groups.FirstOrDefault(x => x.Id == GroupId).GroupName;
            PaidByName = _context.UserDetails.FirstOrDefault(x => x.Email == PaidByName).Name;
            string MemberName = _context.UserDetails.FirstOrDefault(m => m.Email == mEmail).Name;

            notification.Details = PaidByName +  " have paid of Amount " + PaidToAmount + " from group name " + GroupName + " to " + PaidToName;
            notificationsList.Add(notification);

            _context.Notifications.AddRange(notificationsList);
            _context.SaveChanges();
          
            return notificationsList;
        }
        #endregion

        #region AcceptTransactionNotification
        public List<Notification> AcceptTransactionNotification(Guid userid, int TransactionId)
        {
           
                List<Notification> notificationsList = new List<Notification>();
            var trans = _context.Transactions.FirstOrDefault(x => x.Id == TransactionId);
            if (trans != null)
            {
                Notification notification = new Notification();
                notification.Title = "Accept Transaction";
                notification.Date = DateTime.UtcNow;
                notification.IsRead = false;
                notification.IsDeleted = false;
                notification.Type = 5;
                notification.SentTo = _context.GroupMembers.FirstOrDefault(c => c.Id == trans.PaidFromMemberId).UserId.ToString();

                string GroupName = _context.Groups.FirstOrDefault(t => t.Id == (_context.GroupMembers.FirstOrDefault(x => x.Id == trans.PaidToMemberId)).GroupId).GroupName;

                string MemberName = _context.UserDetails.FirstOrDefault(x => x.Email == (_context.GroupMembers.FirstOrDefault(t => t.Id == trans.PaidToMemberId)).Email).Name;

                notification.Details = MemberName + "Accept the transaction for group " + GroupName;
                notificationsList.Add(notification);

                _context.Notifications.AddRange(notificationsList);
                _context.SaveChanges();
            }
            return notificationsList;
        }
        #endregion

        public List<Notification> GetNotifications(Guid userId)
        {
            List<Notification> lstUser = new List<Notification>();
            //pageNo = pageNo * 10;
            try
            {
                lstUser = _context.Notifications.Where(x=>x.SentTo==(userId.ToString()) && x.IsRead==false).OrderByDescending(x=>x.Date).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstUser;
        }
        public int GetNotificationsCount(string userId)
        {
            var count = _context.Notifications.Where(x => x.SentTo == userId && x.IsRead == false).ToList();
            //var count = _context.Notifications.Count(x => x.SentTo == userId);
            return count.Count;
        }

        public int ReadNotification(int id)
        {
            _context.Notifications.FirstOrDefault(x => x.Id == id).IsRead = true;
            return (_context.SaveChanges());
            
        }
    }
}
