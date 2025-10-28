using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NHSHealthPortal.Core.Entities;
using NHSHealthPortal.Core.Interfaces.Repositories;
using NHSHealthPortal.Data.DataContext;

namespace NHSHealthPortal.Data.Repositories;

/// <summary>
/// This Repository used to retrieve the data of patient from data source
/// it help to keep data store and separate it from business logic
/// Mark it sealed to avoid accidental inheritance
/// </summary>
public sealed class PatientRepository :  IPatientRepository
{
    // Define the logger 
    private readonly ILogger<PatientRepository> _logger;
    private readonly AppDbContext _db;
    public PatientRepository(ILogger<PatientRepository> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    /// <summary>
    /// This method used to get the daata from Data source based on Id parameter
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Patient? GetById(int id)
    {
        try
        {
            _logger.LogDebug($"GetByIdAsync called with id {id}");
            // get the patient details 
            // add AsNoTracking will inform the ef core that don't track this entity
            var patient = _db.Patients.AsNoTracking().SingleOrDefault(p => p.Id == id);
            return patient;
        }
        catch (InvalidOperationException ex)
        {
            // This exception occurs because SingleOrDefault return exception if more than 1 records exist
            _logger.LogError($"Patient Id {id}  has more than one record, issue: {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError($"GetByIdAsync exception {e}");
            throw;
        }
    }
}