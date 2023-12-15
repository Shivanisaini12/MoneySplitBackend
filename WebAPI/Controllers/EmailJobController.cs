using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Service_Layer.ICustomServices;
using Service_Layer.UnitOfWork;
using System.Net.Mail;
using System.Reflection.Metadata;
using WebAPI.Hub;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class EmailJobController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mail;
        public EmailJobController(IUnitOfWork unitOfWork, IMailService mail)
        {
            _unitOfWork = unitOfWork;
            _mail = mail;
        }

     
    }
}
