namespace ResidenceMocker.Entities;

public sealed class Guest
{
    public string NationalId { get; set; } = null!;

    public int? Wallet { get; set; }

    public Account Account { get; set; } = null!;

    public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
}
