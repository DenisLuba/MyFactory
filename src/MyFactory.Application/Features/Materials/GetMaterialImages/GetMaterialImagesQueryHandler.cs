using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterialImages;

public sealed class GetMaterialImagesQueryHandler : IRequestHandler<GetMaterialImagesQuery, IReadOnlyList<MaterialImageDto>>
{
    private readonly IApplicationDbContext _db;

    public GetMaterialImagesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MaterialImageDto>> Handle(GetMaterialImagesQuery request, CancellationToken cancellationToken)
    {
        return await _db.MaterialImages
            .AsNoTracking()
            .Where(mi => mi.MaterialId == request.MaterialId)
            .OrderBy(mi => mi.SortOrder)
            .ThenBy(mi => mi.CreatedAt)
            .Select(mi => new MaterialImageDto
            {
                Id = mi.Id,
                MaterialId = mi.MaterialId,
                FileName = mi.FileName,
                Path = mi.Path,
                ContentType = mi.ContentType,
                SortOrder = mi.SortOrder,
                Content = null
            })
            .ToListAsync(cancellationToken);
    }
}
