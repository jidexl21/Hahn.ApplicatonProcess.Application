using Hahn.ApplicationProcess.May2020.Core;

namespace Hahn.ApplicationProcess.May2020.Domain.Services
{
    public interface IApplicantService : IBaseService<Applicant>
    {
        Applicant Save(Applicant item);
        Applicant Update(Applicant item);
        bool Delete(Applicant item);
    }
}
