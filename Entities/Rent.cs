using ResidenceMocker.Enums;

namespace ResidenceMocker.Entities;

public sealed class Rent
{
    public int Id { get; set; }

    public string CancellationPolicy { get; set; } = null!;

    public int? CancellationPenalty { get; set; }

    public DateTime? CancellationTimestamp { get; set; }

    public int FinalPrice { get; set; }
    
    public RentStatus Status { get; set; }

    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public ICollection<DamageReport> DamageReports { get; set; } = new List<DamageReport>();

    public RentalRequest Request { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
