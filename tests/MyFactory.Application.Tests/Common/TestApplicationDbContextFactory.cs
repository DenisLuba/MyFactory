using System;
using Microsoft.EntityFrameworkCore;

namespace MyFactory.Application.Tests.Common;

internal static class TestApplicationDbContextFactory
{
    public static TestApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
