namespace Assignment.Model.Entity
{
    public class Address
    {
        
        public int AddressId { get; set; }
        public int ProviderId { get; set; }        
        public string Address1 { get; set; }      
        public string City { get; set; }        
        public string State { get; set; }        
        public int StateId { get; set; }
        public string ZipCode { get; set; }       
        public int AddressTypeId { get; set; }
    }
}
