using DomainLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hub;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductOfferController : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHubClient> messageHub;
        public ProductOfferController(IHubContext<MessageHub, IMessageHubClient> _messageHub)
        {
            messageHub = _messageHub;
        }
        [HttpPost]
        [Route("productoffers")]
        public string Get()
        {
            List<Notification> offers = new List<Notification>();
            
            messageHub.Clients.All.SendOffersToUser(offers);

            messageHub.Clients.All.SendLeaveToUser(offers);

            messageHub.Clients.All.SendMemberAdd(offers);

            return "Offers sent successfully to all users!";
        }
    }
}

