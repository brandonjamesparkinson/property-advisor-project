namespace Backend.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public required string Line1 { get; set; }
        public string? Line2 { get; set; }
        public required string City { get; set; }
        public required string Postcode { get; set; }   // e.g. "LS1 1AA"
        public string Country { get; set; } = "UK";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
