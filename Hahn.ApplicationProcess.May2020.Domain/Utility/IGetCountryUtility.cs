using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.May2020.Domain.Utility
{
    public interface IGetCountryUtility
    {
        Task<ICountry> GetCountryAsync(string suppliedName);
        ICountry GetCountry(string suppliedName);
    }
}
