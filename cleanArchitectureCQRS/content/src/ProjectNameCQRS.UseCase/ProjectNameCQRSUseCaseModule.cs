using Volo.Abp.Modularity;

namespace ProjectNameCQRS;

[DependsOn(
    // ProjectNameCQRS
    typeof(ProjectNameCQRSDomainModule),
    typeof(ProjectNameCQRSSharedModule),
    typeof(ProjectNameCQRSInfrastructureModule)
)]
public class ProjectNameCQRSUseCaseModule : AbpModule;