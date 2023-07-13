namespace ResidenceMocker.Entities;

public sealed class City
{
    public int Id { get; set; }

    public int ProvinceId { get; set; }

    public string? Name { get; set; }

    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    public Province Province { get; set; } = null!;
}
