using NpgsqlTypes;
using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class AddressEntityMocker : IEntityMocker<Address>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<City> _cityProvider;

    public AddressEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<City> cityProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _cityProvider = cityProvider;
    }

    public Address MockEntity()
    {
        var hasZipcode = _randomDataGenerator.NextBool(0.9);
        var city = _cityProvider.Provide();
        var geolocationX = _randomDataGenerator.NextDouble(-89, 89);
        var geolocationY = _randomDataGenerator.NextDouble(-89, 89);
        var geolocation = new NpgsqlPoint(geolocationX, geolocationY);
        
        return new Address
        {
            CityId = city.Id,
            Street = _randomDataGenerator.NextAlphaString(),
            Details = _randomDataGenerator.NextAlphaString(70),
            Zipcode = hasZipcode ? _randomDataGenerator.NextZipcode() : null,
            IsRural = _randomDataGenerator.NextBool(0.3),
            Geolocation = geolocation
        };
    }
}