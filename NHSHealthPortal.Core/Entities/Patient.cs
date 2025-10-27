namespace NHSHealthPortal.Core.Entities;

/// <summary>
/// Patient Entity inherited the common attribute from base entity
/// </summary>
public class Patient : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public required DateTime DateOfBirth { get; set; }
    public required string NhsNumber { get; set; } = string.Empty;
    public required string GpPractice { get; set; } = string.Empty;
}