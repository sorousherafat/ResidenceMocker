using NpgsqlTypes;

namespace ResidenceMocker.Entities;

public sealed class Address
{
    public int Id { get; set; }

    public int CityId { get; set; }

    public string Street { get; set; } = null!;

    public string Details { get; set; } = null!;

    public string? Zipcode { get; set; }

    public NpgsqlPoint? Geolocation { get; set; }

    public bool? IsRural { get; set; }

    public City City { get; set; } = null!;

    public ICollection<Residence> Residences { get; set; } = new List<Residence>();
}
