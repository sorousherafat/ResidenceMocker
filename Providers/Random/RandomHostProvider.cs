using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomHostProvider : IRandomEntityProvider<Host>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Host>? _hosts;

    public RandomHostProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Host Provide()
    {
        _hosts ??= _dbContext.Hosts.AsNoTracking().ToList();

        return _randomDataGenerator.Pick(_hosts);
    }
}