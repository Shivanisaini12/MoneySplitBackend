using DomainLayer.Data;

namespace WebAPI.Hub
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(List<Notification> message);
        Task SendLeaveToUser(List<Notification> notification);
        Task sendAllNotifications(List<Notification> message);
        Task SendMemberAdd(List<Notification> messages);
        Task SendSettleUp(List<Notification> messages);
        Task AcceptTrans(List<Notification> msg);
    }
}
