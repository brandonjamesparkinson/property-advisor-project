using FluentValidation;

namespace Backend.Application.Listings;

public class ListingsSearchValidator : AbstractValidator<ListingsSearchQuery>
{
    public ListingsSearchValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.MinBeds).GreaterThanOrEqualTo(0).When(x => x.MinBeds.HasValue);
        RuleFor(x => x.MaxBeds).GreaterThanOrEqualTo(0).When(x => x.MaxBeds.HasValue);
        RuleFor(x => x.MaxBeds).GreaterThanOrEqualTo(x => x.MinBeds)
            .When(x => x.MinBeds.HasValue && x.MaxBeds.HasValue);
        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice)
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);
        RuleFor(x => x.Sort).Must(s => new[] { "newest", "price-asc", "price-desc" }.Contains(s))
            .WithMessage("sort must be one of: newest, price-asc, price-desc");
    }
}
