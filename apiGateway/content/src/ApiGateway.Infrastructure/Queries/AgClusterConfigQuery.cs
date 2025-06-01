using ApiGateway.AgClusterConfigs;
using ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace ApiGateway.Queries;

public interface IAgClusterConfigQuery : ITransientDependency
{
    Task<string[]> GetAllClusterIdAsync(CancellationToken cancellationToken);


    Task<AgClusterConfig[]> GetAllAsync(CancellationToken cancellationToken = default);
}

public class AgClusterConfigQuery(ApiGatewayDbContext apiGatewayDbContext) : IAgClusterConfigQuery
{
    /// <inheritdoc/>
    public async Task<AgClusterConfig[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await apiGatewayDbContext.AgClusterConfigs
            .Include(a => a.Destinations)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<string[]> GetAllClusterIdAsync(CancellationToken cancellationToken)
    {
        return await apiGatewayDbContext.AgClusterConfigs.Select(a => a.ClusterId).ToArrayAsync();
    }
}
