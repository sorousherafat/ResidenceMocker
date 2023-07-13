using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class AddressEntityMocker : IEntityMocker<Address>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<City> _cityProvider;

    public AddressEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<City> cityProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _cityProvider = cityProvider;
    }

    public Address MockEntity(int id)
    {
        var hasZipcode = _randomDataGenerator.NextBool(0.9);
        var city = _cityProvider.Provide();
        
        return new Address
        {
            CityId = city.Id,
            Street = _randomDataGenerator.NextAlphaString(),
            Details = _randomDataGenerator.NextAlphaString(70),
            Zipcode = hasZipcode ? _randomDataGenerator.NextZipcode() : null,
            IsRural = _randomDataGenerator.NextBool(0.3)
        };
    }
}