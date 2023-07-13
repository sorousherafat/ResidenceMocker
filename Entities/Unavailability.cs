namespace ResidenceMocker.Entities;

public sealed class Unavailability
{
    public int Id { get; set; }

    public int ResidenceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public Residence Residence { get; set; } = null!;
}
