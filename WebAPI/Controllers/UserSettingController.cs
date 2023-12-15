using AutoMapper;
using DomainLayer.Common;
using DomainLayer.Data;
using DomainLayer.DTO;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository_Layer.IRepository;
using Service_Layer.UnitOfWork;
using System.Security.Claims;
using System.Text.Json;
using static DomainLayer.Common.Enum;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class UserSettingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserSettingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpPost(nameof(EmailUserSetting))]
        public async Task<IActionResult> EmailUserSetting([FromBody] EmailNotification settingsDTO)
        {
            try
            {
                //string json = System.Text.Json.JsonSerializer.Serialize(settings);
                //var settingsDTO = JsonConvert.DeserializeObject<EmailNotification>(json);

                //List<DigestiveEmailType> emailTypes = JsonConvert.DeserializeObject<List<DigestiveEmailType>>(json);

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null && settingsDTO != null)
                {
                    var userClaims = identity.Claims;
                    Guid UserId = new Guid(userClaims.FirstOrDefault(x => x.Type == "Id").Value);
                    bool res = false, result = false;

                    _unitOfWork.UserSettingService.UpdateUserAccountSetting(UserId, settingsDTO);
                    var setting = new DigestiveEmailUserSetting();
                    foreach (var item in settingsDTO.UserSettings)
                    {
                        item.UserId = UserId;
                        setting = _mapper.Map<DigestiveEmailUserSetting>(item);
                        var existSetting = await _unitOfWork.UserSettingService.GetT(x => x.UserId == item.UserId && x.EmailTypeId == item.EmailTypeId);
                        if (existSetting == null)
                        {
                            _unitOfWork.UserSettingService.Insert(setting);
                        }
                        else
                        {
                            existSetting.EmailPeriodId = item.EmailPeriodId;
                            existSetting.IsActive = item.IsActive;
                            _unitOfWork.UserSettingService.Update(existSetting);
                        }
                        _unitOfWork.Save();
                    }
                    if (result)
                    {
                        return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = "User setting activity added successfully." });
                    }
                    else
                    {
                        return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Failure", Response = "User setting activity updated successfully." });
                    }
                }
                else
                {
                    return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "The Data given by you is totally empty.." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "Data Not Found." });
            }
        }

        [HttpGet(nameof(GetAllEmailPeriodType))]
        public async Task<IActionResult> GetAllEmailPeriodType()
        {
            var obj = await _unitOfWork.UserSettingService.GetAllEmailPeriodType();

            if (obj == null)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "No Data Found" });
            }
            else
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
        }

        [HttpGet(nameof(GetUserEmailSettingByUserId))]
        public async Task<IActionResult> GetUserEmailSettingByUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            Guid userId= Guid.Empty;
            if(identity != null)
            {
                var userClaims = identity.Claims;
                userId = new Guid(userClaims.FirstOrDefault(x => x.Type == "Id").Value);
            }

            var obj = await _unitOfWork.UserSettingService.GetEmailUserSettingByUserId(userId);

            if (obj == null)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "No Data Found" });
            }
            else
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
        }

        [HttpGet(nameof(GetAllEmailType))]
        public async Task<IActionResult> GetAllEmailType()
        {
            var obj = await _unitOfWork.UserSettingService.GetAllEmailType();

            if (obj == null)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "No Data Found" });
            }
            else
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
        }

        [HttpGet(nameof(GetAllNotificationsType))]
        public async Task<IActionResult> GetAllNotificationsType()
        {
            var obj = await _unitOfWork.UserSettingService.GetAllNotificationsType();

            if (obj == null)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "No Data Found" });
            }
            else
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
        }

        [HttpGet("GetUserAccountSettingEmailChecks")]
        public IActionResult GetUserAccountSettingEmailChecks()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                Guid UserId = new Guid(userClaims.FirstOrDefault(x => x.Type == "Id").Value);
                var obj = _unitOfWork.UserSettingService.GetUserAccountSettingEmailChecks(UserId);
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj.Result });
            }
            return Ok(new APIResponse { StatusCode = StatusCodes.Status406NotAcceptable.ToString(), Status = "Failure", Response = "Unauthorized Access" });
        }


        [HttpPost(nameof(SaveAccountSettingEmailNotificationSelected))]
        public async Task<IActionResult> SaveAccountSettingEmailNotificationSelected([FromBody] JsonElement settings)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(settings);
                List<UserAccountSettingDTO> accountsetting = JsonConvert.DeserializeObject<List<UserAccountSettingDTO>>(json);

                //List<DigestiveEmailType> emailTypes = JsonConvert.DeserializeObject<List<DigestiveEmailType>>(json);

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null && accountsetting != null)
                {
                    var userClaims = identity.Claims;
                    Guid UserId = new Guid(userClaims.FirstOrDefault(x => x.Type == "Id").Value);
                    bool result = false;
                    var setting = new UserAccountSetting();
                    foreach (var item in accountsetting)
                    {
                        item.UserId = UserId;
                        setting = _mapper.Map<UserAccountSetting>(settings);
                        var existSetting = await _unitOfWork.AccountSettingEmailNotificationService.GetT(x => x.UserId == item.UserId && x.AccountSettingId == item.AccountSettingId);
                        if (existSetting == null)
                        {
                            _unitOfWork.AccountSettingEmailNotificationService.Insert(setting);
                        }
                        else
                        {
                            existSetting.AccountSettingId = item.AccountSettingId;
                            existSetting.IsActive = item.IsActive;
                            _unitOfWork.AccountSettingEmailNotificationService.Update(existSetting);
                        }
                        _unitOfWork.Save();
                    }
                    if (result)
                    {
                        return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = "User setting email or notification added successfully." });
                    }
                    else
                    {
                        return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Failure", Response = "User setting email or notification updated successfully." });
                    }
                }
                else
                {
                    return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "The Data given by you is totally empty.." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), Status = "Failure", Response = "Data Not Found." });
            }
        }



        [HttpGet("GetUserAccountSettingNotificationChecks")]
        public IActionResult GetUserAccountSettingNotificationChecks()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                Guid UserId = new Guid(userClaims.FirstOrDefault(x => x.Type == "Id").Value);
                var obj = _unitOfWork.UserSettingService.GetUserAccountSettingNotificationChecks(UserId);
                return Ok(new APIResponse { StatusCode = StatusCodes.Status200OK.ToString(), Status = "Success", Response = obj });
            }
            return Ok(new APIResponse { StatusCode = StatusCodes.Status406NotAcceptable.ToString(), Status = "Failure", Response = false });
        }
    }
}
