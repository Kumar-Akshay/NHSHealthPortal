using Asp.Versioning;
using Microsoft.OpenApi.Models;
using NHSHealthPortal.API.Helper;
using NHSHealthPortal.Core.Interfaces.Repositories;
using NHSHealthPortal.Core.Interfaces.Services;
using NHSHealthPortal.Data.Repositories;
using NHSHealthPortal.Data.Services;
using Microsoft.EntityFrameworkCore;
using NHSHealthPortal.Data.DataContext;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("PatientsDb"));


// Add the RFC 9457 problem+json response for not ok response
builder.Services.AddProblemDetails();

// Registers API versioning service, allowing the application to support multiple API versions 
// // and mark older versions as deprecated when new ones are introduced.
builder.Services
    .AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader());
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";    
        options.SubstituteApiVersionInUrl = true;
    });

// Configure the Swagger UI version and name
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NHSHealthPortal API", Version = "v1" });
    // Only include actions that belong to "v1"
    options.DocInclusionPredicate((docName, apiDesc) =>
        string.Equals(apiDesc.GroupName, docName, StringComparison.OrdinalIgnoreCase));
});

// Register the services with Scoped, it use one instance per request and shared with request lifetime 
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

// Apply the Fixed rate limiter with limit 100 request per second
// All config values define in app settings json
builder.Services.UseEndpointFixedRateLimiter(builder.Configuration);

var app = builder.Build();

// During Development make it to use Swagger UI and Disable it for Production
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(); 
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NHSHealthPortal API v1");
        c.RoutePrefix = "swagger"; 
    });
}


// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DataSeed.EnsurePatientsSeedAsync(db);
}

// Add the middleware to handle the exception in error response
app.UseExceptionHandler();    
app.UseStatusCodePages();

app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
app.MapControllers();
app.Run();
