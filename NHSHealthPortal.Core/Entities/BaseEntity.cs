namespace NHSHealthPortal.Core.Entities;

/// <summary>
/// Define the BaseEntity
/// Describe the common attributes of each patient
/// </summary>
public abstract class BaseEntity
{
    public required int Id { get; set; }
    public bool IsActive { get; set; } = true;
}