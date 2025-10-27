using Microsoft.Extensions.Logging;
using NHSHealthPortal.Core.Entities;
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
    public Patient? GetById(int id)
    {
        // handle the Id field limit as patient grow we need to update it to long or guid
        // as per requirement I have to use Id otherwise I perfer the Guid as Id
        if (id is int.MinValue or int.MaxValue)
        {
            _logger.LogError($"Invalid patient id: {id}");
            return null;
        }
        //
        if (id <= 0)
        {
            _logger.LogError($"Invalid patient id: {id}");
            return null;
        }
        
        try
        {
            // Get the Patient details
            var patient = _patientRepository.GetById(id);
            if (patient is not null) return patient;

            _logger.LogError($"Patient not found: {id}");
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Patient not found: {id}");
            throw;
        }
    }
}