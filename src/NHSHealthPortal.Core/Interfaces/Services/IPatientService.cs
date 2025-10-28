using NHSHealthPortal.Core.DTOs;
using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Core.Interfaces.Services;

/// <summary>
/// Define the all operation contract that can updated according to business rule
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Get the Patient data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    PatientDto? GetById(int id);
}