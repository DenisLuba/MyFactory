using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Files;
using MyFactory.Domain.Entities.Files;

namespace MyFactory.Application.OldFeatures.Files.Commands.UploadFile;

public sealed class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileResourceDto>
{
    private readonly IApplicationDbContext _context;

    public UploadFileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileResourceDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var uploaderExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == request.UploadedByUserId, cancellationToken);

        if (!uploaderExists)
        {
            throw new InvalidOperationException("Uploaded-by user not found.");
        }

        var duplicateName = await _context.FileResources
            .AsNoTracking()
            .AnyAsync(
                file => file.UploadedByUserId == request.UploadedByUserId && file.FileName == request.FileName,
                cancellationToken);

        if (duplicateName)
        {
            throw new InvalidOperationException("File with the same name already exists for this user.");
        }

        var entity = new FileResource(
            request.FileName,
            request.StoragePath,
            request.ContentType,
            request.SizeBytes,
            request.UploadedByUserId,
            request.UploadedAt ?? DateTime.UtcNow,
            request.Description);

        await _context.FileResources.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return FileResourceDto.FromEntity(entity);
    }
}
