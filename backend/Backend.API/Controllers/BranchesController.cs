using Asp.Versioning;
using Backend.Application.Branches;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BranchesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BranchDto>>> GetAll()
    {
        var branches = await db.Branches
            .AsNoTracking()
            .Select(b => new BranchDto(
                b.Id, b.Name, b.Phone, b.Email,
                b.Listings.Count(l => l.Status == Backend.Domain.Enums.ListingStatus.Published)))
            .ToListAsync();

        return Ok(branches);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BranchDto>> Get(int id)
    {
        var dto = await db.Branches
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BranchDto(
                b.Id, b.Name, b.Phone, b.Email,
                b.Listings.Count(l => l.Status == Backend.Domain.Enums.ListingStatus.Published)))
            .FirstOrDefaultAsync();

        return dto is null ? NotFound() : Ok(dto);
    }
}
