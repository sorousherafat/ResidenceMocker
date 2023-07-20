namespace ResidenceMocker.Entities;

public sealed class Message
{
    public int Id { get; set; }
    
    public string? HostNationalId { get; set; }

    public string? GuestNationalId { get; set; }
    
    public bool SentByHost { get; set; }

    public string Text { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public Host? Host { get; set; }

    public Guest? Guest { get; set; }
}
