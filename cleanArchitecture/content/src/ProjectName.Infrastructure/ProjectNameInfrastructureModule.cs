using Volo.Abp.Modularity;

namespace ProjectName;

[DependsOn(
    typeof(ProjectNameCoreModule)
)]
public class ProjectNameInfrastructureModule : AbpModule;