using Microsoft.EntityFrameworkCore;
using Multitenant.Core.Contracts;
using Multitenant.Core.Entities;
using Multitenant.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Infrastructure.Persistence;
// Create Infrastructure/Persistence/ApplicationDbContext.cs
public class ApplicationDbContext : DbContext
{
    public string TenantId { get; set; }
    private readonly ITenantService _tenantService;

    public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
        // ? null saftey if null not complete
        TenantId = _tenantService.GetTenant()?.TID;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // HasQueryFilter 
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasQueryFilter(a => a.TenantId == TenantId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = _tenantService.GetConnectionString();
        if (!string.IsNullOrEmpty(tenantConnectionString))
        {
            var DBProvider = _tenantService.GetDatabaseProvider();
            if (DBProvider.ToLower() == "mssql")
            {
                optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
            }
            else if (DBProvider.ToLower() == "npgsql")
            {
                optionsBuilder.UseNpgsql(_tenantService.GetConnectionString());
            }
        }
    }

    // each time not make tenant = header in request this step make dynamic
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // select all entities that contain TenantId
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = TenantId;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}