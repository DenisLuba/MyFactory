namespace MyFactory.Infrastructure.Persistence.Auditing;

/// <summary>
/// Marker interface allowing aggregates to opt into automatic audit timestamps when layering permits.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// True when infrastructure may assign CreatedAt/UpdatedAt values automatically.
    /// </summary>
    bool UseAutomaticTimestamps { get; }
}
