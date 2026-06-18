namespace MyFactory.Application.DTOs.Common;

public sealed record ListDto<Item>
{
    public IReadOnlyList<Item> Items { get; set; } = [];
    public int TotalCount { get; init; }
    public int Take { get; init; }
    public int Skip { get; init; }
    public bool HasMore { get; init; }
}