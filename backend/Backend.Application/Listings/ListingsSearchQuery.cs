namespace Backend.Application.Listings;

public class ListingsSearchQuery
{
    public bool? ForSale { get; set; }
    public int? MinBeds { get; set; }
    public int? MaxBeds { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Type { get; set; }
    public string? Postcode { get; set; }
    public string Sort { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
