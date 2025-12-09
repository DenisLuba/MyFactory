using System;
using Microsoft.EntityFrameworkCore;
using MyFactory.Infrastructure.Persistence;

namespace MyFactory.Infrastructure.Tests;

internal static class TestDbContextFactory
{
    public static ApplicationDbContext CreateContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}
