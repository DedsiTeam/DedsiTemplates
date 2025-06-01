using Dedsi.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ApiGateway;

[DependsOn(
    typeof(ApiGatewayUseCaseModule),
    typeof(DedsiAspNetCoreModule)
)]
public class ApiGatewayHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ApiGatewayHttpApiModule).Assembly);
        });
    }
}