namespace Assignment.Model.Entity
{
    public class HealthCareProvider
    {        
        public int ProviderId { get; set; }        
        public string FirstName { get; set; }       
        public string LastName { get; set; }        
        public string NPINumber { get; set; }        
        public string Phone { get; set; }
        public string Email { get; set; }
        public Address ProviderAddresses { get; set; } = new Address();
    }
}
