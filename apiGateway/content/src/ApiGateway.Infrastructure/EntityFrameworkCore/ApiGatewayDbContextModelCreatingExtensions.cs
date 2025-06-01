using ApiGateway.AgClusterConfigs;
using ApiGateway.AgRouteConfigs;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace ApiGateway.EntityFrameworkCore;

public static class ApiGatewayDbContextModelCreatingExtensions
{
    public static void ConfigureProjectName(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<AgClusterConfig>(b =>
        {
            b.ToTable("AgClusterConfigs", ApiGatewayDomainConsts.DbSchemaName);
            b.HasKey(e => e.ClusterId);

            b
            .HasMany(e => e.Destinations)
            .WithOne()
            .HasPrincipalKey(e => e.ClusterId)
            .HasForeignKey(a => a.ClusterId)
            .IsRequired();
        });

        builder.Entity<AgClusterDestinationConfig>(b =>
        {
            b.ToTable("AgClusterDestinationConfigs", ApiGatewayDomainConsts.DbSchemaName);
            b.HasKey(e => e.ClusterId);
        });

        builder.Entity<AgRouteConfig>(b =>
        {
            b.ToTable("AgRouteConfigs", ApiGatewayDomainConsts.DbSchemaName);
            b.HasKey(e => e.ClusterId);

            b
            .HasOne(e => e.Match)
            .WithOne()
            .HasPrincipalKey<AgRouteConfig>(e => e.RouteId)
            .HasForeignKey<AgRouteMatch>(a => a.RouteId)
            .IsRequired();
        });

        builder.Entity<AgRouteMatch>(b =>
        {
            b.ToTable("AgRouteMatchs", ApiGatewayDomainConsts.DbSchemaName);
            b.HasKey(e => e.RouteId);
        });
    }
}