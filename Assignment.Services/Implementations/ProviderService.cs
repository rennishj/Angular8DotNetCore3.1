using Assignment.DataAccess.Abstractions;
using Assignment.Model.Entity;
using Assignment.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.Services.Implementations
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepo;

        public ProviderService(IProviderRepository providerRepo)
        {
            _providerRepo = providerRepo;
        }

        public async Task CreateAllStates(List<State> states)
        {
            await _providerRepo.CreateAllStates(states);
        }

        public async Task<HealthCareProvider> CreateProvider(HealthCareProvider provider)
        {
            return await _providerRepo.CreateProvider(provider);
        }

        public async Task<List<HealthCareProvider>> GetAll()
        {
            return await _providerRepo.GetAll();
        }

        public async Task<List<State>> GetAllStates()
        {
            return await _providerRepo.GetAllStates();
        }

        public async Task<HealthCareProvider> GetById(int providerId)
        {
            return await _providerRepo.GetById(providerId);
        }
    }
}
