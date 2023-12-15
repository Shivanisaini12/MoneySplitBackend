
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading;
using System.Threading.Tasks;
using DomainLayer.Common;
using System.Net.Mail;
using DomainLayer.Data;
using Service_Layer.ICustomServices;
using Service_Layer.UnitOfWork;

namespace WebAPI
{
    public class EmailJob : IJob
    {
        private readonly IMailService _mail;
        private readonly IUnitOfWork _unitOfWork;
        public EmailJob(IMailService mail, IUnitOfWork unitOfWork)
        {
            _mail = mail;
            _unitOfWork = unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            return _unitOfWork.EmailJob.SendAsync();
        }
    }
}
