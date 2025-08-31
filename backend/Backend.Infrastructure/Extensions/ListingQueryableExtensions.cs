using Backend.Application.Listings;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Extensions;

public static class ListingQueryableExtensions
{
    public static IQueryable<Listing> ApplySearch(this IQueryable<Listing> query, ListingsSearchQuery q)
    {
        if (q.ForSale is not null)
            query = query.Where(l => l.ForSale == q.ForSale);

        if (q.MinBeds is not null)
            query = query.Where(l => l.Property.Bedrooms >= q.MinBeds);

        if (q.MaxBeds is not null)
            query = query.Where(l => l.Property.Bedrooms <= q.MaxBeds);

        if (q.MinPrice is not null)
            query = query.Where(l => l.Price >= q.MinPrice);

        if (q.MaxPrice is not null)
            query = query.Where(l => l.Price <= q.MaxPrice);

        if (!string.IsNullOrWhiteSpace(q.Type))
            query = query.Where(l => l.Property.Type.ToString() == q.Type);

        if (!string.IsNullOrWhiteSpace(q.Postcode))
            query = query.Where(l => l.Property.Address.Postcode.StartsWith(q.Postcode));

        return query;
    }

    public static IQueryable<Listing> ApplySorting(this IQueryable<Listing> query, string? sort)
    {
        return sort switch
        {
            "price-asc" => query.OrderBy(l => l.Price).ThenByDescending(l => l.ListedUtc),
            "price-desc" => query.OrderByDescending(l => l.Price).ThenByDescending(l => l.ListedUtc),
            _ => query.OrderByDescending(l => l.ListedUtc)
        };
    }
}
