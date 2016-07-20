namespace ToDo.Api.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data.Models;
    using Infrastructure.Validation;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Models.Account;
    using Server.Common.Constants;
    using Services.Data.Contracts;

    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService service)
        {
            this.accountService = service;
        }

        public AccountController(ApplicationUserManager applicationUserManager)
            : base(applicationUserManager)
        {
        }

        private IAuthenticationManager Authentication => this.Request.GetOwinContext().Authentication;

        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterModel registerModel)
        {
            var user = new User()
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                ProfileDetails = new ProfileDetails()
                {
                    FullName = registerModel.FullName
                }
            };

            IdentityResult result = await this.UserManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return this.BadRequest(MessageConstants.EmailAlreadyTaken);
            }

            return this.Ok(MessageConstants.CreateUser);
        }
        
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok(MessageConstants.Logout);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult Details()
        {
            var userId = this.User.Identity.GetUserId();

            var userDetails = this.accountService
                .ProfileDetails(userId);

            var result = new UserProfileResponseModel()
            {
                FullName = userDetails.FullName,
                Age = userDetails.Age,
                Gender = userDetails.Gender.ToString(),
                Email = this.User.Identity.Name
            };

            return this.Ok(result);
        }

        [HttpPut]
        [ValidateModel]
        [Authorize]
        public IHttpActionResult Edit(UserProfileRequestModel user)
        {
            var userId = this.User.Identity.GetUserId();

            this.accountService.Edit(userId, user.FullName, user.Age, user.Gender);

            return this.Ok();
        }
    }
}