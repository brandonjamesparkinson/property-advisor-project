using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<ListingImage> ListingImages => Set<ListingImage>();
    public DbSet<Branch> Branches => Set<Branch>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Address>(e =>
        {
            e.Property(x => x.Line1).IsRequired().HasMaxLength(200);
            e.Property(x => x.City).IsRequired().HasMaxLength(100);
            e.Property(x => x.Postcode).IsRequired().HasMaxLength(10);
            e.HasIndex(x => x.Postcode);
        });

        b.Entity<Property>(e =>
        {
            e.HasOne(x => x.Address)
             .WithMany()
             .HasForeignKey(x => x.AddressId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Listing>(e =>
        {
            e.HasOne(x => x.Property)
             .WithMany()
             .HasForeignKey(x => x.PropertyId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Branch)
             .WithMany(b => b.Listings)
             .HasForeignKey(x => x.BranchId)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasMany(x => x.Images)
             .WithOne()
             .HasForeignKey(i => i.ListingId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.Status, x.ForSale, x.Price, x.ListedUtc });
        });
    }
}
