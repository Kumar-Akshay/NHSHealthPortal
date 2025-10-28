using NHSHealthPortal.Core.Extensions;

namespace NHSHealthPortal.Core.DTOs;

public class PatientDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required DateTime DateOfBirth { get; init; }
    public required string NhsNumber { get; init; } = string.Empty;
    public required string GpPractice { get; init; } = string.Empty;
    
    // Calculate Fields per patient, it help to determine the age and adult status
    public int Age => PatientExtensions.CalculateAge(DateOfBirth);
    public bool IsAdult => Age >= 18;
}