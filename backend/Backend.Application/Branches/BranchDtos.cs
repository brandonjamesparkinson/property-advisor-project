namespace Backend.Application.Branches;

public record BranchDto(int Id, string Name, string? Phone, string? Email, int ActiveListings);
