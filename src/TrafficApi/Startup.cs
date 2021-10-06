using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IManifestProvider, RPManifestProvider>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(async () => { await this.OnApplicationStarted(app); });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public async Task OnApplicationStarted(IApplicationBuilder app)
        {
            string address = app.ServerFeatures.Get<IServerAddressesFeature>()
                  .Addresses
                  .First();

            var serverUri = new Uri(address);

            var routes = app.ApplicationServices.GetService<IEnumerable<EndpointDataSource>>();
            await app.ApplicationServices.GetService<IManifestProvider>().Register(routes, new ServerAddress { Port = serverUri.Port });
        }
    }
}
