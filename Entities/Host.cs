namespace ResidenceMocker.Entities;

public sealed class Host
{
    public string NationalId { get; set; } = null!;

    public byte[]? NationalCardImage { get; set; }

    public bool? IsVerified { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public Account Account { get; set; } = null!;

    public ICollection<Residence> Residences { get; set; } = new List<Residence>();
}
