using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Finance;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class AdvanceItemDtoExample : IExamplesProvider<List<AdvanceItemDto>>
{
    public List<AdvanceItemDto> GetExamples()
    {
        return new List<AdvanceItemDto>
        {
            new AdvanceItemDto(
                "ADV-2024-001",
                "Иванов И.И.",
                15000,
                "2024-06-01",
                AdvanceStatus.Issued
            ),
            new AdvanceItemDto(
                "ADV-2024-002",
                "Петров П.П.",
                20000,
                "2024-06-05",
                AdvanceStatus.Reported
            )
        };
    }
}
