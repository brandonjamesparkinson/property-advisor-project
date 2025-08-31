using Asp.Versioning;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ListingsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] bool? forSale,
        [FromQuery] int? minBeds,
        [FromQuery] int? maxBeds,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] PropertyType? type,
        [FromQuery] string? postcode,
        [FromQuery] string sort = "newest", // newest|price-asc|price-desc
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        IQueryable<Listing> q = db.Listings
            .AsNoTracking()
            .Include(l => l.Property).ThenInclude(p => p.Address)
            .Include(l => l.Images)
            .Include(l => l.Branch)
            .Where(l => l.Status == ListingStatus.Published);

        if (forSale is not null) q = q.Where(l => l.ForSale == forSale);
        if (minBeds is not null) q = q.Where(l => l.Property.Bedrooms >= minBeds);
        if (maxBeds is not null) q = q.Where(l => l.Property.Bedrooms <= maxBeds);
        if (minPrice is not null) q = q.Where(l => l.Price >= minPrice);
        if (maxPrice is not null) q = q.Where(l => l.Price <= maxPrice);
        if (type is not null && type != PropertyType.Unknown) q = q.Where(l => l.Property.Type == type);
        if (!string.IsNullOrWhiteSpace(postcode)) q = q.Where(l => l.Property.Address.Postcode.StartsWith(postcode));

        q = sort switch
        {
            "price-asc" => q.OrderBy(l => l.Price).ThenByDescending(l => l.ListedUtc),
            "price-desc" => q.OrderByDescending(l => l.Price).ThenByDescending(l => l.ListedUtc),
            _ => q.OrderByDescending(l => l.ListedUtc)
        };

        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var result = new
        {
            page,
            pageSize,
            total,
            items = items.Select(l => new {
                l.Id,
                l.Price,
                l.Currency,
                l.ForSale,
                listedUtc = l.ListedUtc,
                property = new
                {
                    l.Property.Type,
                    l.Property.Bedrooms,
                    l.Property.Bathrooms,
                    address = new
                    {
                        l.Property.Address.Line1,
                        l.Property.Address.City,
                        l.Property.Address.Postcode,
                        l.Property.Address.Latitude,
                        l.Property.Address.Longitude
                    }
                },
                images = l.Images.OrderBy(i => i.SortOrder).Select(i => i.Url).ToList(),
                branch = l.Branch == null ? null : new { l.Branch.Name, l.Branch.Phone }
            })
        };

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var l = await db.Listings
            .AsNoTracking()
            .Include(x => x.Property).ThenInclude(p => p.Address)
            .Include(x => x.Images)
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == id && x.Status == ListingStatus.Published);

        if (l is null) return NotFound();

        return Ok(new
        {
            l.Id,
            l.Price,
            l.Currency,
            l.ForSale,
            l.Status,
            l.ListedUtc,
            property = new
            {
                l.Property.Type,
                l.Property.Bedrooms,
                l.Property.Bathrooms,
                l.Property.Receptions,
                address = new
                {
                    l.Property.Address.Line1,
                    l.Property.Address.City,
                    l.Property.Address.Postcode,
                    l.Property.Address.Latitude,
                    l.Property.Address.Longitude
                }
            },
            images = l.Images.OrderBy(i => i.SortOrder).Select(i => i.Url),
            branch = l.Branch == null ? null : new { l.Branch.Name, l.Branch.Phone, l.Branch.Email }
        });
    }
}
