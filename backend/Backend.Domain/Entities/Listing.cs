using Backend.Domain.Enums;

namespace Backend.Domain.Entities
{
    public class Listing
    {
        public int Id { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public required decimal Price { get; set; }
        public string Currency { get; set; } = "GBP";
        public bool ForSale { get; set; } = true;       // false = To Let

        public ListingStatus Status { get; set; } = ListingStatus.Published;
        public DateTime ListedUtc { get; set; } = DateTime.UtcNow;

        public IList<ListingImage> Images { get; set; } = new List<ListingImage>();

        // Optional quick flags users expect to filter by (kept simple; derive later if needed)
        public bool NewBuild { get; set; }
        public bool ChainFree { get; set; }
    }
}
