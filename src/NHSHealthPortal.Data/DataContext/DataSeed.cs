using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Data.DataContext;

public class DataSeed
{
    /// <summary>
    /// this method prepare the data and save the records
    /// </summary>
    /// <param name="db"></param>
    /// <param name="ct"></param>
    public static async Task EnsurePatientsSeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        // check in-memory data source first
        if (await db.Patients.AnyAsync(ct)) return;

        // perpare the records 
        var records = new List<Patient>()
        {
            new()
            {
                Id = 1,
                NhsNumber = "34290904532",
                Name = "John Smith",
                DateOfBirth = new DateTime(1995, 2, 11),
                GpPractice = "Park View Medical Clinic Middlesbrough"
            },
            new()
            {
                Id = 2,
                NhsNumber = "2345678901",
                Name = "Sarah Johnson",
                DateOfBirth = new DateTime(1990, 8, 22),
                GpPractice = "Park Surgery Middlesbrough"
            },

            new()
            {
                Id = 3,
                NhsNumber = "3456789012",
                Name = "Michael Brown",
                DateOfBirth = new DateTime(1998, 1, 5),
                GpPractice = "Linthorpe Surgery Middlesbrough"
            },

            new()
            {
                Id = 4,
                NhsNumber = "4567890123",
                Name = "Emily Davis",
                DateOfBirth = new DateTime(1995, 3, 30),
                GpPractice = "One Life Medical Centre"
            },
            new()
            {
                Id = 5,
                NhsNumber = "5678901234",
                Name = "Akshay Kumar",
                DateOfBirth = new DateTime(1997, 7, 14),
                GpPractice = "Cleveland Health Centre"
            }
        };


        // add the records to database in memory
        db.AddRange(records);
        await db.SaveChangesAsync(ct);
    }
}