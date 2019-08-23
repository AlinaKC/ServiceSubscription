using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ServiceSubscription.Core;
using System.Text;
using SeviceSubscription.Core.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace SeviceSubscription.Web
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IUserProvider _userProvider;
        private IMailSender _mailSender;
        private readonly AppSettings _appSettings;


        public UsersController(IOptions<AppSettings> appSettings, IUserService userService, IUserProvider userProvider, IMailSender mailSender)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _userProvider = userProvider;
            _mailSender = mailSender;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserInfo userParam)
        {
            var user = _userService.Authenticate(userParam.UserName, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                userId = user.UserId,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString,
                role = user.Role
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserInfo user)
        {
            try
            {
                _userService.CreateUser(user, user.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userProvider.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByUserId(int id)
        {
            var user = _userProvider.GetUserDetailsById(id);

            if (user == null)
            {
                return NotFound();
            }

            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("isEmailExist/{userEmail}")]
        public IActionResult IsEmailAddressExists(string userEmail)
        {
            var isExists = _userProvider.IsEmailAddressExists(userEmail);
            return Ok(isExists);
        }

        [HttpGet("isSubscribe/{id}")]
        public IActionResult HasUserSubscribed(int id)
        {
            var subscribeInfo = _userProvider.HasUserSubscribed(id);
            return Ok(subscribeInfo);
        }

        [HttpPost("subscribe")]
        public IActionResult AddUserSubscription([FromBody]SubscriptionApiInfo apiInfo)
        {
            try
            {
                var subscriptionId = _userProvider.AddUserSubscription(apiInfo.id, apiInfo.subscribeType);
                var subscribeInfo = _userProvider.HasUserSubscribed(apiInfo.id);
                SendEmail(apiInfo.id);
                return Ok(subscribeInfo);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("unsubscribe")]
        public IActionResult UnSubscribeService([FromBody]SubscriptionApiInfo apiInfo)
        {
            _userProvider.UnSubscribeService(apiInfo.id);
            return Ok();
        }

        [HttpGet("isUnsubcribe/{id}")]
        public IActionResult HasUserUnSubscribed(int id)
        {
            var subscribeInfo = _userProvider.HasUserUnSubscribed(id);
            return Ok(subscribeInfo);
        }

        [HttpPost("report")]
        public IActionResult Report([FromBody]ReportApiInfo apiInfo)
        {
            var reportList = _userProvider.GetReport(apiInfo);
            return Ok(reportList);
        }

        [HttpPost("resubscribe")]
        public IActionResult ReSubscribeService([FromBody]SubscriptionApiInfo apiInfo)
        {
            _userProvider.ReSubscribeService(apiInfo.id);
            return Ok();
        }


        #region private methods
        private string SendEmail(int id)
        {
            string userEmail = _userProvider.GetUserEmailById(id);
            string emailBody = GetUserReport(id);
            var emailStatus = _mailSender.SendEmail(userEmail, emailBody);
            return emailStatus;
        }

        private string GetUserReport(int userId)
        {
            var lstMyServices = _userProvider.GetServiceDetailsByUserId(userId);
            StringBuilder str = new StringBuilder();
            str.Append(@"<h4>Invoice of your Service Subscription</h4>
                           <table>
                              <tr>
                                <th>Service Type</th>
                                <th>Cost Paid</th>
                                <th>Transaction On</th>
                              </tr>");
            foreach (var myServicesInfo in lstMyServices)
            {
                str.AppendFormat(@" <tr>
                                <td>{0}</td>
                                <td>${1}</td>
                                <td>{2}</td>
                              </tr>", myServicesInfo.ServiceTypeName, myServicesInfo.Amount, myServicesInfo.AddedOn.ToString("d/M/yyyy"));
            }
            str.Append("<table>");
            return str.ToString();
        } 
        #endregion
    }
}
