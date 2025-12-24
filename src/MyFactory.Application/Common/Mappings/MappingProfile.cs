using AutoMapper;
using MyFactory.Application.DTOs.Materials;
using MyFactory.Application.Features.Products.CreateProduct;
using MyFactory.Application.Features.Products.UpdateProduct;

namespace MyFactory.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create
        new CreateProductCommand(
            dto.Sku,
            dto.Name,
            dto.Status,
            dto.PlanPerHour
        );

        // Update
        new UpdateProductCommand(
            dto.Id!.Value,
            dto.Name,
            dto.PlanPerHour,
            dto.Status
        );

    }
}

