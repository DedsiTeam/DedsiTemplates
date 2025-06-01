using ApiGateway.AgRouteConfigs;
using ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace ApiGateway.Queries;

public interface IAgRouteConfigQuery: ITransientDependency
{
    Task<AgRouteConfig[]> GetAllAsync(CancellationToken cancellationToken = default);
}


public class AgRouteConfigQuery(ApiGatewayDbContext apiGatewayDbContext) : IAgRouteConfigQuery
{
    public async Task<AgRouteConfig[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await apiGatewayDbContext.AgRouteConfigs
            .Include(a => a.Match)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }
}
