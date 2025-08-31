namespace Backend.Domain.Entities
{
    public class ListingImage
    {
        public int Id { get; set; }
        public required string Url { get; set; }        // Later: move to blob storage
        public int SortOrder { get; set; } = 0;
        public int ListingId { get; set; }
    }
}
