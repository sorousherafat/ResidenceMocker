using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomProvinceProvider : IRandomEntityProvider<Province>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Province>? _provinces;

    public RandomProvinceProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Province Provide()
    {
        _provinces ??= _dbContext.Provinces.ToList();

        return _randomDataGenerator.Pick(_provinces);
    }
}