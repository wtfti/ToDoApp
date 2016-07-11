namespace ToDo.Api
{
    using System;
    using System.Web.Http;
    using System.Web.Http.ModelBinding.Binders;
    using ModelBinders;
    using Models.Note;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var provider = new SimpleModelBinderProvider(
                        typeof(NoteRequestModel), new NoteRequestModelBinder());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
