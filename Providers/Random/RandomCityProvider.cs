using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomCityProvider : IRandomEntityProvider<City>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<City>? _cities;

    public RandomCityProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public City Provide()
    {
        _cities ??= _dbContext.Cities.ToList();

        return _randomDataGenerator.Pick(_cities);
    }
}