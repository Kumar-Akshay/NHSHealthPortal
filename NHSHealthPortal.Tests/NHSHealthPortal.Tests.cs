using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NHSHealthPortal.API.Controllers;
using NHSHealthPortal.Core.DTOs;
using NHSHealthPortal.Core.Interfaces.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace NHSHealthPortal.Tests;


public class NhsHealthPortalTests
{
    private readonly Mock<IPatientService> _patientService = new();
    private readonly Mock<ILogger<PatientsController>> _logger = new();

    private PatientsController CreateController(string path)
    {
        var controller = new PatientsController(_logger.Object, _patientService.Object);

        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Path = path
            }
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    [Fact]
    public void GetPatientById_IdLessOrEqualZero_ReturnsBadRequestProblem()
    {
        // Arrange
        var controller = CreateController("/v1/books/0");

        // Act
        ActionResult<PatientDto?> result = controller.GetPatientById(0);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result.Result);
        obj.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var pd = Assert.IsType<ProblemDetails>(obj.Value);
        pd.Title.Should().Be("Invalid Patient ID");
        pd.Detail.Should().Be("Patient ID must be a positive integer");
        pd.Instance.Should().Be("/v1/books/0");

        _patientService.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void GetPatientById_IdIsIntMaxValue_ReturnsBadRequestProblem()
    {
        // Arrange
        var controller = CreateController($"/v1/books/{int.MaxValue}");

        // Act
        ActionResult<PatientDto?> result = controller.GetPatientById(int.MaxValue);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result.Result);
        obj.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var pd = Assert.IsType<ProblemDetails>(obj.Value);
        pd.Title.Should().Be("Invalid Patient ID");
        pd.Detail.Should().Be("Patient ID must be a positive integer");
        pd.Instance.Should().Be($"/v1/books/{int.MaxValue}");

        _patientService.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void GetPatientById_NotFound_ReturnsNotFoundProblem()
    {
        // Arrange
        const int id = 123;
        _patientService.Setup(s => s.GetById(id)).Returns((PatientDto?)null);
        var controller = CreateController($"/v1/books/{id}");

        // Act
        ActionResult<PatientDto?> result = controller.GetPatientById(id);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result.Result);
        obj.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        var pd = Assert.IsType<ProblemDetails>(obj.Value);
        pd.Title.Should().Be("Patient Not Found");
        pd.Detail.Should().Be($"No patient found with ID {id}");
        pd.Instance.Should().Be($"/v1/books/{id}");

        _patientService.Verify(s => s.GetById(id), Times.Once);
    }

    [Fact]
    public void GetPatientById_Found_ReturnsOkWithPatient()
    {
        // Arrange
        const int id = 42;
        var expected = new PatientDto
        {
            Id = id,
            Name = "Alice",
            DateOfBirth = default,
            NhsNumber = string.Empty,
            GpPractice = string.Empty,
        };
        _patientService.Setup(s => s.GetById(id)).Returns(expected);
        var controller = CreateController($"/v1/books/{id}");

        // Act
        ActionResult<PatientDto?> result = controller.GetPatientById(id);

        // Assert 
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<PatientDto>(ok.Value);
        dto.Should().BeSameAs(expected);

        _patientService.Verify(s => s.GetById(id), Times.Once);
    }


}