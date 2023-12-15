using DomainLayer.Data;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hub
{
    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task SendOffersToUser(List<Notification> message)
        {
            await Clients.All.SendOffersToUser(message);
           // await Clients.User("46BA5AA5-73A7-425D-A99B-71999D5A769A").SendOffersToUser(message);
        }

        public async Task sendAllNotifications(List<Notification> notifications)
        {
            await Clients.All.sendAllNotifications(notifications);
        }

        public async Task SendLeaveToUser(List<Notification> notification)
        {
            await Clients.All.SendLeaveToUser(notification);
        }
        public async Task SendMemberAdd(List<Notification> messages)
        {
            await Clients.All.SendMemberAdd(messages);
           
        }
        public async Task SendSettleUp(List<Notification> messages)
        {
            await Clients.All.SendSettleUp(messages);
        }

        public async Task AcceptTrans(List<Notification> msg)
        {
            await Clients.All.AcceptTrans(msg);
        }
    }
}
