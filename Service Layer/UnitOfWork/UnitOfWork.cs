using AutoMapper;
using DomainLayer.Common;
using DomainLayer.Data;
using Repository_Layer.IRepository;
using Service_Layer.CustomServices;
using Service_Layer.ICustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Layer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserDetailService User { get;private set; }
        public IGroupService Group { get; private set; }
        public IGroupMemberService GroupMember { get; private set; }
        public IExpenseService Expense { get; private set; }
        public IMemberInvitationService  MemberInvitation { get; private set; }
        public ITransactionService Transaction { get; private set; }
        public INotificationService Notification { get; private set; }

        public IReportService Report { get; }

        public IUserSettingService UserSettingService { get; private set; }

        public IAccountSettingEmailNotificationService AccountSettingEmailNotificationService { get; private set; }
        public IEmailJobService EmailJob { get; private set; }


        public IActivityLogService ActivityLogServiceSave { get; private set; }

        private RxSplitterContext _context;
        private IMapper _mapper;
        private ISprocRepository _sprocRepo;
        private IMailService _mail;


        public UnitOfWork(RxSplitterContext context, IMapper mapper, ISprocRepository sprocRepo, IMailService mail)
        {
            _context = context;
            _mapper = mapper;
            _sprocRepo = sprocRepo;
            _mail = mail;
            User = new UserDetailService(context);
            Group = new GroupService(context, _sprocRepo);
            GroupMember = new GroupMemberService(context, _sprocRepo);
            Expense = new ExpenseService(context, _mapper,_sprocRepo);
            MemberInvitation = new MemberInvitationService(context, _mapper,_sprocRepo);
            Transaction = new TransactionService(context, _mapper, _sprocRepo);
            Notification = new NotificationService(context, _mapper,_sprocRepo);
            Report  = new ReportService(context, _mapper,_sprocRepo);
            UserSettingService = new UserSettingService(context, _mapper, _sprocRepo);
            AccountSettingEmailNotificationService = new AccountSettingEmailNotificationService(context);
            EmailJob = new EmailJobService(_mail,_mapper,_context,_sprocRepo);

            ActivityLogServiceSave = new ActivityLogService(context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
