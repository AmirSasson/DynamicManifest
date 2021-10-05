using DynamicRoutes.Auth;
using DynamicRoutes.Controllers;
using DynamicRoutes.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;

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

            services.AddAuthentication().AddScheme<WWWAuthenticationOptions, WWWAuthenticationHandler>(
              "WWW",
              o =>
              {

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

            //app.Use(async (context, next) =>
            //{               
            //    var authenticationService = context.RequestServices.GetService<IAuthenticationService>();
            //    var authenticationResult = await authenticationService.AuthenticateAsync(context, "WWW");

            //    var authorized = authenticationResult.Succeeded;

            //    var cleanedPath = context.Request.Path.ToClean();

            //    if (TryGetEndpoint(context, out var endpoint))
            //    {
            //        HttpClient c = new HttpClient();
            //        UriBuilder b = new UriBuilder("http", "localhost", endpoint.Port, cleanedPath);
            //        b.Query = context.Request.QueryString.ToString();
            //        using var req = new HttpRequestMessage(new HttpMethod(context.Request.Method), b.Uri) {/* body.. headers .. more */};
            //        var resp = await c.SendAsync(req);
            //        context.Response.StatusCode = (int)resp.StatusCode;
            //        await context.Response.WriteAsync(await resp.Content.ReadAsStringAsync());
            //        return;
            //    }
            //    else
            //    {
            //        // Call the next delegate/middleware in the pipeline
            //        await next();
            //    }
            //});


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private static bool TryGetEndpoint(HttpContext context, out ApiEndpoint endpoint)
        //{
        //    endpoint = null;
        //    var key = Controllers.EndpointsManifestController.ManifestDB.Keys.FirstOrDefault(pattern =>
        //    {
        //        var template = TemplateParser.Parse(pattern);
        //        var matcher = new TemplateMatcher(template, new RouteValueDictionary());

        //        var isMatch = matcher.TryMatch(context.Request.Path, new RouteValueDictionary());
        //        return isMatch;
        //    });
        //    if (key != null)
        //    {
        //        endpoint = Controllers.EndpointsManifestController.ManifestDB[key];
        //    }
        //    return endpoint != null;
        //}
    }
}
