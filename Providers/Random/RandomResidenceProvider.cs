using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomResidenceProvider : IRandomEntityProvider<Residence>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Residence>? _residences;

    public RandomResidenceProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Residence Provide()
    {
        _residences ??= _dbContext.Residences.AsNoTracking().ToList();

        return _randomDataGenerator.Pick(_residences);
    }
}