namespace ResidenceMocker.Entities;

public sealed class Message
{
    public int Id { get; set; }
    
    public string? SenderNationalId { get; set; }

    public string? ReceiverNationalId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public Account? ReceiverAccount { get; set; }

    public Account? SenderAccount { get; set; }
}
