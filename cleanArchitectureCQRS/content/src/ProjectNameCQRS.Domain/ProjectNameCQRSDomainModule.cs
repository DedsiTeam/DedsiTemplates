using Dedsi.CleanArchitecture.Domain;
using Volo.Abp.Modularity;

namespace ProjectNameCQRS;

[DependsOn(
    typeof(DedsiCleanArchitectureDomainModule)    
)]
public class ProjectNameCQRSDomainModule : AbpModule;