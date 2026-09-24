using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Entities.Identity;
using AerationSterilize.Domain.Entities.Working;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Action = AerationSterilize.Domain.Entities.Identity.Action;

namespace AerationSterilize.Persistence;
public class ApplicationContextSeed
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger _logger;

    public ApplicationContextSeed(
        ApplicationDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ILogger logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while initialzing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedDefaultUserAsync();
            await SeedFunctionsAsync();
            await SeedActionsAsync();
            await SeedActionInFunctionsAsync();
            await SeedPermissionsAsync();
            await SeedProductsAsync();
            await SeedAerationColumnsAsync();
            await SeedAerationPositionsAsync();
            await SeedWorkLinesAsync();
            await SeedWorkTablesAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        if (await _roleManager.Roles.AnyAsync()) return;

        var roles = new[]
        {
            new AppRole
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                RoleCode = "ADMIN",
                Description = "System administrator with full access"
            },
            new AppRole
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "User",
                NormalizedName = "USER",
                RoleCode = "USER",
                Description = "Standard application user"
            }
        };

        foreach (var role in roles)
        {
            await _roleManager.CreateAsync(role);
        }

        _logger.Information("Seeded {Count} roles.", roles.Length);
    }

    private async Task SeedDefaultUserAsync()
    {
        if (await _userManager.Users.AnyAsync()) return;

        var admin = new AppUser
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@aerationsterilize.local",
            NormalizedEmail = "ADMIN@AERATIONSTERILIZE.LOCAL",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            FullName = "System Administrator",
            DayOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsDirector = true,
            IsHeadOfDepartment = true,
            ManagerId = Guid.Empty,
            PositionId = Guid.Empty,
            IsReceipient = 1,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(admin, "Admin@123");
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(admin, "Administrator");
            _logger.Information("Seeded default admin user.");
        }
        else
        {
            _logger.Warning("Failed to seed admin user: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    private async Task SeedFunctionsAsync()
    {
        if (await _context.Functions.AnyAsync()) return;

        var functions = new[]
        {
            new Function { Id = "DASHBOARD", Name = "Dashboard", Url = "/dashboard", ParrentId = string.Empty, SortOrder = 1, CssClass = "fa-dashboard", IsActive = true },
            new Function { Id = "SYSTEM",    Name = "System",    Url = "/system",    ParrentId = string.Empty, SortOrder = 2, CssClass = "fa-cogs",      IsActive = true },
            new Function { Id = "USER",      Name = "User",      Url = "/system/user", ParrentId = "SYSTEM", SortOrder = 1, CssClass = "fa-user",   IsActive = true },
            new Function { Id = "ROLE",      Name = "Role",      Url = "/system/role", ParrentId = "SYSTEM", SortOrder = 2, CssClass = "fa-users",  IsActive = true },
            new Function { Id = "PRODUCT",   Name = "Product",   Url = "/product",   ParrentId = string.Empty, SortOrder = 3, CssClass = "fa-cube",       IsActive = true }
        };

        await _context.Functions.AddRangeAsync(functions);
        _logger.Information("Seeded {Count} functions.", functions.Length);
    }

    private async Task SeedActionsAsync()
    {
        if (await _context.Actions.AnyAsync()) return;

        var actions = new[]
        {
            new Action { Id = "VIEW",   Name = "View",   SortOrder = 1, IsActive = true },
            new Action { Id = "CREATE", Name = "Create", SortOrder = 2, IsActive = true },
            new Action { Id = "UPDATE", Name = "Update", SortOrder = 3, IsActive = true },
            new Action { Id = "DELETE", Name = "Delete", SortOrder = 4, IsActive = true }
        };

        await _context.Actions.AddRangeAsync(actions);
        _logger.Information("Seeded {Count} actions.", actions.Length);
    }

    private async Task SeedActionInFunctionsAsync()
    {
        if (await _context.ActionInFunctions.AnyAsync()) return;

        var functionIds = new[] { "DASHBOARD", "SYSTEM", "USER", "ROLE", "PRODUCT" };
        var actionIds = new[] { "VIEW", "CREATE", "UPDATE", "DELETE" };

        var actionInFunctions = functionIds
            .SelectMany(f => actionIds.Select(a => new ActionInFunction { FunctionId = f, ActionId = a }))
            .ToList();

        await _context.ActionInFunctions.AddRangeAsync(actionInFunctions);
        _logger.Information("Seeded {Count} action-in-functions.", actionInFunctions.Count);
    }

    private async Task SeedPermissionsAsync()
    {
        if (await _context.Permissions.AnyAsync()) return;

        var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var functionIds = new[] { "DASHBOARD", "SYSTEM", "USER", "ROLE", "PRODUCT" };
        var actionIds = new[] { "VIEW", "CREATE", "UPDATE", "DELETE" };

        var permissions = functionIds
            .SelectMany(f => actionIds.Select(a => new Permission
            {
                RoleId = adminRoleId,
                FunctionId = f,
                ActionId = a
            }))
            .ToList();

        await _context.Permissions.AddRangeAsync(permissions);
        _logger.Information("Seeded {Count} permissions for Administrator.", permissions.Count);
    }

    private async Task SeedProductsAsync()
    {
        if (await _context.Products.AnyAsync()) return;

        var products = new[]
        {
            new Product(
                Guid.Parse("aaaaaaaa-0001-0000-0000-000000000001"),
                "Aeration Module A1",
                1500.00m,
                "High-efficiency aeration module for medium-sized tanks."),
            new Product(
                Guid.Parse("aaaaaaaa-0001-0000-0000-000000000002"),
                "Sterilizer Unit S2",
                2750.50m,
                "UV-based sterilizer unit with auto-cleaning support."),
            new Product(
                Guid.Parse("aaaaaaaa-0001-0000-0000-000000000003"),
                "Ozone Generator O3",
                3200.00m,
                "Industrial ozone generator for disinfection processes."),
            new Product(
                Guid.Parse("aaaaaaaa-0001-0000-0000-000000000004"),
                "Diffuser Plate D4",
                450.75m,
                "Ceramic diffuser plate producing fine bubbles for aeration.")
        };

        var now = DateTimeOffset.UtcNow;
        foreach (var p in products)
        {
            p.CreatedDate = now;
        }

        await _context.Products.AddRangeAsync(products);
        _logger.Information("Seeded {Count} products.", products.Length);
    }

    private async Task SeedAerationColumnsAsync()
    {
        if (await _context.AerationColumns.AnyAsync())
            return;

        var columns = new[]
        {
        new AerationColumn
        {
            Id = 1,
            ColumnName = "A",
            Status = true
        },
        new AerationColumn
        {
            Id = 2,
            ColumnName = "B",
            Status = true
        },
        new AerationColumn
        {
            Id = 3,
            ColumnName = "C",
            Status = true
        },
        new AerationColumn
        {
            Id = 4,
            ColumnName = "D",
            Status = true
        },
        new AerationColumn
        {
            Id = 5,
            ColumnName = "E",
            Status = true
        },
        new AerationColumn
        {
            Id = 6,
            ColumnName = "G",
            Status = true
        },
        new AerationColumn
        {
            Id = 7,
            ColumnName = "H",
            Status = true
        }
    };

        await _context.AerationColumns.AddRangeAsync(columns);

        _logger.Information("Seeded {Count} aeration columns.", columns.Length);
    }

    private async Task SeedAerationPositionsAsync()
    {
        if (await _context.AerationPositions.AnyAsync())
            return;

        var positions = new List<AerationPosition>();

        void AddPositions(string prefix, int max, int columnId)
        {
            for (int i = 1; i <= max; i++)
            {
                positions.Add(new AerationPosition
                {
                    PositionCode = $"{prefix}{i}",
                    Image = null,
                    Status = Domain.Enums.AerationStatus.Empty,
                    AerationColumnId = columnId,
                    CreatedDate = DateTimeOffset.UtcNow
                });
            }
        }

        AddPositions("A", 7, 1);
        AddPositions("B", 7, 2);
        AddPositions("C", 7, 3);
        AddPositions("D", 8, 4);
        AddPositions("E", 8, 5);
        AddPositions("G", 13, 6);
        AddPositions("H", 13, 7);

        await _context.AerationPositions.AddRangeAsync(positions);

        _logger.Information(
            "Seeded {Count} aeration positions.",
            positions.Count);
    }

    private async Task SeedWorkLinesAsync()
    {
        if (await _context.WorkLines.AnyAsync())
            return;

        var workLines = new[]
        {
            new WorkLine { Id = 1, Name = "Line 1", Status = true },
            new WorkLine { Id = 2, Name = "Line 2", Status = true },
            new WorkLine { Id = 3, Name = "Line 3", Status = true }
        };

        await _context.WorkLines.AddRangeAsync(workLines);

        _logger.Information("Seeded {Count} work lines.", workLines.Length);
    }

    private async Task SeedWorkTablesAsync()
    {
        if (await _context.WorkTables.AnyAsync())
            return;

        var workTables = new[]
        {
            new WorkTable { Id = 1, Name = "Table 1", Description = "Bàn thao tác 1 - Line 1", Status = Shared.Emumerations.WorkTableStatus.Empty, WorkLineId = 1 },
            new WorkTable { Id = 2, Name = "Table 2", Description = "Bàn thao tác 2 - Line 1", Status = Shared.Emumerations.WorkTableStatus.Empty, WorkLineId = 1 },
            new WorkTable { Id = 3, Name = "Table 1", Description = "Bàn thao tác 1 - Line 2", Status = Shared.Emumerations.WorkTableStatus.Empty, WorkLineId = 2 },
            new WorkTable { Id = 4, Name = "Table 2", Description = "Bàn thao tác 2 - Line 2", Status = Shared.Emumerations.WorkTableStatus.Empty, WorkLineId = 2 },
            new WorkTable { Id = 5, Name = "Table 1", Description = "Bàn thao tác 1 - Line 3", Status = Shared.Emumerations.WorkTableStatus.Empty, WorkLineId = 3 }
        };

        await _context.WorkTables.AddRangeAsync(workTables);

        _logger.Information("Seeded {Count} work tables.", workTables.Length);
    }
}
