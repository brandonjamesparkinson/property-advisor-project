namespace Backend.Application.Listings;

public record ListingSummaryDto(
    int Id, decimal Price, string Currency, bool ForSale, DateTime ListedUtc,
    string AddressLine1, string City, string Postcode,
    int Bedrooms, int Bathrooms, string PropertyType,
    string? PrimaryImageUrl, string? BranchName, string? BranchPhone);

public record ListingDetailDto(
    int Id, decimal Price, string Currency, bool ForSale, string Status, DateTime ListedUtc,
    int Bedrooms, int Bathrooms, int Receptions, string PropertyType, string Tenure,
    string Line1, string? Line2, string City, string Postcode, double? Latitude, double? Longitude,
    IEnumerable<string> Images, string? BranchName, string? BranchPhone, string? BranchEmail);
