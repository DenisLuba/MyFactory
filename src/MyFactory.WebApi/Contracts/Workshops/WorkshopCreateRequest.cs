using System.ComponentModel.DataAnnotations;

namespace MyFactory.WebApi.Contracts.Workshops;

public record WorkshopCreateRequest(
    [Required]
    [StringLength(200, MinimumLength = 2)]
    string Name,
    [EnumDataType(typeof(WorkshopType))]
    WorkshopType Type,
    [EnumDataType(typeof(WorkshopStatus))]
    WorkshopStatus Status
);
