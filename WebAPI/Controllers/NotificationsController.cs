using DomainLayer.Common;
using DomainLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service_Layer.ICustomServices;
using Service_Layer.UnitOfWork;
using System.Security.Claims;
using WebAPI.Hub;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mail;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        public NotificationsController(IUnitOfWork unitOfWork, IMailService mail, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mail = mail;
            _hubContext = hubContext;
        }
        [HttpGet("notificationcount")]
        public IActionResult GetNotificationCount(string userId)
        {
           // var count = _unitOfWork.Notification.GetAll().Result.Count();
            var count = _unitOfWork.Notification.GetNotificationsCount(userId);
            NotificationCountResult result = new NotificationCountResult
            {
                Count = count
            };
            return Ok(count);
        }

        // GET: api/Notifications/notificationresult  
        [HttpGet(nameof(GetNotificationMessage))]
        public async Task<IActionResult> GetNotificationMessage()
        {
            var obj = await _unitOfWork.Notification.GetAll();
            if (obj == null)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "No Data Found" });
            }
            else
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
        }
        [HttpPost]
        [Route("getAllNotification/{pageNo}")]
        public List<Notification> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
          
                var userClaims = identity.Claims;
                Guid UserId = new Guid(userClaims.FirstOrDefault(x => x.Type == "Id").Value);
                return _unitOfWork.Notification.GetNotifications(UserId);
        }

        [HttpGet]
        [Route("ReadNotification")]
        public IActionResult ReadNotification(int id)
        {
            var obj = _unitOfWork.Notification.ReadNotification(id);
            if (obj == null)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "No Data Found" });
            }
            else
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
        }
        // DELETE: api/Notifications/deletenotifications  
        //[HttpDelete]
        //[Route("deletenotifications")]
        //public async Task<IActionResult> DeleteNotifications()
        //{
        //    await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Notification");
        //    await _context.SaveChangesAsync();
        //    await _hubContext.Clients.All.BroadcastMessage();

        //    return NoContent();
        //}
    }
}
