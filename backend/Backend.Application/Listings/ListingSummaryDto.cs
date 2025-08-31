using Backend.Domain.Entities;

namespace Backend.Application.Listings;

public static class ListingSummaryFactory
{
    public static ListingSummaryDto FromEntity(Listing l) =>
        new ListingSummaryDto(
            l.Id,
            l.Price,
            l.Currency,
            l.ForSale,
            l.ListedUtc,
            l.Property.Address.Line1,
            l.Property.Address.City,
            l.Property.Address.Postcode,
            l.Property.Bedrooms,
            l.Property.Bathrooms,
            l.Property.Type.ToString(),
            l.Images.OrderBy(i => i.SortOrder).Select(i => i.Url).FirstOrDefault(),
            l.Branch?.Name,
            l.Branch?.Phone
        );
}
