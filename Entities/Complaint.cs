namespace ResidenceMocker.Entities;

public sealed class Complaint
{
    public int Id { get; set; }

    public int RentId { get; set; }

    public bool? IsAccepted { get; set; }

    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public byte[][]? Photos { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public Rent Rent { get; set; } = null!;
}
