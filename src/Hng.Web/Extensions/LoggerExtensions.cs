using Microsoft.AspNetCore.Diagnostics;
using NLog;
using System.Net;

namespace Hng.Web.Extensions
{
    public static class LoggerExtensions
    {
        public static void UseGlobalErrorHandler(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var logger = LogManager.GetCurrentClassLogger();
                        logger.Error(contextFeature.Error, "Unhandled exception");

                        if (env.IsDevelopment())
                        {
                            await context.Response.WriteAsync(contextFeature.Error.ToString());
                        }
                        else
                        {
                            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                        }
                    }
                });
            });
        }
    }
}
