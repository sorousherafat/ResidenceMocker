using ResidenceMocker.Enums;

namespace ResidenceMocker.Entities;

public sealed class Residence
{
    public int Id { get; set; }

    public int AddressId { get; set; }

    public string HostNationalId { get; set; } = null!;

    public byte[]? PrimaryPhoto { get; set; }

    public byte[][]? AllPhotos { get; set; }

    public int? Price { get; set; }

    public int? RentFee { get; set; }

    public decimal Area { get; set; }

    public short? RoomsNo { get; set; }

    public short? Capacity { get; set; }

    public string? Facility { get; set; }

    public TimeOnly CheckInTime { get; set; }

    public TimeOnly CheckOutTime { get; set; }

    public string CancellationPolicy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? Title { get; set; }

    public bool? HasWifi { get; set; }

    public bool? HasParking { get; set; }
    
    public BuildingType BuildingType { get; set; }
    
    public BedType[] BedsType { get; set; }

    public Address Address { get; set; } = null!;

    public Host Host { get; set; } = null!;

    public ICollection<PriceChange> PriceChanges { get; set; } = new List<PriceChange>();

    public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

    public ICollection<Unavailability> Unavailabilities { get; set; } = new List<Unavailability>();
}
