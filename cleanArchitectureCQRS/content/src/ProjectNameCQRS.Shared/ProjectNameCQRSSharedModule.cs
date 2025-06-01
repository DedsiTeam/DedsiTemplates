using Dedsi.Ddd.Application.Contracts;
using Dedsi.Ddd.CQRS;
using Dedsi.Ddd.Domain.Shared;
using Volo.Abp.Modularity;

namespace ProjectNameCQRS;

[DependsOn(
    typeof(DedsiDddApplicationContractsModule),
    typeof(DedsiDddDomainSharedModule),
    typeof(DedsiDddCqrsModule)
)]
public class ProjectNameCQRSSharedModule: AbpModule;