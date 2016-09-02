namespace ToDo.Api.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    [EnableCors(origins: "http://localhost:8090", headers: "*", methods: "*")]
    public abstract class BaseController : ApiController
    {
        private ApplicationUserManager userManager;

        protected BaseController()
        {
        }

        protected BaseController(ApplicationUserManager applicationUserManager)
        {
            this.UserManager = applicationUserManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        [NonAction]
        protected string CurrentUserId()
        {
            string result = this.User.Identity.GetUserId();

            return result;
        }

        [NonAction]
        protected string CurrentUsername()
        {
            string result = this.User.Identity.GetUserName();

            return result;
        }
    }
}