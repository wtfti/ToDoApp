﻿namespace ToDo.Api.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Models.Account;
    using Server.Common;

    public class AccountController : BaseController
    {
        public AccountController()
        {
            
        }

        public AccountController(ApplicationUserManager applicationUserManager)
            : base(applicationUserManager)
        {
            
        }

        private IAuthenticationManager Authentication => this.Request.GetOwinContext().Authentication;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterModel registerModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new User()
            {
                UserName = registerModel.Email,
                Email = registerModel.Email
            };

            IdentityResult result = await this.UserManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return this.BadRequest(MessageConstants.UserIsNotAddInDbMessage);
            }

            return this.Ok(MessageConstants.CreateUserMessage);
        }
        
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok(MessageConstants.LogoutMessage);
        }
    }
}