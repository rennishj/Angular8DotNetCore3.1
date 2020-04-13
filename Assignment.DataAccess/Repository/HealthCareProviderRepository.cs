using Assignment.DataAccess.Abstractions;
using Assignment.Model.Abstractions;
using Assignment.Model.Entity;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Repository
{
    public class HealthCareProviderRepository : IProviderRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfigProvider _configProvider;
        private readonly ILogger _logger;

        public HealthCareProviderRepository(IConnectionFactory connection, IConfigProvider configProvider, ILogger<HealthCareProviderRepository> logger)
        {
            _connectionFactory = connection;
            _configProvider = configProvider;
            _logger = logger;
        }

        public async Task CreateAllStates(List<State> states)
        {
            try
            {
                using (var con = _connectionFactory.GetConnection(_configProvider.ConnectionString))
                {
                    con.Open();
                    var table = new DataTable();
                    table.Columns.Add("Name", typeof(string));
                    table.Columns.Add("Code", typeof(string));
                    foreach (var state in states)
                    {
                        var row = table.NewRow();
                        row["Name"] = state.Name;
                        row["Code"] = state.Code;
                        table.Rows.Add(row);
                    }

                    var result = await con.ExecuteAsync("exec dbo.CreateStates  @StateUdt", new
                    {
                        StateUdt = table.AsTableValuedParameter("dbo.StateUdt")
                    });
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating the states into the dbo.States table in CreateAllStates().", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a Provider and associated Provider address.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<HealthCareProvider> CreateProvider(HealthCareProvider provider)
        {
           
            using (var con = _connectionFactory.GetConnection(_configProvider.ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {

                    try
                    {
                        var dynamicParams = new DynamicParameters();
                        dynamicParams.Add("@FirstName", provider.FirstName);
                        dynamicParams.Add("@LastName", provider.LastName);
                        dynamicParams.Add("@Email", provider.Email);
                        dynamicParams.Add("@NPINumber", provider.NPINumber);
                        dynamicParams.Add("@Phone", provider.Phone);
                        dynamicParams.Add("@ProviderId", -1, DbType.Int32, System.Data.ParameterDirection.Output);
                        var rowsAffected = await con.ExecuteAsync("dbo.CreateProvider", dynamicParams, transaction, commandType: System.Data.CommandType.StoredProcedure);
                        var providerId = dynamicParams.Get<int>("@ProviderId");                        
                        if (providerId > -1)
                        {
                            provider.ProviderAddresses.ProviderId = providerId;                            
                            var addressParams = new DynamicParameters();
                            addressParams.Add("@ProviderId", provider.ProviderAddresses.ProviderId);
                            addressParams.Add("@Address1", provider.ProviderAddresses.Address1);
                            addressParams.Add("@City", provider.ProviderAddresses.City);
                            addressParams.Add("@ZipCode", provider.ProviderAddresses.ZipCode);
                            addressParams.Add("@AddressTypeId", provider.ProviderAddresses.AddressTypeId);
                            addressParams.Add("@StateId", provider.ProviderAddresses.StateId);
                            await con.ExecuteAsync("dbo.CreateProviderAddress", addressParams, transaction, commandType: System.Data.CommandType.StoredProcedure);
                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception($"ProviderId returned from database is {providerId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError($"Error creating the provider in CreateProvider().", ex);
                        throw;
                    }
                }
            }
            return provider;
        }

        public async Task<List<HealthCareProvider>> GetAll()
        {
            IEnumerable<HealthCareProvider> providers = null;
            using (var con = _connectionFactory.GetConnection(_configProvider.ConnectionString))
            {
                con.Open();
                try
                {
                    providers = await con.QueryAsync<HealthCareProvider>("dbo.GetAllProviders", commandType: System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error loading all the providers in GetAll().", ex);
                    throw;
                }
            }
            return providers?.ToList();
        }

        public async Task<List<State>> GetAllStates()
        {
            IEnumerable<State> states = null;
            using (var con = _connectionFactory.GetConnection(_configProvider.ConnectionString))
            {
                con.Open();
                try
                {
                    states = await con.QueryAsync<State>("dbo.GetAllStates", commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error loading all the states in GetAllStates().", ex);
                    throw;
                }
            }
            return states?.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public async Task<HealthCareProvider> GetById(int providerId)
        {
            HealthCareProvider theProvider = null;
            using (var con = _connectionFactory.GetConnection(_configProvider.ConnectionString))
            {
                con.Open();
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@ProviderId", providerId);
                    var providers = await con.QueryAsync<HealthCareProvider>("dbo.GetProviderById", dynamicParams, commandType: System.Data.CommandType.StoredProcedure);
                    if(providers != null && providers.Count() > 0)
                    {
                        theProvider = providers.First();
                        var providerAddress = await GetProviderAddressByProviderId(providerId);
                        theProvider.ProviderAddresses = providerAddress;
                    }
                    
                    return providers?.ToList()?.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error loading provider by Id in  GetById().", ex);
                    throw;
                }
            }
            
        }

        public async Task<Address> GetProviderAddressByProviderId(int providerId)
        {
            using (var con = _connectionFactory.GetConnection(_configProvider.ConnectionString))
            {
                con.Open();
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@ProviderId", providerId);
                    var providers = await con.QueryAsync<Address>("dbo.GetProviderAddressByProviderId", dynamicParams, commandType: System.Data.CommandType.StoredProcedure);
                    return providers?.ToList()?.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error loading provider address by Id in  GetProviderAddressByProviderId().", ex);
                    throw;
                }
            }
        }
    }
}
