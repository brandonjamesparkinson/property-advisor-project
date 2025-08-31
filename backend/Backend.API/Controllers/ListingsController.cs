using Asp.Versioning;
using Backend.Application.Listings;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Infrastructure.Extensions;

namespace Backend.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ListingsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ListingsSearchQuery q)
    {
        var query = db.Listings
            .AsNoTracking()
            .Include(l => l.Property).ThenInclude(p => p.Address)
            .Include(l => l.Images)
            .Include(l => l.Branch)
            .Where(l => l.Status == ListingStatus.Published)
            .ApplySearch(q)
            .ApplySorting(q.Sort);

        var total = await query.CountAsync();

        var items = await query
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(l => ListingSummaryFactory.FromEntity(l))
            .ToListAsync();

        return Ok(new { q.Page, q.PageSize, total, items });
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
