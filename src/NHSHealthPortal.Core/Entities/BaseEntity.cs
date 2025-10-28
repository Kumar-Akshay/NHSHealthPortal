namespace NHSHealthPortal.Core.Entities;

/// <summary>
/// Define the BaseEntity
/// Describe the common attributes of each patient
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Set the Id field at initialization
    /// </summary>
    public required int Id { get; init; }
}