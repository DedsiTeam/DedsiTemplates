using Dedsi.CleanArchitecture.Domain;
using Volo.Abp.Modularity;

namespace ApiGateway;

[DependsOn(
    typeof(DedsiCleanArchitectureDomainModule)    
)]
public class ApiGatewayDomainModule : AbpModule;