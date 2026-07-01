using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExpenseFlow.Notification.Api.Extensions;

public static class LoggingExtensions
{
    public static void UseSerilogLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        });
    }
}
