using Common;
using DynamicRoutes.Controllers;
using DynamicRoutes.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicRoutes.Middlewares
{
    public class ManifestEndpointMiddleware
    {
        private readonly RequestDelegate _next;

        public ManifestEndpointMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService, IEndpointsManifestRespository endpointRepo, ILogger<ManifestEndpointMiddleware> logger)
        {
            var cleanedPath = context.Request.Path.ToClean();
            ApiEndpoint endpoint;
            if ((endpoint = await getEndpoint(context, endpointRepo)) != null)
            {
                var authenticationResult = await authenticationService.AuthenticateAsync(context, "WWW");
                var authorized = authenticationResult.Succeeded;

                if (authorized)
                {
                    logger.LogInformation($"Delegating to endpoint : {endpoint.ToJson()}");
                    HttpClient c = new HttpClient();
                    UriBuilder b = new UriBuilder("http", "localhost", endpoint.Port, cleanedPath);
                    b.Query = context.Request.QueryString.ToString();
                    using var req = new HttpRequestMessage(new HttpMethod(context.Request.Method), b.Uri) {/* body.. headers .. more */};
                    var resp = await c.SendAsync(req);
                    context.Response.StatusCode = (int)resp.StatusCode;
                    await context.Response.WriteAsync(await resp.Content.ReadAsStringAsync());
                    return;
                }
                else
                {
                    throw authenticationResult.Failure;
                }
            }
            else
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
        }

        private static async Task<ApiEndpoint> getEndpoint(HttpContext context, IEndpointsManifestRespository endpointRepo)
        {
            var endpoint = (await endpointRepo.GetAll()).FirstOrDefault(ep =>
            {
                var template = TemplateParser.Parse(ep.Path);
                var matcher = new TemplateMatcher(template, new RouteValueDictionary());

                var isMatch = matcher.TryMatch(context.Request.Path, new RouteValueDictionary());
                return isMatch;
            });
            
            return endpoint;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ManifestEndpointMiddlewareExtensions
    {
        public static IApplicationBuilder UseManifestEndpointMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ManifestEndpointMiddleware>();
        }
    }
}
