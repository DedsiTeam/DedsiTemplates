using ApiGateway.AgClusterConfigs;
using ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ApiGateway.Repositories;

public interface IAgClusterConfigRepository : IRepository<AgClusterConfig>
{
    Task<bool> DeleteByClusterIdAsync(string clusterId, CancellationToken cancellationToken);
}

public class AgClusterConfigRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider)
    : EfCoreRepository<ApiGatewayDbContext, AgClusterConfig>(dbContextProvider), IAgClusterConfigRepository
{
    /// <inheritdoc/>
    public async Task<bool> DeleteByClusterIdAsync(string clusterId, CancellationToken cancellationToken)
    {
        var entity = await (await GetDbContextAsync())
            .AgClusterConfigs
            .Include(a => a.Destinations)
            .FirstOrDefaultAsync(a => a.ClusterId == clusterId, cancellationToken);

        if (entity != null)
        {
            await DeleteAsync(entity, false, cancellationToken);
            return true;
        }

        return false;
    }
}