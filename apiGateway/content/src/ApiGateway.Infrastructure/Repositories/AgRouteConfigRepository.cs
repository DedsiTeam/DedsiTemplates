using ApiGateway.AgRouteConfigs;
using ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ApiGateway.Repositories;

public interface IAgRouteConfigRepository : IRepository<AgRouteConfig>
{
    Task<bool> DeleteByRouteIdAsync(string routeId, CancellationToken cancellationToken);
}

public class AgRouteConfigRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider)
    : EfCoreRepository<ApiGatewayDbContext, AgRouteConfig>(dbContextProvider), IAgRouteConfigRepository
{
    /// <inheritdoc/>
    public async Task<bool> DeleteByRouteIdAsync(string routeId, CancellationToken cancellationToken)
    {
        var entity = await (await GetDbContextAsync())
            .AgRouteConfigs
            .Include(a => a.Match)
            .FirstOrDefaultAsync(a => a.RouteId == routeId, cancellationToken);

        if (entity != null)
        {
            await DeleteAsync(entity, false, cancellationToken);
            return true;
        }

        return false;
    }
}