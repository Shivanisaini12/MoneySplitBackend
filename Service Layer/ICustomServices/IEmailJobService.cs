using DomainLayer.Common;
using DomainLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Layer.ICustomServices
{
    public interface IEmailJobService
    {
        Task SendEmail(string toMail, string body, string Name);
        Task<bool> SendAsync();
    }
}
