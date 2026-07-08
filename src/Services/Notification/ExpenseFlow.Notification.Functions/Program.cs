using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExpenseFlow.Notification.Functions.Extensions;
using Serilog;
using ExpenseFlow.Notification.Application.Extensions;
using ExpenseFlow.Notification.Infrastructure.Extensions;
using ExpenseFlow.Notification.Persistence.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Structured logging setup
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        services.ConfigureFunctionsApplicationInsights();
        services.AddTelemetryConfig(context.Configuration, "ExpenseFlow.Notification.Functions");

        // Microservice registrations
        services.AddApplicationServices();
        services.AddInfrastructureServices(context.Configuration, registerBackgroundConsumer: false);
        services.AddPersistenceServices(context.Configuration);
    })
    .Build();

host.Run();
