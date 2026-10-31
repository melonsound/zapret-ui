using ZapretUI.Domain.Enums;

namespace ZapretUI.Domain.ValueObjects;

public sealed record ServiceConflict
{
    public required ServiceConflictKind Kind { get; init; }
    public required string Description { get; init; }
    public string? RelatedServiceName { get; init; }
}
