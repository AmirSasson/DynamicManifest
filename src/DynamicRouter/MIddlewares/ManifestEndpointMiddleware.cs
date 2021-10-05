using DynamicRoutes.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
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

        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
        {
            var cleanedPath = context.Request.Path.ToClean();

            if (TryGetEndpoint(context, out var endpoint))
            {
                var authenticationResult = await authenticationService.AuthenticateAsync(context, "WWW");                               
                var authorized = authenticationResult.Succeeded;

                if (authorized)
                {
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

        private static bool TryGetEndpoint(HttpContext context, out ApiEndpoint endpoint)
        {            
            var key = Controllers.EndpointsManifestController.ManifestDB.Keys.FirstOrDefault(pattern =>
            {
                var template = TemplateParser.Parse(pattern);
                var matcher = new TemplateMatcher(template, new RouteValueDictionary());

                var isMatch = matcher.TryMatch(context.Request.Path, new RouteValueDictionary());
                return isMatch;
            });

            endpoint = key != null ? Controllers.EndpointsManifestController.ManifestDB[key] : null;
            return endpoint != null;
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
