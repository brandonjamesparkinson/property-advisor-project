namespace Backend.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public required string Name { get; set; }       // e.g. "Acme Estates – Leeds"
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public IList<Listing> Listings { get; set; } = new List<Listing>();
    }
}
