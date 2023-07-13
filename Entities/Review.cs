namespace ResidenceMocker.Entities;

public sealed class Review
{
    public int Id { get; set; }
    
    public bool IsByHost { get; set; }

    public int RentId { get; set; }

    public short? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public Rent Rent { get; set; } = null!;
}
