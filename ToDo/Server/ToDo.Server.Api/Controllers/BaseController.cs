namespace ToDo.Api.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.AspNet.Identity.Owin;

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

    }
}