using Backend.Domain.Enums;

namespace Backend.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public required PropertyType Type { get; set; }
        public Tenure Tenure { get; set; } = Tenure.Unknown;

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Receptions { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
    }
}
