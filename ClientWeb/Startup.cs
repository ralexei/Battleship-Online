using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(ClientWeb.Startup))]

namespace ClientWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            string root = AppDomain.CurrentDomain.BaseDirectory;
            var ph = new PhysicalFileSystem(Path.Combine(root, "wwwroot"));
            var options = new FileServerOptions
            {
                RequestPath = PathString.Empty,
                EnableDefaultFiles = true,
                FileSystem = ph
            };
            options.StaticFileOptions.FileSystem = ph;
            options.StaticFileOptions.ServeUnknownFileTypes = false;
            app.UseFileServer(options);
            app.MapSignalR();
        }
    }
}
