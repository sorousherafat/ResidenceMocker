using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomRentProvider : IRandomEntityProvider<Rent>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Rent>? _rents;

    public RandomRentProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Rent Provide()
    {
        _rents ??= _dbContext.Rents.AsNoTracking().ToList();

        return _randomDataGenerator.Pick(_rents);
    }
}