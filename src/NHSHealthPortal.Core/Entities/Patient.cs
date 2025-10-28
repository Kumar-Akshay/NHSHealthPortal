namespace NHSHealthPortal.Core.Entities;

/// <summary>
/// Patient Entity inherited the common attribute from base entity
/// </summary>
public class Patient : BaseEntity
{
    public required string Name { get; init; } = string.Empty;
    public required DateTime DateOfBirth { get; init; }
    public required string NhsNumber { get; init; } = string.Empty;
    public required string GpPractice { get; init; } = string.Empty;
}