using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.OldFeatures.Files.Queries.ListFiles;

public sealed class ListFilesQueryHandler : IRequestHandler<ListFilesQuery, IReadOnlyList<FileResourceDto>>
{
    private readonly IApplicationDbContext _context;

    public ListFilesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<FileResourceDto>> Handle(ListFilesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.FileResources.AsNoTracking().AsQueryable();

        if (request.UploadedByUserId.HasValue)
        {
            query = query.Where(file => file.UploadedByUserId == request.UploadedByUserId.Value);
        }

        if (request.UploadedFrom.HasValue)
        {
            query = query.Where(file => file.UploadedAt >= request.UploadedFrom.Value);
        }

        if (request.UploadedTo.HasValue)
        {
            query = query.Where(file => file.UploadedAt <= request.UploadedTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.ContentType))
        {
            query = query.Where(file => file.ContentType == request.ContentType);
        }

        var files = await query
            .OrderByDescending(file => file.UploadedAt)
            .ToListAsync(cancellationToken);

        return files
            .Select(FileResourceDto.FromEntity)
            .ToList();
    }
}
