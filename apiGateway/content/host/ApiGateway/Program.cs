using ApiGateway.Apis;
using Serilog;
using Serilog.Events;
using Yarp.ReverseProxy.Configuration;

namespace Dedsi.ApiGateway;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Async(c => c.File(path: "Logs/logs.txt", rollingInterval: RollingInterval.Hour, retainedFileCountLimit: null))
            .WriteTo.Async(c => c.Console())
            .CreateBootstrapLogger();

        try
        {
            Log.Information("程序已启动！");
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration);
            builder.Services.AddSingleton<IProxyConfigProvider, ApiGatewayProxyConfigProvider>();

            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.File(path: "Logs/logs.txt", rollingInterval: RollingInterval.Hour, retainedFileCountLimit: null))
                        .WriteTo.Async(c => c.Console());
                });

            await builder.AddApplicationAsync<ApiGatewayModule>();

            var app = builder.Build();

            app.MapApiGatewayHttpApis();

            await app.InitializeApplicationAsync();
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "主机意外终止!");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}