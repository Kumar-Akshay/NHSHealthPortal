using NHSHealthPortal.Core.DTOs;
using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Core.Extensions;

/// <summary>
/// This extension helper method used to calculate the Age and 
/// Transfer the Entity to DTO becuase Entity is used for data layer
/// DTO is used at API layer and we don't need to expose our entities to outside world
/// </summary>
public static class PatientExtensions
{
    public static PatientDto ToDto(this Patient? patient)
    {
        if (patient == null) return null;

        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name,
            DateOfBirth = patient.DateOfBirth,
            NhsNumber = patient.NhsNumber,
            GpPractice = patient.GpPractice
            
        };
    }

    public static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}