using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class CityEntityMocker : IEntityMocker<City>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Province> _provinceProvider;

    public CityEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Province> provinceProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _provinceProvider = provinceProvider;
    }

    public City MockEntity(int id)
    {
        var province = _provinceProvider.Provide();
        
        return new City
        {
            Name = _randomDataGenerator.NextAlphaString(),
            ProvinceId = province.Id
        };
    }
}