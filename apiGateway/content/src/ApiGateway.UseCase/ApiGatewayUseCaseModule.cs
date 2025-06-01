using Volo.Abp.Modularity;

namespace ApiGateway;

[DependsOn(
    // ApiGateway
    typeof(ApiGatewayDomainModule),
    typeof(ApiGatewaySharedModule),
    typeof(ApiGatewayInfrastructureModule)
)]
public class ApiGatewayUseCaseModule : AbpModule;