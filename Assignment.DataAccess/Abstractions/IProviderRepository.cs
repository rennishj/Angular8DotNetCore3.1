using Assignment.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Abstractions
{
    public interface IProviderRepository
    {
        Task<HealthCareProvider> CreateProvider(HealthCareProvider provider);        
        Task<List<HealthCareProvider>>GetAll();
        Task<HealthCareProvider> GetById(int providerId);
        Task<Address> GetProviderAddressByProviderId(int providerId);
        Task CreateAllStates(List<State> states);
        Task<List<State>>GetAllStates();
    }
}
