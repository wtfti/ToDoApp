﻿using Microsoft.Owin;
using Owin;
using ToDo.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace ToDo.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}