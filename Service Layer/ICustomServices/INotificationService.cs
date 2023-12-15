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
    public interface INotificationService:IRepository<Notification>
    {
        List<Notification> ExpenseNotification(ExpensesDTO expenseDTO);
        List<Notification> LeaveGroupNotification(int memberId, Guid userId);
        List<Notification> AddMemberNotification(int GroupId, string mEmail);
        List<Notification> GetNotifications(Guid userId);
        int GetNotificationsCount(string userId);
        List<Notification> SettleUpNotification(int GroupId, int memberId, string mEmail, decimal PaidToAmount, string PaidToName,string PaidByName);
        int ReadNotification(int id);
        List<Notification> AcceptTransactionNotification(Guid userid, int TransactionId);
    }
}
