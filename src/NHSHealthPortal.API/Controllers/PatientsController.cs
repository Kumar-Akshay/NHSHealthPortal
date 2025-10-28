using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NHSHealthPortal.Core.DTOs;
using NHSHealthPortal.Core.Interfaces.Services;

namespace NHSHealthPortal.API.Controllers;

/// <summary>
/// Authentication was not in requirement and not implemented
/// handle two content type for OK and any error
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json", "application/problem+json")]
public sealed class PatientsController : ControllerBase
{
   
    private readonly ILogger<PatientsController> _logger;
    private readonly IPatientService _patientService;
    public PatientsController(ILogger<PatientsController> logger, IPatientService patientService)
    {
        _logger = logger;
        _patientService = patientService;
    }

    /// <summary>
    /// Define the Response Cache as None and NoStore because patient sensitive data won't store in CDN server or browser for privacy purpose
    /// Add versioning to the API in Future we need another modifed version of endpoint
    /// Add rate limiting to avoid overload server with request burst
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    
    [HttpGet]
    [EnableRateLimiting("fixed")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/books/{id}")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
    [ProducesDefaultResponseType(typeof(PatientDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PatientDto?> GetPatientById([FromRoute] int id)
    {
        _logger.LogInformation("Getting patient with id {id}", id);
        // Checking the Id is valid
        // It's good to select Guid as Initially
        // Due to requirement mention int type otherwise I would use the Guid
        // Because as Id grows we will require to do change type from int or long Guid
        if (id is int.MaxValue or <= 0)
        {
            _logger.LogError("Invalid id {id}",id);
            
            // Return Problem Details 
            return Problem(
                title: "Invalid Patient ID",
                detail: "Patient ID must be a positive integer",
                statusCode: StatusCodes.Status400BadRequest,
                instance: $"{HttpContext.Request.Path}");
        }

        var patientDetails = _patientService.GetById(id);
        if (patientDetails is not null) return Ok(patientDetails);
       
        _logger.LogError("Patient not found {id}", id);
        return Problem(
            title: "Patient Not Found",
            detail: $"No patient found with ID {id}",
            statusCode: StatusCodes.Status404NotFound,
            instance: $"{HttpContext.Request.Path}");

    }
}