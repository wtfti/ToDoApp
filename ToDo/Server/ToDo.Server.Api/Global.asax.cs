namespace ToDo.Api
{
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using Server.Common.Constants;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            DatabaseConfig.Initialize();
            AutoMapperConfig.RegisterMappings(Assembly.Load(AssemblyConstants.WebApi));
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
