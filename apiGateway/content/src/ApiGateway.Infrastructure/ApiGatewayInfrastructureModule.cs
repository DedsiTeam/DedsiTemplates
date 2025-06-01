using Dedsi.CleanArchitecture.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ApiGateway.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ApiGateway;

[DependsOn(
    typeof(DedsiCleanArchitectureInfrastructureModule)
)]
public class ApiGatewayInfrastructureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // EntityFrameworkCore
        context.Services.AddAbpDbContext<ApiGatewayDbContext>(options =>
        {
            options.AddDefaultRepositories(true);
        });
    }
}