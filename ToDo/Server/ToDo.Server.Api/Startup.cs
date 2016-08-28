using Microsoft.Owin;
using Owin;
using ToDo.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace ToDo.Api
{
    using Microsoft.Owin.Cors;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}