using Dedsi.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ProjectName;

[DependsOn(
    typeof(ProjectNameCoreModule),
    typeof(DedsiAspNetCoreModule)
)]
public class ProjectNameHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProjectNameHttpApiModule).Assembly);
        });
    }
}