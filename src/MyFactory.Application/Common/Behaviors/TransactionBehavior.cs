using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IApplicationDbContext _context;

    public TransactionBehavior(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_context is not DbContext dbContext)
        {
            return await next();
        }

        if (dbContext.Database.CurrentTransaction is not null)
        {
            return await next();
        }

        var strategy = dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await transaction.CommitAsync(cancellationToken);
            return response;
        });
    }
}

