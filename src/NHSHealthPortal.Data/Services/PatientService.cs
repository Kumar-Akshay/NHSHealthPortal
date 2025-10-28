using Microsoft.Extensions.Logging;
using NHSHealthPortal.Core.DTOs;
using NHSHealthPortal.Core.Extensions;
using NHSHealthPortal.Core.Interfaces.Repositories;
using NHSHealthPortal.Core.Interfaces.Services;

namespace NHSHealthPortal.Data.Services;

/// <summary>
/// PatientService class is handle and manipulate the data according to the business rules 
/// Mark it sealed to avoid accidental inheritance
/// </summary>
public sealed class PatientService : IPatientService
{
    private readonly ILogger<PatientService> _logger;
    /// <summary>
    /// Inject the patient repository service to retrieve the patient data 
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    public PatientService(ILogger<PatientService> logger,IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get the Patient details by Id
    /// Validate the Id parameter 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PatientDto? GetById(int id)
    {
        try
        {
            _logger.LogInformation("Getting patient with id {id}", id);
            var patient = (_patientRepository.GetById(id)).ToDto();
            return patient;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Patient not found: {id}");
            throw;
        }
    }
}