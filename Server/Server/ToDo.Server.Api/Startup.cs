using Microsoft.Owin;
using Owin;
using ToDo.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace ToDo.Api
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin.Cors;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}