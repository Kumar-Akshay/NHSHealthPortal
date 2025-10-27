using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Core.Interfaces.Repositories;

public interface IPatientRepository
{
    Patient? GetById(int id);
}