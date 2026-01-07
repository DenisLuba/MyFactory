using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Organization;
using MyFactory.Domain.Entities.Security;
using MyFactory.Infrastructure.Common;

namespace MyFactory.Infrastructure.Persistence;

public class InitialDataSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly Settings _settings;

    public InitialDataSeeder(ApplicationDbContext db, IOptions<Settings> settings)
    {
        _db = db;
        _settings = settings.Value;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);

        if (!_settings.SeedDemoData)
            return;

        await SeedRolesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
        await SeedUnitsAsync(cancellationToken);
        await SeedMaterialTypesAsync(cancellationToken);
        await SeedWarehousesAsync(cancellationToken);
        await SeedDepartmentsAsync(cancellationToken);
        await SeedPositionsAsync(cancellationToken);
        await SeedExpenseTypesAsync(cancellationToken);
        await SeedSuppliersAsync(cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken)
    {
        var roles = new[] { "Admin", "Manager" };

        foreach (var name in roles)
        {
            if (await _db.Roles.AnyAsync(r => r.Name == name, cancellationToken))
                continue;

            _db.Roles.Add(new RoleEntity(name));
        }
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        const string adminUsername = "admin";
        if (await _db.Users.AnyAsync(u => u.Username == adminUsername, cancellationToken))
            return;

        var adminRoleId = await _db.Roles
            .Where(r => r.Name == "Admin")
            .Select(r => r.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (adminRoleId == Guid.Empty)
            return; // role not seeded

        // Password hash is placeholder; replace with real hash when auth is implemented
        _db.Users.Add(new UserEntity(adminUsername, "admin", adminRoleId));
    }

    private async Task SeedUnitsAsync(CancellationToken cancellationToken)
    {
        var units = new (string Code, string Name)[]
        {
            ("pcs", "Pieces"),
            ("m", "Meters"),
            ("kg", "Kilograms")
        };

        foreach (var (code, name) in units)
        {
            if (await _db.Units.AnyAsync(u => u.Code == code, cancellationToken))
                continue;

            _db.Units.Add(new UnitEntity(code, name));
        }
    }

    private async Task SeedMaterialTypesAsync(CancellationToken cancellationToken)
    {
        var types = new[] { "Fabric", "Accessories" };

        foreach (var name in types)
        {
            if (await _db.MaterialTypes.AnyAsync(t => t.Name == name, cancellationToken))
                continue;

            _db.MaterialTypes.Add(new MaterialTypeEntity(name));
        }
    }

    private async Task SeedWarehousesAsync(CancellationToken cancellationToken)
    {
        var warehouses = new (string Name, WarehouseType Type)[]
        {
            ("Main Materials", WarehouseType.Materials),
            ("Finished Goods", WarehouseType.FinishedGoods)
        };

        foreach (var (name, type) in warehouses)
        {
            if (await _db.Warehouses.AnyAsync(w => w.Name == name, cancellationToken))
                continue;

            _db.Warehouses.Add(new WarehouseEntity(name, type));
        }
    }

    private async Task SeedDepartmentsAsync(CancellationToken cancellationToken)
    {
        var departments = new (string Name, DepartmentType Type, string? Code)[]
        {
            ("Production", DepartmentType.Production, "PROD"),
            ("Storage", DepartmentType.Storage, "STOR")
        };

        foreach (var (name, type, code) in departments)
        {
            if (await _db.Departments.AnyAsync(d => d.Name == name, cancellationToken))
                continue;

            _db.Departments.Add(new DepartmentEntity(name, type, code));
        }
    }

    private async Task SeedPositionsAsync(CancellationToken cancellationToken)
    {
        var productionDeptId = await _db.Departments
            .Where(d => d.Name == "Production")
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var storageDeptId = await _db.Departments
            .Where(d => d.Name == "Storage")
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var positions = new List<(string Name, Guid DepartmentId, string? Code, bool canCut, bool canSew, bool canPackage, bool canHandleMaterials)>
        {
            ("Seamstress", productionDeptId, "SEW", false, true, false, false),
            ("Cutter", productionDeptId, "CUT", true, false, false, false),
            ("Packer", productionDeptId, "PACK", false, false, true, false),
            ("Storekeeper", storageDeptId, "STORE", false, false, false, true)
        };

        foreach (var pos in positions)
        {
            if (pos.DepartmentId == Guid.Empty)
                continue;

            if (await _db.Positions.AnyAsync(p => p.Name == pos.Name, cancellationToken))
                continue;

            _db.Positions.Add(new PositionEntity(
                pos.Name,
                pos.DepartmentId,
                pos.Code,
                description: null,
                baseNormPerHour: null,
                baseRatePerNormHour: null,
                defaultPremiumPercent: null,
                canCut: pos.canCut,
                canSew: pos.canSew,
                canPackage: pos.canPackage,
                canHandleMaterials: pos.canHandleMaterials));
        }
    }

    private async Task SeedExpenseTypesAsync(CancellationToken cancellationToken)
    {
        var types = new (string Name, string? Description)[]
        {
            ("Rent", "Office or warehouse rent"),
            ("Utilities", "Electricity, water, heating"),
            ("Other", "Miscellaneous expenses")
        };

        foreach (var (name, description) in types)
        {
            if (await _db.ExpenseTypes.AnyAsync(et => et.Name == name, cancellationToken))
                continue;

            _db.ExpenseTypes.Add(new ExpenseTypeEntity(name, description));
        }
    }

    private async Task SeedSuppliersAsync(CancellationToken cancellationToken)
    {
        var suppliers = new (string Name, string? Description)[]
        {
            ("Default Supplier", "Initial supplier")
        };

        foreach (var (name, description) in suppliers)
        {
            if (await _db.Suppliers.AnyAsync(s => s.Name == name, cancellationToken))
                continue;

            _db.Suppliers.Add(new SupplierEntity(name, description));
        }
    }
}
