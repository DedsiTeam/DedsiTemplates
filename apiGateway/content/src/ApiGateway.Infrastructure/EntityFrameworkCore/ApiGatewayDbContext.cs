using ApiGateway.AgClusterConfigs;
using ApiGateway.AgRouteConfigs;
using Dedsi.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace ApiGateway.EntityFrameworkCore;

[ConnectionStringName(ApiGatewayDomainConsts.ConnectionStringName)]
public class ApiGatewayDbContext(DbContextOptions<ApiGatewayDbContext> options) 
    : DedsiEfCoreDbContext<ApiGatewayDbContext>(options)
{
    public DbSet<AgClusterConfig> AgClusterConfigs { get; set; }

    public DbSet<AgRouteConfig> AgRouteConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureProjectName();
    }

}