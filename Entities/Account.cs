namespace ResidenceMocker.Entities;

public sealed class Account
{
    public string NationalId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime JoinedAt { get; set; }

    public Guest? Guest { get; set; }

    public Host? Host { get; set; }
}
