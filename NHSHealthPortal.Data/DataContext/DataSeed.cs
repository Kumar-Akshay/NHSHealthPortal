using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Data.DataContext;

public static class DataSeed
{
    public static List<Patient> GetPatients()
    {
        return
        [
            new Patient
            {
                Id = 1,
                NhsNumber = "1234567890",
                Name = "John Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                GpPractice = "London Central Surgery",
                IsActive = true,
            },

            new Patient
            {
                Id = 2,
                NhsNumber = "2345678901",
                Name = "Sarah Johnson",
                DateOfBirth = new DateTime(1990, 8, 22),
                GpPractice = "Manchester North Medical Centre",
                IsActive = true,
            },

            new Patient
            {
                Id = 3,
                NhsNumber = "3456789012",
                Name = "Michael Brown",
                DateOfBirth = new DateTime(1978, 12, 5),
                GpPractice = "Birmingham South Practice",
                IsActive = true,
            },

            new Patient
            {
                Id = 4,
                NhsNumber = "4567890123",
                Name = "Emily Davis",
                DateOfBirth = new DateTime(1995, 3, 30),
                GpPractice = "London Central Surgery",
                IsActive = false,
            },

            new Patient
            {
                Id = 5,
                NhsNumber = "5678901234",
                Name = "David Wilson",
                DateOfBirth = new DateTime(1982, 7, 14),
                GpPractice = "Leeds East Health Centre",
                IsActive = true,
            }
        ];
    }
}