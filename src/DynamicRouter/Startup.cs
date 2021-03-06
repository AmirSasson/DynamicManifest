using DynamicRoutes.Auth;
using DynamicRoutes.DataAccess;
using DynamicRoutes.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DynamicRoutes
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

            services.AddSingleton<IEndpointsManifestRespository>((sp) =>
            {
                string COSMOS_EMULATOR_CON_STR = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                var a = new CosmosDbEndpointsManifestRespository(sp.GetService<ILogger<CosmosDbEndpointsManifestRespository>>(), COSMOS_EMULATOR_CON_STR, "MY_TEST", "EndpointsManifest");
                a.GetAll().Wait();
                return a;
            });
            //services.AddSingleton<IEndpointsManifestRespository, InMemoryEndpointsManifestRespository>();

            services.AddAuthentication().AddScheme<WWWAuthenticationOptions, WWWAuthenticationHandler>(
              "WWW",
              o =>
              {
                  o.SkipClientCertificateChecks = false;
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseManifestEndpointMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
