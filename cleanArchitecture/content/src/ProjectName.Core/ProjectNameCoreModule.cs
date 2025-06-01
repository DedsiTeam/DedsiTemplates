using Dedsi.CleanArchitecture.Domain;
using Volo.Abp.Modularity;

namespace ProjectName;

[DependsOn(
    typeof(DedsiCleanArchitectureDomainModule)    
)]
public class ProjectNameCoreModule : AbpModule;