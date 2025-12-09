using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Domain.Entities.Workshops;
using MyFactory.Infrastructure.Common;

namespace MyFactory.Infrastructure.Persistence.Seeds;

public class InitialDataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Settings _settings;
    private readonly ILogger<InitialDataSeeder> _logger;

    private static readonly RoleSeed[] RoleSeeds =
    {
        new("Administrator", "Full access to all factory modules"),
        new("Planner", "Can plan production and manage resources"),
        new("Accountant", "Manages payroll and finance records")
    };

    private static readonly SupplierSeed[] SupplierSeeds =
    {
        new("Global Metals Co.", "sales@globalmetals.example"),
        new("Polymer Source", "orders@polymer-source.example")
    };

    private static readonly WarehouseSeed[] WarehouseSeeds =
    {
        new("Main Raw Warehouse", "Raw", "Building A"),
        new("Finished Goods Hub", "Finished", "Building C"),
        new("Auxiliary Storage", "Auxiliary", "Annex")
    };

    private static readonly WorkshopSeed[] WorkshopSeeds =
    {
        new("Stamping Line", "Metalworking"),
        new("Injection Molding", "Plastics"),
        new("Assembly Cell", "Assembly")
    };

    private static readonly string[] MaterialTypeSeeds = { "Metals", "Polymers", "Packaging" };

    private static readonly MaterialSeed[] MaterialSeeds =
    {
        new("Steel Sheet A36", "Metals", "kg", 28.5m, "PRICE-STEEL-A36"),
        new("ABS Granules", "Polymers", "kg", 16.2m, "PRICE-ABS-GRAN"),
        new("Cardboard Box", "Packaging", "pcs", 1.15m, "PRICE-BOX-L"),
    };

    public InitialDataSeeder(ApplicationDbContext dbContext, IOptions<Settings> options, ILogger<InitialDataSeeder> logger)
    {
        _dbContext = dbContext;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!_settings.SeedDemoData)
        {
            _logger.LogInformation("Seed is disabled in settings.");
            return;
        }

        await _dbContext.Database.MigrateAsync(cancellationToken);

        await SeedRolesAsync(cancellationToken);
        await SeedSuppliersAsync(cancellationToken);
        await SeedMaterialsAsync(cancellationToken);
        await SeedWarehousesAsync(cancellationToken);
        await SeedWorkshopsAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Roles.AnyAsync(cancellationToken))
        {
            return;
        }

        var roles = RoleSeeds.Select(seed => new Role(seed.Name, seed.Description)).ToArray();
        await _dbContext.Roles.AddRangeAsync(roles, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded {Count} roles.", roles.Length);
    }

    private async Task SeedSuppliersAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Suppliers.AnyAsync(cancellationToken))
        {
            return;
        }

        var suppliers = SupplierSeeds.Select(seed => new Supplier(seed.Name, seed.Contact)).ToArray();
        await _dbContext.Suppliers.AddRangeAsync(suppliers, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded {Count} suppliers.", suppliers.Length);
    }

    private async Task SeedMaterialsAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.MaterialTypes.AnyAsync(cancellationToken) || await _dbContext.Materials.AnyAsync(cancellationToken))
        {
            return;
        }

        var materialTypes = MaterialTypeSeeds.Select(name => new MaterialType(name)).ToList();
        await _dbContext.MaterialTypes.AddRangeAsync(materialTypes, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var supplier = await _dbContext.Suppliers
            .OrderBy(s => s.Name)
            .FirstAsync(cancellationToken);

        var typeLookup = materialTypes.ToDictionary(type => type.Name, type => type);
        var materials = new List<Material>();
        var priceEntries = new List<MaterialPriceHistory>();
        var today = DateTime.UtcNow.Date;

        foreach (var seed in MaterialSeeds)
        {
            if (!typeLookup.TryGetValue(seed.MaterialTypeName, out var type))
            {
                _logger.LogWarning("Skipping material seed {Name} because type {Type} is missing.", seed.Name, seed.MaterialTypeName);
                continue;
            }

            var material = new Material(seed.Name, type.Id, seed.Unit);
            materials.Add(material);
            priceEntries.Add(material.AddPrice(supplier.Id, seed.PricePerUnit, today, seed.DocumentReference));
        }

        await _dbContext.Materials.AddRangeAsync(materials, cancellationToken);
        await _dbContext.MaterialPriceHistoryEntries.AddRangeAsync(priceEntries, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {MaterialCount} materials with price history entries.", materials.Count);
    }

    private async Task SeedWarehousesAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Warehouses.AnyAsync(cancellationToken))
        {
            return;
        }

        var warehouses = WarehouseSeeds.Select(seed => new Warehouse(seed.Name, seed.Type, seed.Location)).ToArray();
        await _dbContext.Warehouses.AddRangeAsync(warehouses, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} warehouses.", warehouses.Length);
    }

    private async Task SeedWorkshopsAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Workshops.AnyAsync(cancellationToken))
        {
            return;
        }

        var workshops = WorkshopSeeds.Select(seed => new Workshop(seed.Name, seed.Type)).ToArray();
        await _dbContext.Workshops.AddRangeAsync(workshops, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} workshops.", workshops.Length);
    }

    private sealed record RoleSeed(string Name, string Description);
    private sealed record SupplierSeed(string Name, string Contact);
    private sealed record WarehouseSeed(string Name, string Type, string Location);
    private sealed record WorkshopSeed(string Name, string Type);
    private sealed record MaterialSeed(string Name, string MaterialTypeName, string Unit, decimal PricePerUnit, string DocumentReference);
}
