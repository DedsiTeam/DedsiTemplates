using Dedsi.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ProjectNameCQRS;

[DependsOn(
    typeof(ProjectNameCQRSUseCaseModule),
    typeof(DedsiAspNetCoreModule)
)]
public class ProjectNameCQRSHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProjectNameCQRSHttpApiModule).Assembly);
        });
    }
}