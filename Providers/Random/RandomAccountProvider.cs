using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomAccountProvider : IRandomEntityProvider<Account>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Account>? _accounts;

    public RandomAccountProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Account Provide()
    {
        _accounts ??= _dbContext.Accounts.AsNoTracking().ToList();

        return _randomDataGenerator.Pick(_accounts);
    }
}