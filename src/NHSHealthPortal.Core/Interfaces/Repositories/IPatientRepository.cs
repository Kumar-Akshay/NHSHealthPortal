using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Core.Interfaces.Repositories;

/// <summary>
/// Define the operations of Patient Repository
/// </summary>
public interface IPatientRepository
{
    Patient? GetById(int id);
}