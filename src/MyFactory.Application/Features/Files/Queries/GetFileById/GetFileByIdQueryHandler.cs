using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.Features.Files.Queries.GetFileById;

public sealed class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, FileResourceDto>
{
    private readonly IApplicationDbContext _context;

    public GetFileByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileResourceDto> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await _context.FileResources
            .AsNoTracking()
            .FirstOrDefaultAsync(resource => resource.Id == request.FileId, cancellationToken)
            ?? throw new InvalidOperationException("File not found.");

        return FileResourceDto.FromEntity(file);
    }
}
