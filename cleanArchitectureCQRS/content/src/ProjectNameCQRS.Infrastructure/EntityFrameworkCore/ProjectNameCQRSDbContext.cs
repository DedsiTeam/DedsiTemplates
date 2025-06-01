using Dedsi.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectNameCQRS.Users;
using Volo.Abp.Data;

namespace ProjectNameCQRS.EntityFrameworkCore;

[ConnectionStringName(ProjectNameCQRSDomainConsts.ConnectionStringName)]
public class ProjectNameCQRSDbContext(DbContextOptions<ProjectNameCQRSDbContext> options) 
    : DedsiEfCoreDbContext<ProjectNameCQRSDbContext>(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureProjectName();
    }

}