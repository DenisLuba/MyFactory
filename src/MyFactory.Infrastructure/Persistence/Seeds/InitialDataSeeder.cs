using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Infrastructure.Common;

namespace MyFactory.Infrastructure.Persistence.Seeds;

public class InitialDataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Settings _settings;
    private readonly ILogger<InitialDataSeeder> _logger;

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

        await SeedMaterialsAsync(cancellationToken);
    }

    private async Task SeedMaterialsAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.MaterialTypes.AnyAsync(cancellationToken))
        {
            return;
        }

        var metals = new MaterialType("Metals");
        var plastics = new MaterialType("Plastics");
        await _dbContext.MaterialTypes.AddRangeAsync(new[] { metals, plastics }, cancellationToken);

        var supplier = new Supplier("Default Supplier", "supplier@example.com");
        await _dbContext.Suppliers.AddAsync(supplier, cancellationToken);

        var steelSheet = new Material("Steel Sheet", metals.Id, "kg");
        var absGranules = new Material("ABS Granules", plastics.Id, "kg");

        await _dbContext.Materials.AddRangeAsync(new[] { steelSheet, absGranules }, cancellationToken);

        var now = DateTime.UtcNow.Date;
        var steelEntry = steelSheet.AddPrice(supplier.Id, 25.5m, now, "SEED-STEEL");
        var absEntry = absGranules.AddPrice(supplier.Id, 14.9m, now, "SEED-ABS");

        await _dbContext.MaterialPriceHistoryEntries.AddRangeAsync(new[] { steelEntry, absEntry }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded default materials and suppliers.");
    }
}
