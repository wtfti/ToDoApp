namespace ToDo.Api.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Web.Http;
    using Models.Account;

    public class AccountController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Register(RegisterModel registerModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            return this.Ok("created brao");
        }

        public IHttpActionResult Login()
        {
            return this.Ok();
        }
    }
}