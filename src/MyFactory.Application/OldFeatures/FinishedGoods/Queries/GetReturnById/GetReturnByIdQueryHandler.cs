using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Queries.GetReturnById;

public sealed class GetReturnByIdQueryHandler : IRequestHandler<GetReturnByIdQuery, ReturnDto>
{
    private readonly IApplicationDbContext _context;

    public GetReturnByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReturnDto> Handle(GetReturnByIdQuery request, CancellationToken cancellationToken)
    {
        var customerReturn = await _context.CustomerReturns
            .AsNoTracking()
            .Include(entity => entity.Items)
            .FirstOrDefaultAsync(entity => entity.Id == request.ReturnId, cancellationToken)
            ?? throw new InvalidOperationException("Return not found.");

        return await ReturnDtoFactory.CreateAsync(_context, customerReturn, cancellationToken);
    }
}
