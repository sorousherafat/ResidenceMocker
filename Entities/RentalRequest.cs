using ResidenceMocker.Enums;

namespace ResidenceMocker.Entities;

public sealed class RentalRequest
{
    public int Id { get; set; }

    public int? ResidenceId { get; set; }

    public string? GuestNationalId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public short? GuestsNo { get; set; }

    public int RawPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }
    
    public RentalRequestStatus Status { get; set; }

    public Guest? Guest { get; set; }

    public Rent? Rent { get; set; }

    public Residence? Residence { get; set; }
}
