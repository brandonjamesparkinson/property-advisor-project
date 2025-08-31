using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/dev")]
public class DevController(AppDbContext db) : ControllerBase
{
    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        if (db.Listings.Any()) return Ok("Already seeded");

        var addr = new Address { Line1 = "1 Test Street", City = "Leeds", Postcode = "LS1 1AA", Latitude = 53.7996, Longitude = -1.5491 };
        var prop = new Property { Type = PropertyType.Terrace, Bedrooms = 3, Bathrooms = 1, Receptions = 1, Address = addr };
        var branch = new Branch { Name = "Acme Estates - Leeds", Phone = "0113 000 0000", Email = "leeds@acme.test" };
        var listing = new Listing { Property = prop, Branch = branch, Price = 245000m, ForSale = true };
        listing.Images.Add(new ListingImage { Url = "https://picsum.photos/seed/house1/800/600", SortOrder = 1 });

        db.Add(listing);
        await db.SaveChangesAsync();
        return Ok("Seeded");
    }
}
