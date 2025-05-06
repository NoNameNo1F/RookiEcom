namespace RookiEcom.Domain.Shared
{
    public class Address
    {
        public string Street { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string City { get; set; }

        public Address()
        {
        }
        
        public Address(string street, string district, string ward, string city)
        {
            Street = street;
            District = district;
            Ward = ward;
            City = city;
        }
    }
}